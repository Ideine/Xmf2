using System;
using Foundation;
using System.Linq;

namespace UIKit
{
	public static class ViewExtensions
	{
		private static readonly NSString _searchField = new NSString("_searchField");

		public static UITextField TextField(this UISearchBar searchBar)
		{
			var textField = searchBar.ValueForKey(_searchField) as UITextField;
			return textField;
		}
		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}

			return view.Subviews.Select(subView => subView.FindFirstResponder()).FirstOrDefault(firstResponder => firstResponder != null);
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsInstanceOfType(view.Superview))
				{
					return view.Superview;
				}

				if (!Equals(view.Superview, stopAt))
				{
					return view.Superview.FindSuperviewOfType(stopAt, type);
				}
			}
			return null;
		}

		public static UIView FindTopSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			var superview = view.FindSuperviewOfType(stopAt, type);
			var topSuperView = superview;
			while (superview != null && !Equals(superview, stopAt))
			{
				superview = superview.FindSuperviewOfType(stopAt, type);
				if (superview != null)
				{
					topSuperView = superview;
				}
			}
			return topSuperView;
		}

		public static UIView RemoveSubviews(this UIView view)
		{
			foreach (var subView in view.Subviews)
			{
				subView.RemoveFromSuperview();
			}
			return view;
		}

		public static bool IsLandscape()
		{
			var orientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool landscape = orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight;
			return landscape;
		}
	}
}
