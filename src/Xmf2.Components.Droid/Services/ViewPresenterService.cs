using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Xmf2.Components.Droid.Fragments;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;
using DialogFragment = Android.Support.V4.App.DialogFragment;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace Xmf2.Components.Droid.Services
{
	public class ViewPresenterService : BaseServiceContainer, IViewPresenterService
	{
		protected Activity CurrentActivity
		{
			get
			{
				Activity current = CrossCurrentActivity.Current.Activity;
				if (current == null)
				{
					throw new InvalidOperationException("Can not resolve current activity");
				}

				return current;
			}
		}

		public ViewPresenterService(IServiceLocator services) : base(services) { }

		public void Close()
		{
			if (CurrentActivity is AppCompatActivity act)
			{
				if (act.SupportFragmentManager.Fragments.LastOrDefault() is DialogFragment frag)
				{
					CurrentActivity.RunOnUiThread(() => frag.DismissAllowingStateLoss());
				}
				else if (act.SupportFragmentManager.BackStackEntryCount > 1)
				{
					CurrentActivity.RunOnUiThread(() => act.SupportFragmentManager.PopBackStackImmediate());
				}
				else
				{
					CurrentActivity.Finish();
				}
			}
			else
			{
				CurrentActivity.Finish();
			}
		}

		public void CloseModal()
		{
			if (CurrentActivity is AppCompatActivity act)
			{
				if (act.SupportFragmentManager.Fragments.LastOrDefault() is DialogFragment frag)
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						try
						{
							frag.DismissAllowingStateLoss();
						}
						catch (ObjectDisposedException)
						{
							//problème avec le dispose java
						}
						catch (ArgumentException)
						{
							//problème avec le dispose java
						}
					});
				}
			}
		}

		public bool IsCurrentViewFor<TViewModel>()
		{
			if (CurrentActivity is AppCompatActivity compatActivity)
			{
				var frag = compatActivity.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));

				var tcs = new TaskCompletionSource<bool>();

				if (frag != null)
				{
					CurrentActivity.RunOnUiThread(() => tcs.SetResult(frag is IViewFor viewFor && viewFor.ViewModelType == typeof(TViewModel)));
				}
				else
				{
					CurrentActivity.RunOnUiThread(() => tcs.SetResult(CurrentActivity is IViewFor viewFor && viewFor.ViewModelType == typeof(TViewModel)));
				}

				return tcs.Task.Result;
			}
			else
			{
				return false;
			}
		}

		public void Show(Intent intent)
		{
			CurrentActivity.StartActivity(intent);
		}

		public void ShowView<TActivity>(bool clearHistory = false) where TActivity : AppCompatActivity
		{
			if (!(CrossCurrentActivity.Current is TActivity))
			{
				Show(CreateIntentForType<TActivity>(clearHistory));
			}
		}

		public void ShowView(DialogFragment fragment, string tag)
		{
			if (CurrentActivity is AppCompatActivity appCompatActivity && !appCompatActivity.IsFinishing)
			{
				FragmentManager fragmentManager = appCompatActivity.SupportFragmentManager;

				//remove existant
				Fragment previousDialogView = fragmentManager.FindFragmentByTag(tag);
				if (previousDialogView != null)
				{
					fragmentManager
						.BeginTransaction()
						.Remove(previousDialogView)
						.CommitAllowingStateLoss();

					fragmentManager.ExecutePendingTransactions();
				}

				fragment.Show(fragmentManager, tag);
			}
		}

		public void ShowView<TFragment>(string tag) where TFragment : DialogFragment, new()
		{
			ShowView(new TFragment(), tag);
		}

		public void ShowFragment<TActivity, TFragment>()
			where TActivity : AppCompatActivity, IFragmentActivity
			where TFragment : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				CurrentActivity.RunOnUiThread(() =>
				{
					BackToRootFragmentInUIThread(act);
					act.SupportFragmentManager.PopBackStackImmediate();
					act.ShowFragment<TFragment>();
				});
			}
			else
			{
				var intent = CreateIntentForType<TActivity>(true);
				var fragResolver = Services.Resolve<IFragmentResolverService>();
				var fragExtra = fragResolver.GetCode(typeof(TFragment));
				intent.PutExtra(FragmentResolverService.LIST_ENTRY_POINT, new[] { fragExtra });
				Show(intent);
			}
		}

		public void ShowFragment<TActivity, TRootFragment, TFragment>()
			where TActivity : AppCompatActivity, IFragmentActivity
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				CurrentActivity.RunOnUiThread(() =>
				{
					BackToRootFragmentInUIThread(act);
					var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
					if (topFrag is TRootFragment)
					{
						act.ShowFragment<TFragment>();
					}
					else
					{
						act.SupportFragmentManager.PopBackStackImmediate();
						act.ShowFragment<TRootFragment>();
						act.ShowFragment<TFragment>();
					}
				});
			}
			else
			{
				var intent = CreateIntentForType<TActivity>(true);
				var fragResolver = Services.Resolve<IFragmentResolverService>();
				var rootExtra = fragResolver.GetCode(typeof(TRootFragment));
				var fragExtra = fragResolver.GetCode(typeof(TFragment));
				intent.PutExtra(FragmentResolverService.LIST_ENTRY_POINT, new[] { rootExtra, fragExtra });
				Show(intent);
			}
		}

		public void ShowFragment<TActivity, TRootFragment, TFragment, TFragmentBis>(bool forceCreate = false)
			where TActivity : AppCompatActivity, IFragmentActivity
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
			where TFragmentBis : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				if (forceCreate)
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						BackToRootFragmentInUIThread(act);
						var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
						if (!(topFrag is TRootFragment))
						{
							act.SupportFragmentManager.PopBackStackImmediate();
							act.ShowFragment<TRootFragment>();
						}

						act.ShowFragment<TFragment>();
						act.ShowFragment<TFragmentBis>();
					});
				}
				else
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						bool needToCreate = true;
						while (act.SupportFragmentManager.BackStackEntryCount > 1)
						{
							if (act.SupportFragmentManager.GetTopFragment() is TFragment)
							{
								needToCreate = false;
								break;
							}

							act.SupportFragmentManager.PopBackStackImmediate();
						}

						if (needToCreate)
						{
							var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
							if (!(topFrag is TRootFragment))
							{
								act.SupportFragmentManager.PopBackStackImmediate();
								act.ShowFragment<TRootFragment>();
							}

							act.ShowFragment<TFragment>();
						}

						act.ShowFragment<TFragmentBis>();
					});
				}
			}
			else
			{
				var intent = CreateIntentForType<TActivity>(true);
				var fragResolver = Services.Resolve<IFragmentResolverService>();
				var rootExtra = fragResolver.GetCode(typeof(TRootFragment));
				var fragExtra = fragResolver.GetCode(typeof(TFragment));
				var fragbBisExtra = fragResolver.GetCode(typeof(TFragmentBis));
				intent.PutExtra(FragmentResolverService.LIST_ENTRY_POINT, new[] { rootExtra, fragExtra, fragbBisExtra });
				Show(intent);
			}
		}

		public void ShowFragment<TActivity, TRootFragment, TFragment, TFragmentBis, TFragmentTer>(bool forceCreateFirst = false, bool forceCreateSecond = false)
			where TActivity : AppCompatActivity, IFragmentActivity
			where TRootFragment : Fragment, new()
			where TFragment : Fragment, new()
			where TFragmentBis : Fragment, new()
			where TFragmentTer : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				if (forceCreateFirst)
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						BackToRootFragmentInUIThread(act);
						var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
						if (!(topFrag is TRootFragment))
						{
							act.SupportFragmentManager.PopBackStackImmediate();
							act.ShowFragment<TRootFragment>();
						}

						act.ShowFragment<TFragment>();
						act.ShowFragment<TFragmentBis>();
						act.ShowFragment<TFragmentTer>();
					});
				}
				else if (forceCreateSecond)
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						bool needToCreateFirst = true;
						while (act.SupportFragmentManager.BackStackEntryCount > 1)
						{
							if (act.SupportFragmentManager.GetTopFragment() is TFragment)
							{
								needToCreateFirst = false;
								break;
							}

							act.SupportFragmentManager.PopBackStackImmediate();
						}

						if (needToCreateFirst)
						{
							var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
							if (!(topFrag is TRootFragment))
							{
								act.SupportFragmentManager.PopBackStackImmediate();
								act.ShowFragment<TRootFragment>();
							}

							act.ShowFragment<TFragment>();
						}

						act.ShowFragment<TFragmentBis>();
						act.ShowFragment<TFragmentTer>();
					});
				}
				else
				{
					CurrentActivity.RunOnUiThread(() =>
					{
						bool needToCreateFirst = true;
						bool needToCreateSecond = true;
						while (act.SupportFragmentManager.BackStackEntryCount > 1)
						{
							var topFrag = act.SupportFragmentManager.GetTopFragment();
							if (topFrag is TFragmentBis)
							{
								needToCreateFirst = needToCreateSecond = false;
								break;
							}

							if (topFrag is TFragment)
							{
								needToCreateFirst = false;
							}

							act.SupportFragmentManager.PopBackStackImmediate();
						}

						if (needToCreateFirst)
						{
							var topFrag = act.SupportFragmentManager.Fragments.LastOrDefault(x => x.IsVisible && !(x is DialogFragment));
							if (!(topFrag is TRootFragment))
							{
								act.SupportFragmentManager.PopBackStackImmediate();
								act.ShowFragment<TRootFragment>();
							}

							act.ShowFragment<TFragment>();
						}

						if (needToCreateSecond)
						{
							act.ShowFragment<TFragmentBis>();
						}

						act.ShowFragment<TFragmentTer>();
					});
				}
			}
		}

		public void ShowRootFragment<TActivity, TFragment>() where TActivity : AppCompatActivity, IFragmentActivity where TFragment : Fragment, new()
		{
			if (CurrentActivity is TActivity act && act.SupportFragmentManager.Fragments.First() is TFragment)
			{
				BackToRootFragment(act);
			}
			else
			{
				ShowNewFragment<TActivity, TFragment>(true);
			}
		}

		protected void ShowNewFragment<TActivity, TFragment>(bool clearHistory) where TActivity : AppCompatActivity, IFragmentActivity where TFragment : Fragment
		{
			var intent = CreateIntentForType<TActivity>(clearHistory);
			if (intent != null)
			{
				intent.PutExtra(FragmentResolverService.ENTRY_POINT, Services.Resolve<IFragmentResolverService>().GetCode(typeof(TFragment)));
				Show(intent);
			}
		}

		public void ReplaceFragment<TActivity, TFragment>() where TActivity : AppCompatActivity, IFragmentActivity where TFragment : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				act.RunOnUiThread(() =>
				{
					act.SupportFragmentManager.PopBackStackImmediate();
					act.ShowFragment<TFragment>();
				});
			}
			else
			{
				ShowNewFragment<TActivity, TFragment>(clearHistory: false);
			}
		}

		public void PushFragment<TActivity, TFragment>() where TActivity : AppCompatActivity, IFragmentActivity where TFragment : Fragment, new()
		{
			CloseModal();
			if (CurrentActivity is TActivity act)
			{
				CurrentActivity.RunOnUiThread(() => act.ShowFragment<TFragment>());
			}
			else
			{
				ShowNewFragment<TActivity, TFragment>(clearHistory: false);
			}
		}

		protected void BackToRootFragment(FragmentActivity act)
		{
			CurrentActivity.RunOnUiThread(() => BackToRootFragmentInUIThread(act));
		}

		private void BackToRootFragmentInUIThread(FragmentActivity act)
		{
			act.SupportFragmentManager.Fragments.OfType<DialogFragment>().Where(x => x.IsVisible).ToList().ForEach(x => x.DismissAllowingStateLoss());

			while (act.SupportFragmentManager.BackStackEntryCount > 1)
			{
				act.SupportFragmentManager.PopBackStackImmediate();
			}
		}

		protected virtual Intent CreateIntentForType<TActivity>(bool clearHistory) where TActivity : AppCompatActivity
		{
			Intent intent = new Intent(CurrentActivity, typeof(TActivity));
			if (clearHistory)
			{
				intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
			}

			return intent;
		}
	}
}