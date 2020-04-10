using UIKit;
using Xmf2.Core.iOS.Extensions;
using static Xmf2.Core.iOS.Controls.UIBaseLinearLayout;

namespace Xmf2.Core.iOS.Controls
{
	public class VerticalConstraintCreator : IConstraintCreator
	{
		public static NSLayoutConstraint AnchorStart(UIView container, UIView cell)
		{
			return NSLayoutConstraint.Create(cell, NSLayoutAttribute.Top, NSLayoutRelation.Equal, container, NSLayoutAttribute.Top, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint Space(UIView previousCell, UIView nextCell)
		{
			return NSLayoutConstraint.Create(nextCell, NSLayoutAttribute.Top, NSLayoutRelation.Equal, previousCell, NSLayoutAttribute.Bottom, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint AnchorEnd(UIView cell, UIView container)
		{
			return NSLayoutConstraint.Create(container, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Bottom, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint[] FillSize(UIView container, UIView cell)
		{
			return new[]
			{
				NSLayoutConstraint.Create(container, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, cell, NSLayoutAttribute.CenterX, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(container, NSLayoutAttribute.Width, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Width, 1f, 0).WithAutomaticIdentifier(),
			};
		}

		NSLayoutConstraint IConstraintCreator.AnchorEnd(UIView cell, UIView container) => AnchorEnd(cell, container);
		NSLayoutConstraint IConstraintCreator.AnchorStart(UIView container, UIView cell) => AnchorStart(container, cell);
		NSLayoutConstraint[] IConstraintCreator.FillSize(UIView container, UIView cell) => FillSize(container, cell);

		NSLayoutConstraint[] IConstraintCreator.Space(UIView previousCell, UIView nextCell)
		{
			return new[] { Space(previousCell, nextCell) };
		}

		public NSLayoutConstraint[] Space(UIView previousCell, UIView separator, UIView nextCell)
		{
			return new[] { Space(previousCell, separator), Space(separator, nextCell) };
		}

		public NSLayoutConstraint[] FillSizeSeparator(UIView container, UIView separator)
		{
			return new[]{
				NSLayoutConstraint.Create(container, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, separator, NSLayoutAttribute.CenterX, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(container, NSLayoutAttribute.Width, NSLayoutRelation.Equal, separator, NSLayoutAttribute.Width, 1f, 0).WithAutomaticIdentifier(),
			};
		}
	}
}