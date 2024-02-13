using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIDashedView : UIView
	{
		private readonly float _cornerRadius;

		public UIDashedView(UIColor dashColor, float dashWidth = 6f, float dashGap = 2f, float lineWidth = 3f, float cornerRadius = 6f)
		{
			_cornerRadius = cornerRadius;

			var layer = Layer;
			layer.StrokeColor = dashColor.CGColor;
			layer.FillColor = null;
			layer.LineDashPattern = new NSNumber[] { dashWidth, dashGap };
			layer.LineWidth = lineWidth;
		}

		[Export("layerClass")]
		public static Class GetLayerClass() => new Class(typeof(CAShapeLayer));

		public new CAShapeLayer Layer => (CAShapeLayer)base.Layer;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			Layer.Path = UIBezierPath.FromRoundedRect(Bounds, _cornerRadius).CGPath;
		}

		public UIDashedView WithDotColor(CGColor color)
		{
			Layer.StrokeColor = color;
			return this;
		}
	}
}
