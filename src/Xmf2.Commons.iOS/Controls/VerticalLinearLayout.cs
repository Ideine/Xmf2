using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public abstract class BaseLinearLayout : UIView
	{
		private UIView _lastView;
		private NSLayoutConstraint _endConstraint;

		public BaseLinearLayout()
		{
			TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public List<UIView> Clear()
		{
			_lastView = null;
			_endConstraint = null;

			var views = Subviews.ToList();
			foreach (UIView view in views)
			{
				view.RemoveFromSuperview();
			}
			RemoveConstraints(Constraints);
			return views;
		}

		public void AddItem(UIView item)
		{
			AddSubview(item);
			item.TranslatesAutoresizingMaskIntoConstraints = false;

			AddConstraint(FillSizeConstraint(item));
			AddConstraint(CenterConstraint(item));


			if (_lastView == null)
			{
				NSLayoutConstraint top = StartConstraint(item);
				AddConstraint(top);
			}
			else
			{
				RemoveConstraint(_endConstraint);
				AddConstraint(Space(_lastView, item));
			}

			_lastView = item;
			_endConstraint = EndConstraint(_lastView);
			AddConstraint(_endConstraint);
		}

		public void SetItems(params UIView[] items)
		{
			this.Clear();
			this.AddItems(items);
		}
		
		public void AddItems(params UIView[] items)
		{
			if (items.Length == 0)
			{
				return;
			}
			
			AddSubviews(items);

			NSLayoutConstraint[] constraints = new NSLayoutConstraint[items.Length * 2];
			NSLayoutConstraint[] spaceConstraints = new NSLayoutConstraint[items.Length - 1];
			for (int index = 0, constraintsOffset = 0; index < items.Length; index++, constraintsOffset += 2)
			{
				UIView item = items[index];
				item.TranslatesAutoresizingMaskIntoConstraints = false;
				constraints[constraintsOffset] = FillSizeConstraint(item);
				constraints[constraintsOffset + 1] = CenterConstraint(item);

				if (index > 0)
				{
					spaceConstraints[index - 1] = Space(items[index - 1], item);
				}
			}

			AddConstraints(constraints);
			AddConstraints(spaceConstraints);

			if (_lastView == null)
			{
				NSLayoutConstraint top = StartConstraint(items[0]);
				AddConstraint(top);
			}
			else
			{
				RemoveConstraint(_endConstraint);
				AddConstraint(Space(_lastView, items[0]));
			}

			_lastView = items[items.Length - 1];
			_endConstraint = EndConstraint(_lastView);
			AddConstraint(_endConstraint);
		}

		protected abstract NSLayoutConstraint StartConstraint(UIView item);

		protected abstract NSLayoutConstraint EndConstraint(UIView item);

		protected abstract NSLayoutConstraint FillSizeConstraint(UIView item);

		protected abstract NSLayoutConstraint CenterConstraint(UIView item);

		protected abstract NSLayoutConstraint Space(UIView first, UIView second);
	}
	
	public class VerticalLinearLayout : BaseLinearLayout
	{
		protected override NSLayoutConstraint StartConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Top, NSLayoutRelation.Equal, item, NSLayoutAttribute.Top, 1f, 0);

		protected override NSLayoutConstraint EndConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, item, NSLayoutAttribute.Bottom, 1f, 0);

		protected override NSLayoutConstraint FillSizeConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Width, NSLayoutRelation.Equal, item, NSLayoutAttribute.Width, 1f, 0);

		protected override NSLayoutConstraint CenterConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, item, NSLayoutAttribute.CenterX, 1f, 0);

		protected override NSLayoutConstraint Space(UIView first, UIView second) => NSLayoutConstraint.Create(first, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, second, NSLayoutAttribute.Top, 1f, 0);
	}
	
	public class HorizontalLinearLayout : BaseLinearLayout
	{
		protected override NSLayoutConstraint StartConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Left, NSLayoutRelation.Equal, item, NSLayoutAttribute.Left, 1f, 0);

		protected override NSLayoutConstraint EndConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Right, NSLayoutRelation.Equal, item, NSLayoutAttribute.Right, 1f, 0);

		protected override NSLayoutConstraint FillSizeConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, item, NSLayoutAttribute.Height, 1f, 0);

		protected override NSLayoutConstraint CenterConstraint(UIView item) => NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, item, NSLayoutAttribute.CenterY, 1f, 0);

		protected override NSLayoutConstraint Space(UIView first, UIView second) => NSLayoutConstraint.Create(first, NSLayoutAttribute.Right, NSLayoutRelation.Equal, second, NSLayoutAttribute.Left, 1f, 0);
	}
}