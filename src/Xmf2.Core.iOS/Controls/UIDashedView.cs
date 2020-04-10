using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIDashedView : UIView
	{
		public UIDashedView()
		{
			var layer = Layer;
			layer.StrokeColor = UIColor.Black.CGColor;
			layer.FillColor = null;
			layer.LineDashPattern = new NSNumber[] { 6, 2 };
			layer.LineWidth = 3f;
		}

		[Export("layerClass")]
		public static Class GetLayerClass() => new Class(typeof(CAShapeLayer));

		public new CAShapeLayer Layer => (CAShapeLayer)base.Layer;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			Layer.Path = UIBezierPath.FromRoundedRect(Bounds, 6f).CGPath;
		}

		public UIDashedView WithDotColor(CGColor color)
		{
			Layer.StrokeColor = color;
			return this;
		}
	}
}
