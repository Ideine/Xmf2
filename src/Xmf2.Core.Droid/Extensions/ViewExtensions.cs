using Android.Views;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ViewExtensions
	{
		public static T GetFirstDescendantOfType<T>(this ViewGroup root) where T : View
		{
			for (int i = 0 ; i < root.ChildCount ; i++)
			{
				View view = root.GetChildAt(i);
				if (view is T typedView)
				{
					return typedView;
				}
				else if (view is ViewGroup rootChild)
				{
					T descendant = GetFirstDescendantOfType<T>(rootChild);
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