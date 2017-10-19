using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.App;
using ReactiveUI;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.BaseView
{
	//Class, method and from ReactiveUI : https://github.com/reactiveui/ReactiveUI/blob/7.4.0/src/ReactiveUI/IReactiveObject.cs

	/// <summary>
	/// This is an Activity that is both an Activity and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class XMFAppCompatActivity<TViewModel> : ReactiveAppCompatActivity, IViewFor<TViewModel>, ICanActivate
		where TViewModel : class
	{
		protected XMFAppCompatActivity() { }

		protected XMFAppCompatActivity(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

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
	/// This is an Activity that is both an Activity and has ReactiveObject powers 
	/// (i.e. you can call RaiseAndSetIfChanged)
	/// </summary>
	public class ReactiveAppCompatActivity : AppCompatActivity, IReactiveObject, IReactiveNotifyPropertyChanged<ReactiveAppCompatActivity>, IHandleObservableErrors
	{
		protected ReactiveAppCompatActivity() { }

		protected ReactiveAppCompatActivity(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		public event ReactiveUI.PropertyChangingEventHandler PropertyChanging
		{
			add => WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.AddHandler(this, value);
			remove => WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.RemoveHandler(this, value);
		}

		void IReactiveObject.RaisePropertyChanging(ReactiveUI.PropertyChangingEventArgs args)
		{
			WeakEventManager<ReactiveUI.INotifyPropertyChanging, ReactiveUI.PropertyChangingEventHandler, ReactiveUI.PropertyChangingEventArgs>.DeliverEvent(this, args);
		}

		public event PropertyChangedEventHandler PropertyChanged
		{
			add => WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.AddHandler(this, value);
			remove => WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.RemoveHandler(this, value);
		}

		void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			WeakEventManager<INotifyPropertyChanged, PropertyChangedEventHandler, PropertyChangedEventArgs>.DeliverEvent(this, args);
		}

		/// <summary>
		/// Represents an Observable that fires *before* a property is about to
		/// be changed.         
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<ReactiveAppCompatActivity>> Changing => this.getChangingObservable();

		/// <summary>
		/// Represents an Observable that fires *after* a property has changed.
		/// </summary>
		public IObservable<IReactivePropertyChangedEventArgs<ReactiveAppCompatActivity>> Changed => this.getChangedObservable();

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

		readonly Subject<Unit> activated = new Subject<Unit>();
		public IObservable<Unit> Activated => activated.AsObservable();

		readonly Subject<Unit> deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated => deactivated.AsObservable();

		protected override void OnPause()
		{
			base.OnPause();
			deactivated.OnNext(Unit.Default);
		}

		protected override void OnResume()
		{
			base.OnResume();
			activated.OnNext(Unit.Default);
		}

		readonly Subject<Tuple<int, Result, Intent>> activityResult = new Subject<Tuple<int, Result, Intent>>();
		public IObservable<Tuple<int, Result, Intent>> ActivityResult => activityResult.AsObservable();

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			activityResult.OnNext(Tuple.Create(requestCode, resultCode, data));
		}

		public Task<Tuple<Result, Intent>> StartActivityForResultAsync(Intent intent, int requestCode)
		{
			// NB: It's important that we set up the subscription *before* we
			// call ActivityForResult
			var ret = ActivityResult
				.Where(x => x.Item1 == requestCode)
				.Select(x => Tuple.Create(x.Item2, x.Item3))
				.FirstAsync()
				.ToTask();

			StartActivityForResult(intent, requestCode);
			return ret;
		}

		public Task<Tuple<Result, Intent>> StartActivityForResultAsync(Type type, int requestCode)
		{
			// NB: It's important that we set up the subscription *before* we
			// call ActivityForResult
			var ret = ActivityResult
				.Where(x => x.Item1 == requestCode)
				.Select(x => Tuple.Create(x.Item2, x.Item3))
				.FirstAsync()
				.ToTask();

			StartActivityForResult(type, requestCode);
			return ret;
		}
	}
}
