using Foundation;
using UIKit;

namespace Xmf2.Core.iOS.Extensions
{
	public static class UISearchBarExtensions
	{
		private static readonly NSString _searchField = new NSString("searchField");
		private static readonly NSString _searchCancelButton = new NSString("cancelButton");

		/// <summary>
		/// Returns <see cref="UISearchBar"/>'s <see cref="UITextField"/>.
		/// NOT SUPPORTED BY APPLE therefore may return null in the future.
		/// </summary>
		public static UITextField TextField(this UISearchBar searchBar)
		{
			try
			{
				return searchBar?.ValueForKey(_searchField) as UITextField;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Returns <see cref="UISearchBar"/>'s cancel button.
		/// NOT SUPPORTED BY APPLE therefore may return null in the future.
		/// </summary>
		public static UIButton CancelButton(this UISearchBar searchBar)
		{
			try
			{
				return searchBar?.ValueForKey(_searchCancelButton) as UIButton;
			}
			catch
			{
				return null;
			}
		}
	}
}