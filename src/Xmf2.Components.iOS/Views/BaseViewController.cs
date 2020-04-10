using System;
using System.Collections.Generic;
using System.Linq;
using CoreFoundation;
using Foundation;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Core.iOS.Helpers;
using Xmf2.Core.Subscriptions;
using Xmf2.iOS.Extensions.Constraints;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Components.iOS.Views
{
	public interface IViewFor
	{
		Type ViewModelType { get; }
	}

	public abstract class BaseViewController<TComponentViewModel, TComponentView> : BaseViewController, IViewFor
		where TComponentViewModel : class, IComponentViewModel
		where TComponentView : class, IComponentView
	{
		private bool _layoutDone;
		private TComponentViewModel _componentViewModel;
		private EventSubscriber _stateChangedSubscriber;

		protected TComponentView Component { get; private set; }

		protected IServiceLocator Services => _componentViewModel.Services;

		public Type ViewModelType => _componentViewModel.GetType();

		public BaseViewController(TComponentViewModel viewModel, Func<IServiceLocator, TComponentView> factory)
		{
			_componentViewModel = viewModel;
			Component = factory(Services).DisposeComponentWith(Disposables);

			_stateChangedSubscriber = new EventSubscriber(
				() => ApplicationState.StateChanged += UpdateState,
				() => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(Disposables);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			_componentViewModel.Lifecycle.Start();

			if (NavigationController != null)
			{
				NavigationController.NavigationBarHidden = true;
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			_componentViewModel.Lifecycle.Resume();

			_stateChangedSubscriber.Subscribe();
			if (!_layoutDone)
			{
				_layoutDone = true;
				View.WithSubviews(Component.View)
					.CenterAndFillHeight(Component.View)
					.CenterAndFillWidth(Component.View);
			}

			UpdateState();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			_componentViewModel.Lifecycle.Pause();

			_stateChangedSubscriber.Unsubscribe();
		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();
			_componentViewModel.Lifecycle.Stop();
		}

		private void UpdateState()
		{
			IViewState state = _componentViewModel.ViewState();
			DispatchQueue.MainQueue.DispatchAsync(() => Component.SetState(state));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Component = null;
				_componentViewModel = null;
				_stateChangedSubscriber = null;
			}
			base.Dispose(disposing);
		}
	}

	public abstract class BaseViewController : UIViewController
	{
		protected Xmf2Disposable Disposables;

		protected BaseViewController()
		{
			Disposables = new Xmf2Disposable();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.ForceToSupportedOrientation();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.WithBackgroundColor(UIColor.White);

			new KeyboardScrollHelper(this).DisposeBindingWith(Disposables);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposables?.Dispose();
				Disposables = null;
			}
			base.Dispose(disposing);
		}


		/// <summary>
		/// Only used if RootNavigationController is type of <see cref="HandleFreeRotateNavigationController"/>
		/// </summary>
		#region Handle Orientation

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation() => UIInterfaceOrientation.Portrait;

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations() => UIInterfaceOrientationMask.Portrait;

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
		}

		public override bool ShouldAutorotate() => true;

		private void ForceToSupportedOrientation()
		{
			var currentOrientation = UIDevice.CurrentDevice.Orientation;
			var supportedOrientations = GetDeviceOrientationFromMasks(GetSupportedInterfaceOrientations()).ToArray();
			var isCurrentOrientationSupported = (supportedOrientations.Any(o => o == currentOrientation));
			if (!isCurrentOrientationSupported)
			{
				var preferedOrientation = this.PreferredInterfaceOrientationForPresentation();
				UIDevice.CurrentDevice.SetValueForKey(new NSNumber((ulong)preferedOrientation), new Foundation.NSString("orientation"));
			}
		}

		private static IEnumerable<UIDeviceOrientation> GetDeviceOrientationFromMasks(UIInterfaceOrientationMask orientationMask)
		{
			var flagList = new[]
			{
				UIInterfaceOrientationMask.Portrait, //2
				UIInterfaceOrientationMask.PortraitUpsideDown, //4
				UIInterfaceOrientationMask.LandscapeRight, //8
				UIInterfaceOrientationMask.LandscapeLeft, //16
			};
			foreach (var availableFlag in flagList)
			{
				var actualFlag = (orientationMask & availableFlag);
				if (actualFlag > 0)
				{
					switch (actualFlag)
					{
						case UIInterfaceOrientationMask.Portrait: yield return UIDeviceOrientation.Portrait; break;
						case UIInterfaceOrientationMask.PortraitUpsideDown: yield return UIDeviceOrientation.PortraitUpsideDown; break;
						case UIInterfaceOrientationMask.LandscapeRight: yield return UIDeviceOrientation.LandscapeRight; break;
						case UIInterfaceOrientationMask.LandscapeLeft: yield return UIDeviceOrientation.LandscapeLeft; break;
						default:
							continue;
					}
				}
			}
		}

		#endregion
	}
}