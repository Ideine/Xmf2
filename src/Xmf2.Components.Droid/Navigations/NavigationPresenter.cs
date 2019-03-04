using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Xmf2.Components.Extensions;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Navigations;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using IPresenterService = Xmf2.Components.Navigations.IPresenterService;

namespace Xmf2.Components.Droid.Navigations
{
	/*
	 * Dans le cas où l'on voudrait show plusieurs activity :
	 * Intent intent = new Intent (this, MainActivity.class);
     *  TaskStackBuilder stackBuilder = TaskStackBuilder.create(this);
     *  stackBuilder.addParentStack(MainActivity.class);
     *  stackBuilder.addNextIntent(intent);
     *  Intent intentEmailView = new Intent (this, EmailViewActivity.class);
     *  intentEmailView.putExtra("EmailId","you can Pass emailId here");
     *  stackBuilder.addNextIntent(intentEmailView);
	 */

	public interface IDeferredNavigationAction
	{
		void Execute(Activity activity);
	}

	public static class NavigationParameterContainer
	{
		private static readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();

		internal static string CreateNavigationParameter(object parameter)
		{
			string key = $"{parameter.GetType().Name}+{Guid.NewGuid():N}";
			_parameters.Add(key, parameter);
			return key;
		}

		public static IComponentViewModel GetViewModel(string key) => Get<IComponentViewModel>(key);
		public static IDeferredNavigationAction GetDeferredNavigationAction(string key) => Get<IDeferredNavigationAction>(key);

		private static T Get<T>(string key)
		{
			if (_parameters.TryGetValue(key, out object result))
			{
				return (T)result;
			}

			throw new ArgumentOutOfRangeException($"The key {key} does not match any navigation parameters");
		}
	}

	public interface IRegistrationPresenterService
	{
		void RegisterDefaultFragmentHost<TActivity>(bool shouldClearHistory = false)
			where TActivity : AppCompatActivity;

		void AssociateActivity<TActivity>(ScreenDefinition screenDefinition, bool shouldClearHistory = false)
			where TActivity : AppCompatActivity;

		void AssociateFragment<TFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TFragment> fragmentCreator, Type hostActivityType = null, bool? shouldClearHistory = null)
			where TFragment : Fragment;

		void AssociateFragment<TFragmentHost, TFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TFragment> fragmentCreator)
			where TFragmentHost : AppCompatActivity
			where TFragment : Fragment;

		void AssociateDialogFragment<TDialogFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TDialogFragment> fragmentCreator, Type hostActivityType = null, bool? shouldClearHistory = null)
			where TDialogFragment : DialogFragment;

		void AssociateDialogFragment<TFragmentHost, TDialogFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TDialogFragment> fragmentCreator)
			where TFragmentHost : AppCompatActivity
			where TDialogFragment : DialogFragment;
	}

	public class NavigationPresenter : IPresenterService, IRegistrationPresenterService
	{
		public const string FRAGMENT_START_PARAMETER_CODE = nameof(FRAGMENT_START_PARAMETER_CODE);
		public const string VIEWMODEL_LINK_PARAMETER_CODE = nameof(VIEWMODEL_LINK_PARAMETER_CODE);

		private readonly Dictionary<ScreenDefinition, ViewFactory> _factoryAssociation = new Dictionary<ScreenDefinition, ViewFactory>();
		private readonly NavigationStack _navigationStack = new NavigationStack();
		private ActivityViewFactory _defaultFragmentHost;

		private static Activity CurrentActivity
		{
			get
			{
				Activity result = CrossCurrentActivity.Current.Activity;
				if (result is null)
				{
					throw new InvalidOperationException("Can not resolve current activity");
				}

				return result;
			}
		}

		public void RegisterDefaultFragmentHost<TActivity>(bool shouldClearHistory = false)
			where TActivity : AppCompatActivity
		{
			_defaultFragmentHost = new ActivityViewFactory(typeof(TActivity), shouldClearHistory);
		}

		public void AssociateActivity<TActivity>(ScreenDefinition screenDefinition, bool shouldClearHistory = false)
			where TActivity : AppCompatActivity
		{
			_factoryAssociation[screenDefinition] = new ActivityViewFactory(typeof(TActivity), shouldClearHistory);
		}

		public void AssociateFragment<TFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TFragment> fragmentCreator, Type hostActivityType = null, bool? shouldClearHistory = null)
			where TFragment : Fragment
		{
			FragmentViewFactory res = null;
			if (hostActivityType is null)
			{
				if (_defaultFragmentHost is null)
				{
					throw new InvalidOperationException("No fragment host has been specified and none has been registered as default");
				}

				res = new FragmentViewFactory(fragmentCreator, _defaultFragmentHost.ActivityType, shouldClearHistory ?? _defaultFragmentHost.ShouldClearHistory);
			}
			else
			{
				res = new FragmentViewFactory(fragmentCreator, hostActivityType, shouldClearHistory ?? false);
			}

			_factoryAssociation[screenDefinition] = res;
		}

		public void AssociateFragment<TFragmentHost, TFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TFragment> fragmentCreator)
			where TFragmentHost : AppCompatActivity
			where TFragment : Fragment
		{
			AssociateFragment(screenDefinition, fragmentCreator, typeof(TFragmentHost));
		}

		public void AssociateDialogFragment<TDialogFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TDialogFragment> fragmentCreator, Type hostActivityType = null, bool? shouldClearHistory = null)
			where TDialogFragment : DialogFragment
		{
			DialogFragmentViewFactory res = null;
			if (hostActivityType is null)
			{
				if (_defaultFragmentHost is null)
				{
					throw new InvalidOperationException("No fragment host has been specified and none has been registered as default");
				}

				res = new DialogFragmentViewFactory(fragmentCreator, _defaultFragmentHost.ActivityType, shouldClearHistory ?? _defaultFragmentHost.ShouldClearHistory);
			}
			else
			{
				res = new DialogFragmentViewFactory(fragmentCreator, hostActivityType, shouldClearHistory ?? false);
			}

			_factoryAssociation[screenDefinition] = res;
		}

		public void AssociateDialogFragment<TFragmentHost, TDialogFragment>(ScreenDefinition screenDefinition, Func<IComponentViewModel, TDialogFragment> fragmentCreator)
			where TFragmentHost : AppCompatActivity
			where TDialogFragment : DialogFragment
		{
			AssociateDialogFragment<TDialogFragment>(screenDefinition, fragmentCreator, typeof(TFragmentHost));
		}

		public async Task UpdateNavigation(NavigationOperation navigationOperation, INavigationInProgress navigationInProgress)
		{
			if (navigationOperation.Pushes.Count == 0 && navigationOperation.Pops.Count == 0)
			{
				return;
			}

			List<PushInformation> controllersToPush = navigationOperation.Pushes.ConvertAll(x => new PushInformation(_factoryAssociation[x.Screen], x.Instance));

			foreach (NavigationOperation.Push push in navigationOperation.Pushes)
			{
				await push.Instance.GetViewModel(""); //TODO: add route here
			}

			if (navigationInProgress.IsCancelled)
			{
				Task.Run(async () =>
				{
					// we wait 10s just in case, shouldn't put too much memory pressure on GC
					await Task.Delay(10_000);

					foreach (NavigationOperation.Push push in navigationOperation.Pushes)
					{
						push.Instance.ViewModelInstance?.SafeDispose();
					}
				}).ConfigureAwait(false);
				return;
			}

			navigationInProgress.Commit();

			CurrentActivity.RunOnUiThread(() => { _navigationStack.ApplyActions(navigationOperation.Pops.Count, controllersToPush); });
		}

		public void CloseApp()
		{
			CurrentActivity.Finish();
		}

		#region Factories

		private abstract class ViewFactory
		{
		}

		private class FragmentViewFactory : ViewFactory
		{
			public Func<IComponentViewModel, Fragment> Creator { get; }

			public Type HostActivityType { get; }

			public bool ShouldClearHistory { get; }

			public FragmentViewFactory(Func<IComponentViewModel, Fragment> creator, Type hostActivityType, bool shouldClearHistory)
			{
				Creator = creator;
				HostActivityType = hostActivityType;
				ShouldClearHistory = shouldClearHistory;
			}
		}

		private class DialogFragmentViewFactory : ViewFactory
		{
			public Func<IComponentViewModel, DialogFragment> Creator { get; }

			public Type HostActivityType { get; }

			public bool ShouldClearHistory { get; }

			public DialogFragmentViewFactory(Func<IComponentViewModel, DialogFragment> creator, Type hostActivityType, bool shouldClearHistory)
			{
				Creator = creator;
				HostActivityType = hostActivityType;
				ShouldClearHistory = shouldClearHistory;
			}
		}

		private class ActivityViewFactory : ViewFactory
		{
			public Type ActivityType { get; }

			public bool ShouldClearHistory { get; }

			public ActivityViewFactory(Type activityType, bool shouldClearHistory)
			{
				ActivityType = activityType;
				ShouldClearHistory = shouldClearHistory;
			}
		}

		private class PushInformation
		{
			public ViewFactory Factory { get; }

			public ScreenInstance Instance { get; }

			public PushInformation(ViewFactory factory, ScreenInstance instance)
			{
				Factory = factory;
				Instance = instance;
			}
		}

		#endregion

		#region Stack management

		private class NavigationStack
		{
			private readonly List<InnerStack> _innerStacks = new List<InnerStack>();

			private InnerStack Top => _innerStacks[_innerStacks.Count - 1];

			public void ApplyActions(int popsCount, List<PushInformation> pushesCount)
			{
				List<PopOperation> pops = Pop(popsCount);
				List<PushOperation> pushes = Push(pushesCount);
				MergedPopPushOperation mergedOp = null;

				if (pops.Count > 0 && pushes.Count > 0 && TryMerge(pops[pops.Count - 1], pushes[0], out mergedOp))
				{
					pops.RemoveAt(pops.Count - 1);
					pushes.RemoveAt(0);
				}

				Activity activity = CurrentActivity;
				foreach (PopOperation pop in pops)
				{
					pop.Execute(activity);
				}

				mergedOp?.Execute(activity);

				foreach (PushOperation push in pushes)
				{
					push.Execute(activity);
				}

				Console.WriteLine("apply actions");
			}

			private List<PopOperation> Pop(int popCount)
			{
				if (popCount == 0 || _innerStacks.Count == 0)
				{
					return new List<PopOperation>();
				}

				List<PopOperation> popOperations = new List<PopOperation>();

				int lastInnerStackPopIndex = _innerStacks.Count;
				for (int i = _innerStacks.Count - 1 ; i >= 0 ; i--)
				{
					InnerStack item = _innerStacks[i];
					if (item.Count <= popCount)
					{
						popCount -= item.Count;
						popOperations.Add(item.AsPopOperation());
						lastInnerStackPopIndex = i;
					}
					else
					{
						break;
					}
				}

				if (lastInnerStackPopIndex < _innerStacks.Count)
				{
					_innerStacks.RemoveRange(lastInnerStackPopIndex, _innerStacks.Count - lastInnerStackPopIndex);
				}

				if (popCount > 0)
				{
					if (_innerStacks.Count == 0)
					{
						throw new InvalidOperationException("Trying to close more views than existing");
					}

					popOperations.Add(_innerStacks[_innerStacks.Count - 1].AsSpecificPopOperation(popCount));
				}

				//simplify list of pop operations
				int insertIndex = 0;
				for (int i = 1 ; i < popOperations.Count ; i++)
				{
					if (TryMerge(popOperations[insertIndex], popOperations[i], out PopOperation op))
					{
						popOperations[insertIndex] = op;
					}
					else
					{
						insertIndex++;
						if (insertIndex != i)
						{
							popOperations[insertIndex] = popOperations[i];
						}
					}
				}

				if (insertIndex != popOperations.Count - 1)
				{
					int firstIndexToRemove = insertIndex + 1;
					popOperations.RemoveRange(firstIndexToRemove, popOperations.Count - firstIndexToRemove);
				}

				return popOperations;
			}

			private List<PushOperation> Push(List<PushInformation> pushInformations)
			{
				List<PushOperation> pushOperations = new List<PushOperation>();
				InnerStack top = null;
				if (_innerStacks.Count > 0)
				{
					top = _innerStacks[_innerStacks.Count - 1];
				}

				foreach (PushInformation pushInformation in pushInformations)
				{
					if (pushInformation.Factory is ActivityViewFactory activityViewFactory)
					{
						ActivityInnerStack activityInnerStack = new ActivityInnerStack(this, activityViewFactory.ActivityType, activityIsOnlyAFragmentContainer: false, shouldClearHistory: activityViewFactory.ShouldClearHistory);
						pushOperations.Add(new ActivityPushOperation(activityInnerStack, pushInformation.Instance.ViewModelInstance));
						top = activityInnerStack;
						_innerStacks.Add(top);
					}
					else if (pushInformation.Factory is DialogFragmentViewFactory dialogFragmentViewFactory)
					{
						ActivityInnerStack host = GetActivityInnerStack(top);
						if (host == null)
						{
							//need to push the activity first
							ActivityInnerStack activityInnerStack = new ActivityInnerStack(this, dialogFragmentViewFactory.HostActivityType, activityIsOnlyAFragmentContainer: true, shouldClearHistory: dialogFragmentViewFactory.ShouldClearHistory);
							pushOperations.Add(new ActivityPushOperation(activityInnerStack, viewModel: null));
							top = host = activityInnerStack;
							_innerStacks.Add(top);
						}

						DialogFragmentInnerStack dialogFragmentInnerStack = new DialogFragmentInnerStack(this, host, dialogFragmentViewFactory.Creator(pushInformation.Instance.ViewModelInstance));
						pushOperations.Add(new FragmentPushOperation(host)
						{
							FragmentStacksToPush =
							{
								dialogFragmentInnerStack
							}
						});
						top = dialogFragmentInnerStack;
						_innerStacks.Add(top);
					}
					else if (pushInformation.Factory is FragmentViewFactory fragmentViewFactory)
					{
						ActivityInnerStack host = GetActivityInnerStack(top);
						if (host == null)
						{
							//need to push the activity first
							ActivityInnerStack activityInnerStack = new ActivityInnerStack(this, fragmentViewFactory.HostActivityType, activityIsOnlyAFragmentContainer: true, shouldClearHistory: fragmentViewFactory.ShouldClearHistory);
							pushOperations.Add(new ActivityPushOperation(activityInnerStack, viewModel: null));
							top = host = activityInnerStack;
							_innerStacks.Add(top);
						}

						FragmentInnerStack fragmentInnerStack = new FragmentInnerStack(this, host, fragmentViewFactory.Creator(pushInformation.Instance.ViewModelInstance));
						pushOperations.Add(new FragmentPushOperation(host)
						{
							FragmentStacksToPush =
							{
								fragmentInnerStack
							}
						});
						host.FragmentStack.Add(fragmentInnerStack);
					}
					else
					{
						throw new InvalidOperationException("This kind of factory is not supported...");
					}

					ActivityInnerStack GetActivityInnerStack(InnerStack item)
					{
						if (item is null)
						{
							return null;
						}

						if (item is ActivityInnerStack res)
						{
							return res;
						}

						if (item is DialogFragmentInnerStack dialogFragmentInnerStack)
						{
							return dialogFragmentInnerStack.FragmentHost;
						}

						if (item is FragmentInnerStack fragmentInnerStack)
						{
							return fragmentInnerStack.FragmentHost;
						}

						throw new InvalidOperationException($"Unsupported inner stack type : {item.GetType().Name}");
					}
				}

				//simplify list of pop operations
				int insertIndex = 0;
				for (int i = 1 ; i < pushOperations.Count ; i++)
				{
					if (TryMerge(pushOperations[insertIndex], pushOperations[i], out PushOperation op))
					{
						pushOperations[insertIndex] = op;
					}
					else
					{
						insertIndex++;
						if (insertIndex != i)
						{
							pushOperations[insertIndex] = pushOperations[i];
						}
					}
				}

				if (insertIndex < pushOperations.Count - 1)
				{
					int firstIndexToRemove = insertIndex + 1;
					pushOperations.RemoveRange(firstIndexToRemove, pushOperations.Count - firstIndexToRemove);
				}

				return pushOperations;
			}

			private bool TryMerge(PopOperation op1, PopOperation op2, out PopOperation res)
			{
				if (op1 is FragmentPopOperation fragmentPopOperation)
				{
					if (op2 is FragmentPopOperation fragmentPopOperation2)
					{
						if (fragmentPopOperation.HostStack == fragmentPopOperation2.HostStack)
						{
							fragmentPopOperation.FragmentStacksToPop.AddRange(fragmentPopOperation2.FragmentStacksToPop);
							res = fragmentPopOperation;
							return true;
						}
					}
					else if (op2 is ActivityPopOperation finishActivityPopOperation)
					{
						if (fragmentPopOperation.HostStack == finishActivityPopOperation.ActivityStack)
						{
							// avoid popping fragment if activity will be stopped anyway
							res = finishActivityPopOperation;
							return true;
						}
					}
				}

				res = null;
				return false;
			}

			private bool TryMerge(PushOperation op1, PushOperation op2, out PushOperation res)
			{
				if (op1 is ActivityPushOperation startActivityPushOperation)
				{
					if (op2 is FragmentPushOperation showFragmentPushOperation)
					{
						if (startActivityPushOperation.ActivityStack == showFragmentPushOperation.HostStack)
						{
							//merge show fragment in start activity
							startActivityPushOperation.FragmentStacksToPush.AddRange(showFragmentPushOperation.FragmentStacksToPush);
							res = startActivityPushOperation;
							return true;
						}
					}
				}
				else if (op1 is FragmentPushOperation showFragmentPushOperation1 && op2 is FragmentPushOperation showFragmentPushOperation2)
				{
					if (showFragmentPushOperation1.HostStack == showFragmentPushOperation2.HostStack)
					{
						showFragmentPushOperation1.FragmentStacksToPush.AddRange(showFragmentPushOperation2.FragmentStacksToPush);
						res = showFragmentPushOperation1;
						return true;
					}
				}

				res = null;
				return false;
			}

			private bool TryMerge(PopOperation popOp, PushOperation pushOp, out MergedPopPushOperation res)
			{
				if (popOp is FragmentPopOperation fragmentPopOperation && pushOp is FragmentPushOperation fragmentPushOperation)
				{
					if (fragmentPopOperation.HostStack == fragmentPushOperation.HostStack)
					{
						res = new MergedFragmentPopPushOperation(fragmentPopOperation.HostStack, fragmentPopOperation.FragmentStacksToPop, fragmentPushOperation.FragmentStacksToPush);
						return true;
					}
				}

				if (popOp is ActivityPopOperation activityPopOperation && pushOp is ActivityPushOperation activityPushOperation)
				{
					res = new MergedActivityPopPushOperation(activityPopOperation, activityPushOperation);
					return true;
				}

				res = null;
				return false;
			}

			#region Inner stacks

			private abstract class InnerStack
			{
				public NavigationStack NavigationStack { get; }

				protected InnerStack(NavigationStack navigationStack)
				{
					NavigationStack = navigationStack;
				}

				public abstract int Count { get; }

				public abstract PopOperation AsPopOperation();

				public abstract PopOperation AsSpecificPopOperation(InnerStack child);
				public abstract PopOperation AsSpecificPopOperation(int count);
			}

			private class ActivityInnerStack : InnerStack
			{
				public bool ActivityIsOnlyAFragmentContainer { get; }

				public Type ActivityType { get; }

				public bool ShouldClearHistory { get; }

				public List<InnerStack> FragmentStack { get; } = new List<InnerStack>();

				public ActivityInnerStack(NavigationStack navigationStack, Type activityType, bool activityIsOnlyAFragmentContainer, bool shouldClearHistory) : base(navigationStack)
				{
					ActivityType = activityType;
					ActivityIsOnlyAFragmentContainer = activityIsOnlyAFragmentContainer;
					ShouldClearHistory = shouldClearHistory;
				}

				public override int Count
				{
					get
					{
						if (ActivityIsOnlyAFragmentContainer)
						{
							return FragmentStack.Sum(x => x.Count);
						}

						return FragmentStack.Sum(x => x.Count) + 1;
					}
				}

				public override PopOperation AsPopOperation()
				{
					FragmentStack.Clear();
					//TODO: do we really want to finish the activity if it's the last one of the app ?
					return new ActivityPopOperation(this);
				}

				public override PopOperation AsSpecificPopOperation(InnerStack child)
				{
					if (child is FragmentInnerStack fragmentInnerStack)
					{
						FragmentStack.RemoveAt(FragmentStack.Count - 1);
						return new FragmentPopOperation(this)
						{
							FragmentStacksToPop =
							{
								fragmentInnerStack
							}
						};
					}

					throw new InvalidOperationException("Specific pop operation on unsupported child type");
				}

				public override PopOperation AsSpecificPopOperation(int count)
				{
					FragmentPopOperation result = new FragmentPopOperation(this);
					for (int i = 0, index = FragmentStack.Count - 1 ; i < count ; ++i, index--)
					{
						if (FragmentStack[index] is DialogFragmentInnerStack dialogFragmentInnerStack)
						{
							result.FragmentStacksToPop.Add(dialogFragmentInnerStack);
						}
						else if (FragmentStack[index] is FragmentInnerStack fragmentInnerStack)
						{
							result.FragmentStacksToPop.Add(fragmentInnerStack);
						}
						else
						{
							throw new InvalidOperationException("Specific pop operation on unsupported child type");
						}
					}

					FragmentStack.RemoveRange(FragmentStack.Count - count, count);

					return result;
				}
			}

			private interface IFragmentInnerStack
			{
				Fragment Fragment { get; }

				string FragmentTag { get; }
			}

			private class FragmentInnerStack : InnerStack, IFragmentInnerStack
			{
				public ActivityInnerStack FragmentHost { get; }

				public Fragment Fragment { get; }

				public string FragmentTag { get; }

				public FragmentInnerStack(NavigationStack navigationStack, ActivityInnerStack fragmentHost, Fragment fragment) : base(navigationStack)
				{
					FragmentHost = fragmentHost;
					Fragment = fragment;
					FragmentTag = $"{Fragment.GetType().Name}+{Guid.NewGuid():N}";
				}

				public override int Count => 1;

				public override PopOperation AsPopOperation() => FragmentHost.AsSpecificPopOperation(this);

				public override PopOperation AsSpecificPopOperation(InnerStack child)
				{
					throw new InvalidOperationException("Operation not supported, fragment can not have children");
				}

				public override PopOperation AsSpecificPopOperation(int count)
				{
					throw new InvalidOperationException("Operation not supported, fragment can not have children");
				}
			}

			private class DialogFragmentInnerStack : InnerStack, IFragmentInnerStack
			{
				public ActivityInnerStack FragmentHost { get; }

				public DialogFragment Fragment { get; }
				Fragment IFragmentInnerStack.Fragment => Fragment;

				public string FragmentTag { get; }

				public DialogFragmentInnerStack(NavigationStack navigationStack, ActivityInnerStack fragmentHost, DialogFragment fragment) : base(navigationStack)
				{
					FragmentHost = fragmentHost;
					Fragment = fragment;
					FragmentTag = $"{Fragment.GetType().Name}+{Guid.NewGuid():N}";
				}

				public override int Count => 1;

				public override PopOperation AsPopOperation()
				{
					return new FragmentPopOperation(FragmentHost)
					{
						FragmentStacksToPop =
						{
							this
						}
					};
				}

				public override PopOperation AsSpecificPopOperation(InnerStack child)
				{
					throw new InvalidOperationException("Operation not supported, fragment can not have children");
				}

				public override PopOperation AsSpecificPopOperation(int count)
				{
					throw new InvalidOperationException("Operation not supported, fragment can not have children");
				}
			}

			#endregion

			private abstract class Operation
			{
				public abstract void Execute(Activity activity);
			}

			#region Pop operations

			private abstract class PopOperation : Operation
			{
			}

			private class ActivityPopOperation : PopOperation
			{
				public ActivityInnerStack ActivityStack { get; }

				public ActivityPopOperation(ActivityInnerStack activityStack)
				{
					ActivityStack = activityStack;
				}

				public override void Execute(Activity activity)
				{
					//TODO: we could have some issue here if we need to close multiple activities at once
					CurrentActivity.Finish();
				}
			}

			private class FragmentPopOperation : PopOperation
			{
				public ActivityInnerStack HostStack { get; }

				public List<IFragmentInnerStack> FragmentStacksToPop { get; } = new List<IFragmentInnerStack>();

				public FragmentPopOperation(ActivityInnerStack hostStack)
				{
					HostStack = hostStack;
				}

				public override void Execute(Activity activity)
				{
					if (activity is AppCompatActivity appCompatActivity)
					{
						UpdateFragments(HostStack.NavigationStack, appCompatActivity, FragmentStacksToPop, null, activity as IFragmentActivity);
					}
				}
			}

			#endregion

			#region Push operations

			private abstract class PushOperation : Operation
			{
			}

			private class ActivityPushOperation : PushOperation
			{
				public ActivityInnerStack ActivityStack { get; }

				public IComponentViewModel ViewModel { get; }

				public List<IFragmentInnerStack> FragmentStacksToPush { get; } = new List<IFragmentInnerStack>();

				public ActivityPushOperation(ActivityInnerStack activityStack, IComponentViewModel viewModel)
				{
					ActivityStack = activityStack;
					ViewModel = viewModel;
				}

				public override void Execute(Activity activity)
				{
					Intent intent = new Intent(activity, ActivityStack.ActivityType);
					if (ActivityStack.ShouldClearHistory)
					{
						intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
					}

					if (ViewModel != null)
					{
						intent.PutExtra(VIEWMODEL_LINK_PARAMETER_CODE, NavigationParameterContainer.CreateNavigationParameter(ViewModel));
					}

					if (FragmentStacksToPush.Count > 0)
					{
						FragmentPushOperation operation = new FragmentPushOperation(ActivityStack);
						operation.FragmentStacksToPush.AddRange(FragmentStacksToPush);
						intent.PutExtra(FRAGMENT_START_PARAMETER_CODE, NavigationParameterContainer.CreateNavigationParameter(new DeferredNavigationAction(operation)));
					}

					activity.StartActivity(intent);
				}
			}

			private class FragmentPushOperation : PushOperation
			{
				public ActivityInnerStack HostStack { get; }

				public List<IFragmentInnerStack> FragmentStacksToPush { get; } = new List<IFragmentInnerStack>();

				public FragmentPushOperation(ActivityInnerStack hostStack)
				{
					HostStack = hostStack;
				}

				public override void Execute(Activity activity)
				{
					if (activity is AppCompatActivity appCompatActivity)
					{
						UpdateFragments(HostStack.NavigationStack, appCompatActivity, null, FragmentStacksToPush, activity as IFragmentActivity);
					}
				}
			}

			#endregion

			#region Merged operation

			private abstract class MergedPopPushOperation : Operation
			{
			}

			private class MergedFragmentPopPushOperation : MergedPopPushOperation
			{
				public ActivityInnerStack HostStack { get; }

				public List<IFragmentInnerStack> FragmentStacksToPop { get; }
				public List<IFragmentInnerStack> FragmentStacksToPush { get; }

				public MergedFragmentPopPushOperation(ActivityInnerStack hostStack, List<IFragmentInnerStack> fragmentStacksToPop, List<IFragmentInnerStack> fragmentStacksToPush)
				{
					HostStack = hostStack;
					FragmentStacksToPop = fragmentStacksToPop;
					FragmentStacksToPush = fragmentStacksToPush;
				}

				public override void Execute(Activity activity)
				{
					if (activity is AppCompatActivity appCompatActivity)
					{
						UpdateFragments(HostStack.NavigationStack, appCompatActivity, FragmentStacksToPop, FragmentStacksToPush, activity as IFragmentActivity);
					}
				}
			}

			private class MergedActivityPopPushOperation : MergedPopPushOperation
			{
				public ActivityPopOperation PopActivity { get; }

				public ActivityPushOperation PushActivity { get; }

				public MergedActivityPopPushOperation(ActivityPopOperation popActivity, ActivityPushOperation pushActivity)
				{
					PopActivity = popActivity;
					PushActivity = pushActivity;
				}

				public override void Execute(Activity activity)
				{
					PushActivity.Execute(activity);
					PopActivity.Execute(activity);
				}
			}

			private static void UpdateFragments(NavigationStack navigationStack, AppCompatActivity appCompatActivity, List<IFragmentInnerStack> fragmentsToPop, List<IFragmentInnerStack> fragmentsToPush, IFragmentActivity fragmentActivity)
			{
				FragmentTransaction transaction = null;

				if (fragmentsToPop != null)
				{
					List<FragmentInnerStack> fragmentListToPop = null;
					foreach (IFragmentInnerStack fragmentStack in fragmentsToPop)
					{
						if (fragmentStack is DialogFragmentInnerStack dialogFragmentInnerStack)
						{
							dialogFragmentInnerStack.Fragment.DismissAllowingStateLoss();
						}
						else if (fragmentStack is FragmentInnerStack fragmentInnerStack)
						{
							if (fragmentListToPop is null)
							{
								fragmentListToPop = new List<FragmentInnerStack>(fragmentsToPop.Count);
							}

							fragmentListToPop.Add(fragmentInnerStack);
						}
					}

					if (fragmentListToPop != null)
					{
						transaction = appCompatActivity.SupportFragmentManager.BeginTransaction();
						foreach (FragmentInnerStack fragmentStack in fragmentListToPop)
						{
							transaction = transaction.Remove(fragmentStack.Fragment);
						}
					}
				}

				if (fragmentsToPush != null)
				{
					List<DialogFragmentInnerStack> dialogFragmentsToPush = null;
					foreach (IFragmentInnerStack fragmentStack in fragmentsToPush)
					{
						if (fragmentStack is DialogFragmentInnerStack dialogFragmentInnerStack)
						{
							if (dialogFragmentsToPush is null)
							{
								dialogFragmentsToPush = new List<DialogFragmentInnerStack>(fragmentsToPush.Count);
							}

							dialogFragmentsToPush.Add(dialogFragmentInnerStack);
						}
						else if (fragmentStack is FragmentInnerStack fragmentInnerStack && fragmentActivity != null)
						{
							if (transaction is null)
							{
								transaction = appCompatActivity.SupportFragmentManager.BeginTransaction();
							}

							transaction.AddToBackStack(fragmentStack.FragmentTag);
							transaction = transaction.Replace(fragmentActivity.FragmentContainerId, fragmentStack.Fragment, fragmentStack.FragmentTag);
						}
					}

					transaction?.CommitAllowingStateLoss();

					if (dialogFragmentsToPush != null)
					{
						foreach (DialogFragmentInnerStack dialogFragment in dialogFragmentsToPush)
						{
							dialogFragment.Fragment.Show(appCompatActivity.SupportFragmentManager, dialogFragment.FragmentTag);
						}
					}
				}
				else
				{
					if (transaction != null && navigationStack.Top is ActivityInnerStack activityTop && activityTop.FragmentStack.Count > 0)
					{
						if (activityTop.FragmentStack[activityTop.FragmentStack.Count - 1] is FragmentInnerStack fragmentInnerStack)
						{
							transaction = transaction.Replace(fragmentActivity.FragmentContainerId, fragmentInnerStack.Fragment, fragmentInnerStack.FragmentTag);
						}
					}

					transaction?.CommitAllowingStateLoss();
				}
			}

			#endregion

			private class DeferredNavigationAction : IDeferredNavigationAction
			{
				private readonly Operation _deferredOperation;

				public DeferredNavigationAction(Operation deferredOperation)
				{
					_deferredOperation = deferredOperation;
				}

				public void Execute(Activity activity)
				{
					_deferredOperation.Execute(activity);
				}
			}
		}

		#endregion
	}

	public interface IFragmentActivity
	{
		int FragmentContainerId { get; }
	}
}