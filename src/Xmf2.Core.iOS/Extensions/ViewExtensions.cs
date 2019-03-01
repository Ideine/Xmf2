using System;
using UIKit;

namespace Xmf2.Core.iOS.Extensions
{
	public static class ViewExtensions
	{
		public static T GetFirstDescendantOfType<T>(this UIView root) where T : UIView
		{
			for (var i = 0; i < root.Subviews.Length; i++)
			{
				var view = root.Subviews[i];
				if (view is T)
				{
					return (T)view;
				}

				var descendant = GetFirstDescendantOfType<T>(view);
				if (descendant != null)
				{
					return descendant;
				}
			}
			return null;
		}
	}
}
