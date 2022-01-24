using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ReactiveUI;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.ListElement
{
	public class XMF2ReactiveRecyclerViewViewHolder<TViewModel> : RecyclerView.ViewHolder, ILayoutViewHost, IViewFor<TViewModel>, IReactiveNotifyPropertyChanged<XMF2ReactiveRecyclerViewViewHolder<TViewModel>>, IReactiveObject
		where TViewModel : class, IReactiveObject
	{
		protected XMF2ReactiveRecyclerViewViewHolder(View view)
			: base(view)
		{
			setupRxObj();

			this.Selected = Observable.FromEventPattern(h => view.Click += h, h => view.Click -= h).Select(_ => this.AdapterPosition);
		}

		protected XMF2ReactiveRecyclerViewViewHolder(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer)
		{
		}

		/// <summary>
		/// Signals that this ViewHolder has been selected. 
		/// 
		/// The <see cref="int"/> is the position of this ViewHolder in the <see cref="RecyclerView"/> 
		/// and corresponds to the <see cref="RecyclerView.ViewHolder.AdapterPosition"/> property.
		/// </summary>
		public IObservable<int> Selected { get; private set; }

		public View View
		{
			get { return this.ItemView; }
		}

		TViewModel _ViewModel;
		public TViewModel ViewModel
		{
			get { return _ViewModel; }
			set { this.RaiseAndSetIfChanged(ref _ViewModel, value); }
		}

		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = (TViewModel)value; }
		}

		/// <inheritdoc/>
		public event PropertyChangingEventHandler? PropertyChanging;

		/// <inheritdoc/>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <inheritdoc/>
		void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args) => PropertyChanging?.Invoke(this, args);

		/// <inheritdoc/>
		void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args) => PropertyChanged?.Invoke(this, args);

		/// <summary>
		/// Represents an Observable that fires *before* a property is about to
		/// be changed.         
		/// </summary>
		//[IgnoreDateMember]
		public IObservable<IReactivePropertyChangedEventArgs<XMF2ReactiveRecyclerViewViewHolder<TViewModel>>> Changing
		{
			get => this.getChangingObservable();
		}

		/// <summary>
		/// Represents an Observable that fires *after* a property has changed.
		/// </summary>
		//[IgnoreDataMember]
		public IObservable<IReactivePropertyChangedEventArgs<XMF2ReactiveRecyclerViewViewHolder<TViewModel>>> Changed
		{
			get => this.getChangedObservable();
		}

		//[IgnoreDataMember]
		protected Lazy<PropertyInfo[]> allPublicProperties;

		//[IgnoreDataMember]
		public IObservable<Exception> ThrownExceptions { get { return this.getThrownExceptionsObservable(); } }

		[OnDeserialized]
		void setupRxObj(StreamingContext sc) { setupRxObj(); }

		void setupRxObj()
		{
			allPublicProperties = new Lazy<PropertyInfo[]>(() =>
				GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToArray());
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

		public bool AreChangeNotificationsEnabled()
		{
			return this.areChangeNotificationsEnabled();
		}
	}
}
