using System;
using Android.Util;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using MvvmCross.Core.ViewModels;
using Xmf2.Commons.MvxExtends.Interactions;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Views
{
	[Register("xmf2.commons.mvxextends.droidappcompat.views.PopupMenuLauncher")]
	public class PopupMenuLauncher : View
	{
		PopupMenu _menu;
		PopupMenuRequest _currentRequest;

		//* Constructors
		public PopupMenuLauncher(Context context) : base(context) { }
		public PopupMenuLauncher(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public PopupMenuLauncher(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }
		protected PopupMenuLauncher(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		private IDisposable _subscription;
		private IMvxInteraction<PopupMenuRequest> _popupMenuInteraction;
		public IMvxInteraction<PopupMenuRequest> PopupMenuInteraction
		{
			get { return _popupMenuInteraction; }
			set
			{
				if (_subscription != null)
				{
					_subscription.Dispose();
					_subscription = null;
				}
				_popupMenuInteraction = value;
				if (_popupMenuInteraction != null)
				{
					_subscription = _popupMenuInteraction.WeakSubscribe(OpenPopupMenu);
				}
			}
		}

		private void OpenPopupMenu(PopupMenuRequest request)
		{
			this.CleanAll();

			_currentRequest = request;

			_menu = new PopupMenu(this.Context, this);

			int menuId = Menu.First + 1;
			foreach (var item in request.LstPopupItem)
			{
				_menu.Menu.Add(0, menuId, item.Order, item.Title);
				menuId++;
			}

			_menu.MenuItemClick += MenuItemClick;
			_menu.DismissEvent += MenuDismissEvent;

			_menu.Show();
		}

		void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
		{
			if (_currentRequest != null && e.Item != null)
			{
				_currentRequest.Execute(e.Item.Order);
			}

			this.CleanAll();
		}

		void MenuDismissEvent(object sender, PopupMenu.DismissEventArgs e)
		{
			if (_currentRequest != null)
			{
				_currentRequest.ExecuteCancel();
			}
			this.CleanAll();
		}

		private void CleanAll()
		{
			if (_menu != null)
			{
				try
				{
					_menu.MenuItemClick -= MenuItemClick;
					_menu.DismissEvent -= MenuDismissEvent;
				}
				catch { }
				_menu = null;
			}

			if (_currentRequest != null)
			{
				_currentRequest.Clean();
				_currentRequest = null;
			}
		}

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				this.CleanAll();
			}

			base.Dispose(isDisposing);
		}
	}
}