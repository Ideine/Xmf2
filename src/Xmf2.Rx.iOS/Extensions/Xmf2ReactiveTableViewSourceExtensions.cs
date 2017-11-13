using Foundation;
using System;
using UIKit;

namespace ReactiveUI
{
	public static class Xmf2ReactiveTableViewSourceExtensions
	{
		public static void WithDataList<TSource, TCell>(
			this ReactiveTableViewSource<TSource> tableViewSource,
			UITableView tableView,
			ReactiveList<TSource> dataSource,
			NSString cellKey,
			Action<TCell> initializeCellAction)
			where TCell : UITableViewCell
			where TSource : class
		{
			tableViewSource.Data = new[]{
				new TableSectionInformation<TSource, TCell>(dataSource ?? new ReactiveList<TSource>(), cellKey, (float)tableView.RowHeight, initializeCellAction)
			};
		}

		public static void WithDataList<TSource, TCell>(
			this ReactiveCollectionViewSource<TSource> collectionViewSource,
			ReactiveList<TSource> dataSource,
			NSString cellKey,
			Action<TCell> initalizeCellAction)
			where TSource : class
			where TCell : UICollectionViewCell
		{
			collectionViewSource.Data = new[]
			{
				new CollectionViewSectionInformation<TSource, TCell>(dataSource, cellKey, initalizeCellAction),
			};
		}
	}
}
