using System;
using System.ComponentModel;
using System.Reactive;
using Android.Runtime;
using AndroidX.Fragment.App;
using ReactiveUI;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.BaseView
{
	/// <summary>
	/// This is a DialogFragment that is both a DialogFragment and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class XMFReactiveDialogFragment<TViewModel> : XMFReactiveDialogFragment, IViewFor<TViewModel>
		where TViewModel : class
	{
		protected XMFReactiveDialogFragment()
		{
		}

		protected XMFReactiveDialogFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}

		TViewModel _ViewModel;

		public TViewModel ViewModel
		{
			get => _ViewModel;
			set => this.RaiseAndSetIfChanged(ref _ViewModel, value);
		}

		object IViewFor.ViewModel
		{
			get => _ViewModel;
			set => _ViewModel = (TViewModel)value;
		}
	}

	/// <summary>
	/// This is a Fragment that is both an Activity and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class XMFReactiveDialogFragment : DialogFragment,
		IReactiveNotifyPropertyChanged<XMFReactiveDialogFragment>, IReactiveObject, IHandleObservableErrors, ICanActivate
	{
		protected XMFReactiveDialogFragment()
		{
		}

		protected XMFReactiveDialogFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
		{
		}

        public event PropertyChangingEventHandler PropertyChanging;

        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => PropertyChanging?.Invoke(this, args);

        public event PropertyChangedEventHandler PropertyChanged;

        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed.         
        /// </summary>
        public IObservable<IReactivePropertyChangedEventArgs<XMFReactiveDialogFragment>> Changing =>
			this.getChangingObservable();

		/// <summary>
		/// Represents an Observable that fires *after* a property has changed.
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<XMFReactiveDialogFragment>> Changed =>
			this.getChangedObservable();

		/// <summary>
		/// When this method is called, an object will not fire change
		/// notifications (neither traditional nor Observable notifications)
		/// until the return value is disposed.
		/// </summary>
		/// <returns>An object that, when disposed, reenables change
		/// notifications.</returns>
		public IDisposable SuppressChangeNotifications()
		{
			return this.suppressChangeNotifications();
		}

		public IObservable<Exception> ThrownExceptions => this.getThrownExceptionsObservable();

		private readonly CanActivateImplementation _activationImplementation = new CanActivateImplementation();

		public IObservable<Unit> Activated => _activationImplementation.Activated;

		public IObservable<Unit> Deactivated => _activationImplementation.Deactivated;

		public override void OnPause()
		{
			base.OnPause();
			_activationImplementation.Deactivate();
		}

		public override void OnResume()
		{
			base.OnResume();
			_activationImplementation.Activate();
		}

		public override void OnDetach()
		{
			base.OnDetach();
			Dispose();
		}
	}
}