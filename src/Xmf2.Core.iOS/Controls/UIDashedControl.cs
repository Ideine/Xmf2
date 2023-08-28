#if NET7_0_OR_GREATER
using System.Runtime.InteropServices;
using ObjCRuntime;
#else
using NFloat = System.nfloat;
#endif
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
			var layer = Layer;
			layer.StrokeColor = UIColor.Black.CGColor;
			layer.FillColor = null;
			layer.LineDashPattern = new NSNumber[]
			{
				6,
				2
			};
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

		public UIDashedControl WithDotColor(CGColor color)
		{
			Layer.StrokeColor = color;
			return this;
		}

		public UIDashedControl WithLineWidth(NFloat width)
		{
			Layer.LineWidth = width;
			return this;
		}
	}
}