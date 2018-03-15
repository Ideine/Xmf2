using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using ReactiveUI;
using ReactiveUI.Android.Support;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.ListElement
{
	public class BaseReactiveRecyclerViewViewHolder<TViewModel> : XMF2ReactiveRecyclerViewViewHolder<TViewModel>, ICanActivate, IRecyclerViewViewHolder 
		where TViewModel : class, IReactiveObject
	{
		protected Context Context { get; private set; }

		private readonly CanActivateImplementation _activationImplementation = new CanActivateImplementation();

		public IObservable<Unit> Activated => _activationImplementation.Activated;

		public IObservable<Unit> Deactivated => _activationImplementation.Deactivated;

		public ICommand ItemClick { get; set; }

		public ICommand ItemLongClick { get; set; }


		public BaseReactiveRecyclerViewViewHolder(View view) : base(view)
		{
			Context = view.Context;
			OnContentViewSet();
			SetViewModelBindings();
			ItemView.Click += OnClickItem;
			ItemView.LongClick += OnLongClickItem;
		}

		protected BaseReactiveRecyclerViewViewHolder(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer)
		{

		}

		protected virtual void OnContentViewSet() { }

		protected virtual void SetViewModelBindings() { }

		void OnClickItem(object sender, EventArgs e)
		{
			ItemClick?.TryExecute(GetItemClickParameter());
		}

		protected virtual object GetItemClickParameter() => ViewModel;

		void OnLongClickItem(object sender, EventArgs e)
		{
			if (ItemLongClick != null)
			{
				ItemLongClick?.TryExecute(GetItemLongClickParameter());
			}
			else
			{
				ItemClick?.TryExecute(GetItemClickParameter());
			}
		}

		protected virtual object GetItemLongClickParameter()
		{
			return ViewModel;
		}

		#region Lifecycle

		public virtual void OnViewAttachedToWindow()
		{
			_activationImplementation.Activate();
		}

		public virtual void OnViewDetachedFromWindow()
		{
			_activationImplementation.Deactivate();
		}

		public virtual void OnViewRecycled() { }

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ItemView != null)
				{
					ItemView.Click -= OnClickItem;
					ItemView.LongClick -= OnLongClickItem;
				}

				ItemClick = null;
				ItemLongClick = null;
				Context = null;
			}
			base.Dispose(disposing);
		}
	}
}
