using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platform.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.Target;
using Xmf2.Commons.MvxExtends.Interactions;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Target
{
	public class PopupMenuTargetBinding : MvxAndroidTargetBinding
	{
		private IDisposable _subscription;

		private PopupMenu _menu;
		private PopupMenuRequest _currentRequest;

		public override Type TargetType => typeof(IMvxInteraction<PopupMenuRequest>);

		public PopupMenuTargetBinding(View view) : base(view) { }

		protected override void SetValueImpl(object target, object value)
		{
			if (_subscription != null)
			{
				_subscription.Dispose();
				_subscription = null;
			}

			if (value == null)
			{
				return;
			}

			var view = (View)target;
			var interaction = value as IMvxInteraction<PopupMenuRequest>;
			if (interaction == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value '{0}' could not be parsed as a valid IMvxInteraction<PopupMenuRequest>", value);
			}
			else
			{
				_subscription = interaction.WeakSubscribe(OpenPopupMenu);
			}
		}

		private void OpenPopupMenu(object sender, MvxValueEventArgs<PopupMenuRequest> request)
		{
			this.CleanAll();

			_currentRequest = request.Value;

			var view = base.Target as View;
			if (view == null)
			{
				return;
			}

			_menu = new PopupMenu(view.Context, view);

			int menuId = Menu.First + 1;
			foreach (var item in _currentRequest.LstPopupItem)
			{
				_menu.Menu.Add(0, menuId, item.Order, item.Title);
				menuId++;
			}

			_menu.MenuItemClick += MenuItemClick;
			_menu.DismissEvent += MenuDismissEvent;

			_menu.Show();
		}

		private void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
		{
			if (_currentRequest != null && e.Item != null)
			{
				_currentRequest.Execute(e.Item.Order);
			}

			this.CleanAll();
		}

		private void MenuDismissEvent(object sender, PopupMenu.DismissEventArgs e)
		{
			_currentRequest?.ExecuteCancel();
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