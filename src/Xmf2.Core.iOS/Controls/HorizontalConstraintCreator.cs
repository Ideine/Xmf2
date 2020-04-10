using UIKit;
using Xmf2.Core.iOS.Extensions;

namespace Xmf2.Core.iOS.Controls
{
	public class HorizontalConstraintCreator : UIBaseLinearLayout.IConstraintCreator
	{
		public static NSLayoutConstraint AnchorStart(UIView container, UIView cell)
		{
			return NSLayoutConstraint.Create(cell, NSLayoutAttribute.Left, NSLayoutRelation.Equal, container, NSLayoutAttribute.Left, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint Space(UIView previousCell, UIView nextCell)
		{
			return NSLayoutConstraint.Create(nextCell, NSLayoutAttribute.Left, NSLayoutRelation.Equal, previousCell, NSLayoutAttribute.Right, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint AnchorEnd(UIView cell, UIView container)
		{
			return NSLayoutConstraint.Create(container, NSLayoutAttribute.Right, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Right, 1f, 0).WithAutomaticIdentifier();
		}

		public static NSLayoutConstraint[] FillSize(UIView container, UIView cell)
		{
			return new[]
			{
				NSLayoutConstraint.Create(container, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, cell, NSLayoutAttribute.CenterY, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(container, NSLayoutAttribute.Height, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Height, 1f, 0).WithAutomaticIdentifier(),
			};
		}

		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorEnd(UIView cell, UIView container) => AnchorEnd(cell, container);
		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorStart(UIView container, UIView cell) => AnchorStart(container, cell);
		NSLayoutConstraint[] UIBaseLinearLayout.IConstraintCreator.FillSize(UIView container, UIView cell) => FillSize(container, cell);

		NSLayoutConstraint[] UIBaseLinearLayout.IConstraintCreator.Space(UIView previousCell, UIView nextCell)
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
				NSLayoutConstraint.Create(container,NSLayoutAttribute.Height,NSLayoutRelation.Equal,separator,NSLayoutAttribute.Height,1,0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(container,NSLayoutAttribute.CenterX,NSLayoutRelation.Equal,separator,NSLayoutAttribute.CenterX,1,0).WithAutomaticIdentifier()
			};
		}
	}
}