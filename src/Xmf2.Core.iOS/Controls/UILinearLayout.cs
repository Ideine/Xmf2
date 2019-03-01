using System;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UILinearLayout : UIBaseLinearLayout
	{
		public enum LayoutOrientation
		{
			Vertical,
			Horizontal
		}

		private LayoutOrientation _orientation;

		public LayoutOrientation Orientation
		{
			get => _orientation;
			set
			{
				if (_orientation != value)
				{
					_orientation = value;
					UpdateConstraintCreator(value);
				}
			}
		}

		public UILinearLayout(LayoutOrientation orientation = LayoutOrientation.Vertical, Func<UIView> separatorCreator = null)
			: base(GetConstraintCreator(orientation), separatorCreator)
		{
			_orientation = orientation;
		}

		private static IConstraintCreator GetConstraintCreator(LayoutOrientation orientation)
		{
			switch (orientation)
			{
				case LayoutOrientation.Vertical: return new VerticalConstraintCreator();
				case LayoutOrientation.Horizontal: return new HorizontalConstraintCreator();
				default:
					throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
			}
		}

		private void UpdateConstraintCreator(LayoutOrientation orientation)
		{
			this.ConstraintCreator = GetConstraintCreator(orientation);
		}
	}
}