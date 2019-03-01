using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIDashedControl : UIControl
	{
		public UIDashedControl()
		{
			var layer = this.Layer;
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
			this.Layer.Path = UIBezierPath.FromRoundedRect(this.Bounds, 6f).CGPath;
		}

		public UIDashedControl WithDotColor(CGColor color)
		{
			this.Layer.StrokeColor = color;
			return this;
		}
	}
}
