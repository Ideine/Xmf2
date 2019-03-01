using System;
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Controls.Layers
{
	public class OvalLayer : CoreAnimation.CALayer
	{
		private readonly CoreAnimation.CAShapeLayer _shape;

		public OvalLayer()
		{
			_shape = new CoreAnimation.CAShapeLayer();
			base.Mask = _shape;
		}

		public override CGRect Bounds
		{
			get => base.Bounds;
			set
			{
				base.Bounds = value;
				_shape.Path = UIBezierPath.FromOval(value).CGPath;
			}
		}
	}
}
