using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Xmf2.Core.iOS.Extensions
{
	public static class ImageViewExtensions
	{
		public static (int width, int height) GetImageSize(this UIView view)
		{
			IEnumerable<NSLayoutConstraint> activeConstraints = view.Constraints.Where(x => x.Active && !x.Description.Contains("NSContentSizeLayoutConstraint"));
			int height = (int?)activeConstraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height && x.Relation is NSLayoutRelation.Equal or NSLayoutRelation.LessThanOrEqual)?.Constant ?? 0;
			int width = (int?)activeConstraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Width && x.Relation is NSLayoutRelation.Equal or NSLayoutRelation.LessThanOrEqual)?.Constant ?? 0;

			return (width, height);
		}

		public static string WithAspect(this string url, UIViewContentMode contentMode)
		{
			string parameterChar = url.Contains('?') ? "&" : "?";
			switch (contentMode)
			{
				case UIViewContentMode.ScaleToFill:
				case UIViewContentMode.ScaleAspectFill:
					return url + $"{parameterChar}aspect=Fill";
				case UIViewContentMode.ScaleAspectFit:
					return url + $"{parameterChar}aspect=Fit";
				default:
					return url + $"{parameterChar}aspect=Undefined";
			}
		}
	}
}