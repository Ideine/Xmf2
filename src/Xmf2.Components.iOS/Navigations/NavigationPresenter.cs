using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Xmf2.Components.Extensions;
using Xmf2.Components.iOS.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Navigations;
using IPresenterService = Xmf2.Components.Navigations.IPresenterService;

namespace Xmf2.Components.iOS.Navigations
{
	public delegate UIViewController ViewCreator(IComponentViewModel viewModel);

	public interface IRegistrationPresenterService
	{
		void Associate(ScreenDefinition screenDefinition, ViewCreator controllerFactory);
		void AssociateModal(ScreenDefinition screenDefinition, ViewCreator controllerFactory);
	}

	public class NavigationPresenter : IPresenterService, IRegistrationPresenterService
	{
		private readonly Dictionary<ScreenDefinition, ControllerInformation> _factoryAssociation = new Dictionary<ScreenDefinition, ControllerInformation>();
		private readonly UIWindow _window;
		private readonly NavigationStack _navigationStack = new NavigationStack();

		public NavigationPresenter(UIWindow window)
		{
			_window = window;
		}

		public void Associate(ScreenDefinition screenDefinition, ViewCreator controllerFactory)
		{
			_factoryAssociation[screenDefinition] = new ControllerInformation(controllerFactory, isModal: false);
		}

		public void AssociateModal(ScreenDefinition screenDefinition, ViewCreator controllerFactory)
		{
			_factoryAssociation[screenDefinition] = new ControllerInformation(controllerFactory, isModal: true);
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
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				CallbackActionWaiter callbackActionWaiter = new CallbackActionWaiter();
				callbackActionWaiter.WaitOne();
				_navigationStack.EnsureInitialized(_window);

				_navigationStack.ApplyActions(navigationOperation.Pops.Count, controllersToPush, callbackActionWaiter);

				// lines below could be commented if we encounter issues with viewmodel disposing
				callbackActionWaiter.Add(() =>
				{
					foreach (NavigationOperation.Pop pop in navigationOperation.Pops)
					{
						pop.Instance.ViewModelInstance?.DispatchSafeDispose();
					}
				});

				Task.Run(async () =>
				{
					// we wait 10s to let the time for navigation controller animations before disposing content
					await Task.Delay(10_000);
					UIApplication.SharedApplication.InvokeOnMainThread(callbackActionWaiter.ReleaseOne);
				});
			});
		}

		public void CloseApp()
		{
			throw new InvalidOperationException("Cannot be perform in this platform");
		}

		#region Factory types

		private class ControllerInformation
		{
			public ViewCreator Factory { get; }

			public bool IsModal { get; }

			public ControllerInformation(ViewCreator factory, bool isModal)
			{
				Factory = factory;
				IsModal = isModal;
			}
		}

		private class PushInformation
		{
			public ControllerInformation Controller { get; }

			public ScreenInstance Screen { get; }

			public PushInformation(ControllerInformation controller, ScreenInstance screen)
			{
				Controller = controller;
				Screen = screen;
			}
		}

		#endregion

		#region Stack management

		private class NavigationStack
		{
			private readonly List<InnerStack> _innerStacks = new List<InnerStack>();

			public void EnsureInitialized(UIWindow window)
			{
				if (window.RootViewController is UINavigationController)
				{
					return;
				}

				UINavigationController navigationController = new HandleFreeRotateNavigationController();
				window.RootViewController = navigationController;

				_innerStacks.Add(new NavigationControllerInnerStack(navigationController, null));
			}

			public void ApplyActions(int popsCount, List<PushInformation> pushesCount, CallbackActionWaiter callbackActionWaiter)
			{
				List<PopOperation> pops = Pop(popsCount);
				List<PushOperation> pushes = Push(pushesCount);
				MergedPopPushOperation mergedOp = null;

				if (pops.Count > 0 && pushes.Count > 0 && TryMerge(pops[pops.Count - 1], pushes[0], out mergedOp))
				{
					pops.RemoveAt(pops.Count - 1);
					pushes.RemoveAt(0);
				}

				int popAnimatedIndex = mergedOp is null && pushes.Count == 0 ? pops.Count - 1 : -1;
				bool mergedAnimated = pushes.Count == 0;
				int pushAnimatedIndex = pushes.Count - 1;

				for (var index = 0; index < pops.Count; index++)
				{
					pops[index].Execute(callbackActionWaiter, popAnimatedIndex == index);
				}

				mergedOp?.Execute(callbackActionWaiter, mergedAnimated);

				for (var index = 0; index < pushes.Count; index++)
				{
					pushes[index].Execute(callbackActionWaiter, pushAnimatedIndex == index);
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
				for (int i = _innerStacks.Count - 1; i >= 0; i--)
				{
					var item = _innerStacks[i];
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

				if (lastInnerStackPopIndex == 0)
				{
					lastInnerStackPopIndex = 1;
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
				for (int i = 1; i < popOperations.Count; i++)
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

			private List<PushOperation> Push(IEnumerable<PushInformation> pushInformations)
			{
				List<PushOperation> pushOperations = new List<PushOperation>();
				InnerStack stackTop = _innerStacks[_innerStacks.Count - 1];
				InnerStack top = stackTop;
				if (stackTop is ModalControllerInnerStack topModal)
				{
					top = topModal.Modal;
				}

				foreach (PushInformation pushInformation in pushInformations)
				{
					if (pushInformation.Controller.IsModal)
					{
						UIViewController host = top.Host;
						if (stackTop is ModalControllerInnerStack)
						{
							host = AsViewController(top);
						}

						ModalControllerInnerStack newTopStack = new ModalControllerInnerStack(host)
						{
							Modal = CreateFromType(pushInformation, top)
						};
						pushOperations.Add(new ModalControllerPushOperation(top, newTopStack));
						_innerStacks.Add(newTopStack);
						top = newTopStack.Modal;
					}
					else if (top is NavigationControllerInnerStack topNavigationController)
					{
						InnerStack newItem = CreateFromType(pushInformation, top);
						pushOperations.Add(new NavigationControllerPushOperation(topNavigationController)
						{
							Controllers =
							{
								newItem
							}
						});
						topNavigationController.Stack.Add(newItem);
					}
					else
					{
						throw new InvalidOperationException("Something went wrong, come with the debugger to find, too many possibilities...");
					}
				}

				//simplify list of pop operations
				int insertIndex = 0;
				for (int i = 1; i < pushOperations.Count; i++)
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

				InnerStack CreateFromType(PushInformation info, InnerStack container)
				{
					UIViewController res = info.Controller.Factory(info.Screen.ViewModelInstance);
					if (res is UINavigationController)
					{
						return new NavigationControllerInnerStack(res, container);
					}

					return new SimpleControllerInnerStack(container.Host, res, container);
				}
			}

			private bool TryMerge(PopOperation op1, PopOperation op2, out PopOperation res)
			{
				if (op1 is NavigationControllerPopOperation nav1 && op2 is NavigationControllerPopOperation nav2)
				{
					if (nav1.HostStack == nav2.HostStack)
					{
						res = new NavigationControllerPopOperation(nav1.HostStack, nav1.CountToPop + nav2.CountToPop);
						return true;
					}
				}

				res = null;
				return false;
			}

			private bool TryMerge(PushOperation op1, PushOperation op2, out PushOperation res)
			{
				if (op1 is NavigationControllerPushOperation nav1 && op2 is NavigationControllerPushOperation nav2)
				{
					if (nav1.HostStack == nav2.HostStack)
					{
						NavigationControllerPushOperation result = new NavigationControllerPushOperation(nav1.HostStack);
						result.Controllers.AddRange(nav1.Controllers);
						result.Controllers.AddRange(nav2.Controllers);
						res = result;
						return true;
					}
				}

				res = null;
				return false;
			}

			private bool TryMerge(PopOperation popOp, PushOperation pushOp, out MergedPopPushOperation res)
			{
				if (popOp is NavigationControllerPopOperation navigationControllerPopOperation &&
				    pushOp is NavigationControllerPushOperation navigationControllerPushOperation)
				{
					if (navigationControllerPopOperation.HostStack == navigationControllerPushOperation.HostStack)
					{
						res = new MergedPopPushNavigationControllerOperation(navigationControllerPopOperation, navigationControllerPushOperation);
						return true;
					}
				}

				res = null;
				return false;
			}

			private static UIViewController AsViewController(InnerStack item)
			{
				if (item is SimpleControllerInnerStack simpleControllerInnerStack)
				{
					return simpleControllerInnerStack.Controller;
				}

				if (item is ModalControllerInnerStack modalControllerInnerStack)
				{
					return AsViewController(modalControllerInnerStack.Modal);
				}

				throw new NotSupportedException($"Unsupported type of {item.GetType().Name}");
			}

			private abstract class InnerStack
			{
				protected InnerStack(UIViewController host)
				{
					Host = host;
				}

				public UIViewController Host { get; }

				public abstract int Count { get; }

				public abstract PopOperation AsPopOperation();

				public abstract PopOperation AsSpecificPopOperation(InnerStack child);
				public abstract PopOperation AsSpecificPopOperation(int count);
			}

			private class SimpleControllerInnerStack : InnerStack
			{
				public UIViewController Controller { get; }

				public InnerStack Container { get; }

				public SimpleControllerInnerStack(UIViewController host, UIViewController controller, InnerStack container) : base(host)
				{
					Controller = controller;
					Container = container;
				}

				public override int Count => 1;

				public override PopOperation AsPopOperation() => Container.AsSpecificPopOperation(this);

				public override PopOperation AsSpecificPopOperation(InnerStack child) => throw new NotSupportedException("This operation is not supported for simple controllers");
				public override PopOperation AsSpecificPopOperation(int count) => throw new NotSupportedException("This operation is not supported for simple controllers");
			}

			private class NavigationControllerInnerStack : InnerStack
			{
				public NavigationControllerInnerStack(UIViewController host, InnerStack container) : base(host)
				{
					Container = container;
				}

				public List<InnerStack> Stack { get; } = new List<InnerStack>();

				public InnerStack Container { get; }

				public override int Count => Stack.Sum(x => x.Count);

				public override PopOperation AsPopOperation()
				{
					if (Container != null)
					{
						Stack.Clear();
						return Container.AsSpecificPopOperation(this);
					}

					var result = new NavigationControllerPopOperation(this, Stack.Count);
					Stack.Clear();
					return result;
				}

				public override PopOperation AsSpecificPopOperation(InnerStack child)
				{
					Stack.RemoveAt(Stack.Count - 1);
					return new NavigationControllerPopOperation(this, 1);
				}

				public override PopOperation AsSpecificPopOperation(int count)
				{
					Stack.RemoveRange(Stack.Count - count, count);
					return new NavigationControllerPopOperation(this, count);
				}
			}

			private class ModalControllerInnerStack : InnerStack
			{
				public ModalControllerInnerStack(UIViewController host) : base(host)
				{
				}

				public InnerStack Modal { get; set; }

				public override int Count => Modal.Count;

				public override PopOperation AsPopOperation()
				{
					return new ModalControllerPopOperation(this);
				}

				public override PopOperation AsSpecificPopOperation(InnerStack child) => Modal.AsSpecificPopOperation(child);
				public override PopOperation AsSpecificPopOperation(int count) => Modal.AsSpecificPopOperation(count);
			}

			private abstract class PopOperation
			{
				public abstract void Execute(CallbackActionWaiter callbackActionWaiter, bool animated);
			}

			private abstract class PopOperation<TInnerStack> : PopOperation
				where TInnerStack : InnerStack
			{
				protected PopOperation(TInnerStack hostStack)
				{
					HostStack = hostStack;
				}

				public TInnerStack HostStack { get; }
			}

			private class ModalControllerPopOperation : PopOperation<ModalControllerInnerStack>
			{
				public ModalControllerPopOperation(ModalControllerInnerStack hostStack) : base(hostStack)
				{
				}

				public override void Execute(CallbackActionWaiter callbackActionWaiter, bool animated)
				{
					if (HostStack.Modal is NavigationControllerInnerStack navigationControllerInnerStack)
					{
						callbackActionWaiter.WaitOne();
						navigationControllerInnerStack.Host.DismissViewController(animated, callbackActionWaiter.ReleaseOne);
						callbackActionWaiter.Add(() => navigationControllerInnerStack.Host.SafeDispose());
					}
					else if (HostStack.Modal is SimpleControllerInnerStack simpleControllerInnerStack)
					{
						callbackActionWaiter.WaitOne();
						simpleControllerInnerStack.Controller.DismissViewController(animated, callbackActionWaiter.ReleaseOne);
						callbackActionWaiter.Add(() => simpleControllerInnerStack.Controller.SafeDispose());
					}
					else
					{
						throw new NotSupportedException($"Unsupported type of {HostStack.Modal.GetType().Name}");
					}
				}
			}

			private class NavigationControllerPopOperation : PopOperation<NavigationControllerInnerStack>
			{
				public int CountToPop { get; }

				public NavigationControllerPopOperation(NavigationControllerInnerStack hostStack, int countToPop) : base(hostStack)
				{
					CountToPop = countToPop;
				}

				public override void Execute(CallbackActionWaiter callbackActionWaiter, bool animated)
				{
					UINavigationController navigationController = (UINavigationController) HostStack.Host;
					List<UIViewController> vcs = navigationController.ViewControllers.ToList();
					if (CountToPop == 1)
					{
						UIViewController poppedViewController = vcs[vcs.Count - 1];
						callbackActionWaiter.Add(() => poppedViewController.SafeDispose());
						navigationController.PopViewController(animated);
					}
					else
					{
						int popToIndex = vcs.Count - 1 - CountToPop;
						navigationController.PopToViewController(vcs[popToIndex], animated);
						callbackActionWaiter.Add(() =>
						{
							for (int i = popToIndex + 1; i < vcs.Count; ++i)
							{
								vcs[i].SafeDispose();
							}
						});
					}
				}
			}

			private abstract class PushOperation
			{
				public abstract void Execute(CallbackActionWaiter callbackActionWaiter, bool animated);
			}

			private abstract class PushOperation<THostStack> : PushOperation
				where THostStack : InnerStack
			{
				public THostStack HostStack { get; }

				protected PushOperation(THostStack hostStack)
				{
					HostStack = hostStack;
				}
			}

			private class NavigationControllerPushOperation : PushOperation<NavigationControllerInnerStack>
			{
				public List<InnerStack> Controllers { get; } = new List<InnerStack>();

				public NavigationControllerPushOperation(NavigationControllerInnerStack hostStack) : base(hostStack)
				{
				}

				public override void Execute(CallbackActionWaiter callbackActionWaiter, bool animated)
				{
					UINavigationController navigationController = (UINavigationController) HostStack.Host;
					if (Controllers.Count == 1)
					{
						navigationController.PushViewController(AsViewController(Controllers[0]), animated);
					}
					else
					{
						UIViewController[] vcs = navigationController.ViewControllers;
						UIViewController[] newVcs = new UIViewController[vcs.Length + Controllers.Count];

						for (int i = 0; i < vcs.Length; i++)
						{
							newVcs[i] = vcs[i];
						}

						for (int i = 0, j = vcs.Length; j < newVcs.Length; ++i, ++j)
						{
							newVcs[j] = AsViewController(Controllers[i]);
						}

						navigationController.SetViewControllers(newVcs, animated);
					}
				}
			}

			private class ModalControllerPushOperation : PushOperation<InnerStack>
			{
				private InnerStack Controller { get; }

				public ModalControllerPushOperation(InnerStack hostStack, InnerStack controller) : base(hostStack)
				{
					Controller = controller;
				}

				public override void Execute(CallbackActionWaiter callbackActionWaiter, bool animated)
				{
					UIViewController host = Controller.Host;

					var vc = AsViewController(Controller);
					callbackActionWaiter.WaitOne();
					host.PresentViewController(vc, animated, callbackActionWaiter.ReleaseOne);
				}
			}

			private abstract class MergedPopPushOperation
			{
				public abstract void Execute(CallbackActionWaiter callbackActionWaiter, bool animated);
			}

			private class MergedPopPushNavigationControllerOperation : MergedPopPushOperation
			{
				public NavigationControllerPopOperation Pop { get; }

				public NavigationControllerPushOperation Push { get; }

				public MergedPopPushNavigationControllerOperation(NavigationControllerPopOperation pop, NavigationControllerPushOperation push)
				{
					Pop = pop;
					Push = push;
				}

				public override void Execute(CallbackActionWaiter callbackActionWaiter, bool animated)
				{
					UINavigationController navigationController = (UINavigationController) Pop.HostStack.Host;
					UIViewController[] vcs = navigationController.ViewControllers;
					UIViewController[] newVcs;
					List<UIViewController> controllersToDispose = new List<UIViewController>(Pop.CountToPop);

					int newCount = vcs.Length - Pop.CountToPop + Push.Controllers.Count;
					if (vcs.Length == newCount)
					{
						newVcs = vcs;

						for (int j = vcs.Length - Pop.CountToPop; j < vcs.Length; ++j)
						{
							controllersToDispose.Add(newVcs[j]);
						}
					}
					else
					{
						newVcs = new UIViewController[vcs.Length - Pop.CountToPop + Push.Controllers.Count];

						int copyCount = vcs.Length - Pop.CountToPop;
						for (int i = 0; i < copyCount; ++i)
						{
							newVcs[i] = vcs[i];
						}

						for (int j = copyCount; j < vcs.Length; ++j)
						{
							controllersToDispose.Add(vcs[j]);
						}
					}

					for (int i = 0, j = vcs.Length - Pop.CountToPop; j < newVcs.Length; ++i, ++j)
					{
						newVcs[j] = AsViewController(Push.Controllers[i]);
					}

					navigationController.SetViewControllers(newVcs, animated);

					callbackActionWaiter.Add(() =>
					{
						foreach (UIViewController controller in controllersToDispose)
						{
							controller.SafeDispose();
						}
					});
				}
			}
		}

		#endregion
	}
}