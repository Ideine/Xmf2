using System;
using UIKit;
using Foundation;
using MvvmCross.Binding.iOS.Views;

namespace Xmf2.Commons.MvxExtends.Touch.ViewsAdapter.TableView
{
	public class MvxDeselectRowTableViewSource :  MvxSimpleTableViewSource
	{


		public MvxDeselectRowTableViewSource (UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null) : base (tableView, nibName, cellIdentifier, bundle)
		{
		}

		public MvxDeselectRowTableViewSource (UITableView tableView, Type cellType, string cellIdentifier = null) : base (tableView, cellType, cellIdentifier)
		{
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow (indexPath, true);
			base.RowSelected (tableView, indexPath);
		}
	}
}

