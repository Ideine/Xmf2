namespace UIKit
{
	public static class UITableViewExtensions
	{
		public static void RegisterClassForCellReuse<TCell>(this UITableView tableView) where TCell : UITableViewCell
		{
			var cellType = typeof(TCell);
			tableView.RegisterClassForCellReuse(cellType, cellType.FullName);
		}
		public static void RegisterClassForHeaderFooterViewReuse<TCell>(this UITableView tableView) where TCell : UITableViewHeaderFooterView
		{
			var cellType = typeof(TCell);
			tableView.RegisterClassForHeaderFooterViewReuse(cellType, cellType.FullName);
		}

		public static TCell DequeueReusableCell<TCell>(this UITableView tableView) where TCell : UITableViewCell
		{
			var cellType = typeof(TCell);
			return tableView.DequeueReusableCell(cellType.FullName) as TCell;
		}

		public static TCell DequeueReusableHeaderFooterView<TCell>(this UITableView tableView) where TCell : UITableViewHeaderFooterView
		{
			var cellType = typeof(TCell);
			return tableView.DequeueReusableHeaderFooterView(cellType.FullName) as TCell;
		}
	}
}