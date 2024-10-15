using Foundation;
using System;
using System.Collections.ObjectModel;
using UIKit;

namespace ReactiveUI
{
	public static class Xmf2ReactiveTableViewSourceExtensions
	{
		public static void WithDataList<TSource, TCell>(
			this ReactiveTableViewSource<TSource> tableViewSource,
			UITableView tableView,
            ObservableCollection<TSource> dataSource,
			NSString cellKey,
			Action<TCell> initializeCellAction)
			where TCell : UITableViewCell
			where TSource : class
		{
			tableViewSource.Data = new[]{
				new TableSectionInformation<TSource, TCell>(dataSource ?? new ObservableCollection<TSource>(), cellKey, (float)tableView.RowHeight, initializeCellAction)
			};
		}
    }
}
