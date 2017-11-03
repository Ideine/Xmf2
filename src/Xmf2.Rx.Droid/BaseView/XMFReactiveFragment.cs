﻿using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Runtime;
using ReactiveUI;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.BaseView
{
	//Class, method and from ReactiveUI : https://github.com/reactiveui/ReactiveUI/blob/7.4.0/src/ReactiveUI/IReactiveObject.cs

	/// <summary>
	/// This is a Fragment that is both an Activity and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class XMFFragment<TViewModel> : XMFFragment, IViewFor<TViewModel> where TViewModel : class
	{
		protected XMFFragment() { }

		protected XMFFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		TViewModel _viewModel;

		public TViewModel ViewModel
		{
			get { return _viewModel; }
			set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
		}

		object IViewFor.ViewModel
		{
			get { return _viewModel; }
			set { _viewModel = (TViewModel)value; }
		}
	}

	/// <summary>
	/// This is a Fragment that is both an Activity and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class XMFFragment : global::Android.Support.V4.App.Fragment, IReactiveNotifyPropertyChanged<XMFFragment>, IReactiveObject, IHandleObservableErrors, ICanActivate
	{
		protected XMFFragment() { }

		protected XMFFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		public event ReactiveUI.PropertyChangingEventHandler PropertyChanging
		{
			add { WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.AddHandler(this, value); }
			remove { WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.RemoveHandler(this, value); }
		}

		void IReactiveObject.RaisePropertyChanging(ReactiveUI.PropertyChangingEventArgs args)
		{
			WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.DeliverEvent(this, args);
		}

		public event PropertyChangedEventHandler PropertyChanged
		{
			add { WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.AddHandler(this, value); }
			remove { WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.RemoveHandler(this, value); }
		}

		void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.DeliverEvent(this, args);
		}

		/// <summary>
		/// Represents an Observable that fires *before* a property is about to
		/// be changed.         
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<XMFFragment>> Changing
		{
			get { return this.getChangingObservable(); }
		}

		/// <summary>
		/// Represents an Observable that fires *after* a property has changed.
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<XMFFragment>> Changed
		{
			get { return this.getChangedObservable(); }
		}

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

		public IObservable<Exception> ThrownExceptions { get { return this.getThrownExceptionsObservable(); } }

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
	}
}