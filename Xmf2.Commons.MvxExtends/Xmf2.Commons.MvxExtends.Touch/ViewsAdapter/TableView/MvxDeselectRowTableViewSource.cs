using System;
using UIKit;
using Foundation;

namespace Xmf2.Commons.MvxExtends.Touch.ViewsAdapter.TableView
{
	public class MvxDeselectRowTableViewSource :  Cirrious.MvvmCross.Binding.Touch.Views.MvxSimpleTableViewSource
	{

		private readonly int _tableRowHeight;

		public MvxDeselectRowTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null,int tableRowHeight = 50) : base (tableView, nibName, cellIdentifier, bundle)
		{
			_tableRowHeight = tableRowHeight;
		}

		public MvxDeselectRowTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null,int tableRowHeight = 50): base(tableView, cellType, cellIdentifier)
		{
			_tableRowHeight = tableRowHeight;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			base.RowSelected(tableView, indexPath);
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return _tableRowHeight;
		}

		public override UIView GetViewForFooter (UITableView tableView, nint section)
		{
			return new UIView();
		}
	}
}

