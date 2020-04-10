using System.Runtime.CompilerServices;
using CoreGraphics;
using UIKit;
using Xmf2.Core.iOS.Layouts;

namespace Xmf2.Core.iOS.Extensions
{
	public static class LayoutExtensions
	{
		public static NSLayoutConstraint WithAutomaticIdentifier(this NSLayoutConstraint constraint, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
		{
			constraint.SetIdentifier($"{memberName}_line:{sourceLineNumber}");
			return constraint;
		}

		public static NSLayoutConstraint WithIdentifier(this NSLayoutConstraint constraint, string identifier)
		{
			constraint.SetIdentifier(identifier);
			return constraint;
		}

		public static TView ScaleTo<TView>(this TView view, float ratio, HorizontalAnchor horizontalAnchor = HorizontalAnchor.Center, VerticalAnchor verticalAnchor = VerticalAnchor.Center) where TView : UIView
		{
			view.Transform = GetScaleTransform(view, ratio, horizontalAnchor, verticalAnchor);
			return view;
		}
		public static CGAffineTransform GetScaleTransform(this UIView view, float ratio, HorizontalAnchor horizontalAnchor = HorizontalAnchor.Center, VerticalAnchor verticalAnchor = VerticalAnchor.Center)
		{
			var transform = CGAffineTransform.MakeScale(ratio, ratio);
			switch (horizontalAnchor)
			{
				case HorizontalAnchor.Left:
					transform = transform * CGAffineTransform.MakeTranslation(-view.Frame.Width * (1 - ratio) / 2f, 0f);
					break;
				case HorizontalAnchor.Right:
					transform = transform * CGAffineTransform.MakeTranslation(view.Frame.Width * (1 - ratio) / 2f, 0f);
					break;
				case HorizontalAnchor.Center:
				default:
					break;
			}
			switch (verticalAnchor)
			{
				case VerticalAnchor.Top:
					transform = transform * CGAffineTransform.MakeTranslation(0f, -view.Frame.Width * (1 - ratio));
					break;
				case VerticalAnchor.Bottom:
					transform = transform * CGAffineTransform.MakeTranslation(0f, view.Frame.Width * (1 - ratio));
					break;
				case VerticalAnchor.Center:
				default:
					break;
			}
			return transform;
		}
	}
}
