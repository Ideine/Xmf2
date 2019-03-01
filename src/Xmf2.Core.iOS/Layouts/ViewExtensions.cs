using System.Linq;
using UIKit;

namespace Xmf2.Core.iOS.Layouts
{
	public static class ViewExtensions
	{
		public static UIScrollView FindScrollView(this UIView view)
		{
			if (view is UIScrollView result)
			{
				return result;
			}

			return view.Subviews.Select(x => x.FindScrollView()).FirstOrDefault(x => x != null);
		}
	}
}