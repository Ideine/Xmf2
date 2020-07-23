using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Base;
using MvvmCross.Platforms.Android.Binding.Target;
using MvvmCross.ViewModels;
using Xmf2.Commons.MvxExtends.Interactions;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Target
{
	public class PopupMenuTargetBinding : MvxAndroidTargetBinding<View, IMvxInteraction<PopupMenuRequest>>
	{
		private IDisposable _subscription;

		private PopupMenu _menu;
		private PopupMenuRequest _currentRequest;

		public PopupMenuTargetBinding(View view) : base(view) { }

		protected override void SetValueImpl(View target, IMvxInteraction<PopupMenuRequest> value)
		{
			if (_subscription != null)
			{
				_subscription.Dispose();
				_subscription = null;
			}

			if (value != null)
			{
				_subscription = value.WeakSubscribe(OpenPopupMenu);
			}
		}

		private void OpenPopupMenu(object sender, MvxValueEventArgs<PopupMenuRequest> request)
		{
			CleanAll();

			_currentRequest = request.Value;

			View view = Target;
			if (view != null)
			{
				_menu = new PopupMenu(view.Context, view);

				int menuId = Menu.First + 1;
				foreach (PopupMenuRequest.Item item in _currentRequest.LstPopupItem)
				{
					_menu.Menu.Add(0, menuId, item.Order, item.Title);
					menuId++;
				}

				_menu.MenuItemClick += MenuItemClick;
				_menu.DismissEvent += MenuDismissEvent;

				_menu.Show();
			}
		}

		private void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
		{
			if (_currentRequest != null && e.Item != null)
			{
				_currentRequest.Execute(e.Item.Order);
			}

			CleanAll();
		}

		private void MenuDismissEvent(object sender, PopupMenu.DismissEventArgs e)
		{
			_currentRequest?.ExecuteCancel();
			CleanAll();
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
				CleanAll();

				if (_subscription != null)
				{
					_subscription.Dispose();
					_subscription = null;
				}
			}

			base.Dispose(isDisposing);
		}
	}
}