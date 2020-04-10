using Android.Views;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ViewExtensions
	{
		public static T GetFirstDescendantOfType<T>(this ViewGroup root) where T : View
		{
			for (var i = 0; i < root.ChildCount; i++)
			{
				var view = root.GetChildAt(i);
				if (view is T)
				{
					return (T)view;
				}
				if (view is ViewGroup rootChild)
				{
					var descendant = GetFirstDescendantOfType<T>(rootChild);
					if (descendant != null)
					{
						return descendant;
					}
				}
			}
			return null;
		}
	}
}
