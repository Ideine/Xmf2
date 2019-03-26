using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class UIColumnView : UIView
	{
		private List<SubviewConstraints> _subviewConstraints;
		private UIView[] _subviews;

		public UIColumnView()
		{
			_subviewConstraints = new List<SubviewConstraints>();
			_subviews = new UIView[0];
		}


		public void SetSubviews(UIView[] subviews)
		{
			this.EnsureRemove(this.Subviews);
			this.AddSubviews(subviews);
			this.SetNeedsUpdateConstraints();
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();

			if (SubviewsAreDifferent(_subviews, this.Subviews))
			{
				_subviews = this.Subviews.ToArray();

				this.EnsureRemove(_subviewConstraints.SelectMany(o => o.AllConstraints()));
				_subviewConstraints.Clear();

				for (int i = 0; i < _subviews.Length; i++)
				{
					UIView subView = _subviews[i];
					subView.TranslatesAutoresizingMaskIntoConstraints = false;
					float multiplier = ((i * 2f) + 1f) / _subviews.Length;
					var newConstraints = new SubviewConstraints
					{
						HCenterX = NSLayoutConstraint.Create(subView, CenterX, Equal, this, CenterX, multiplier, 0f).WithAutomaticIdentifier(),
						VCenterY = NSLayoutConstraint.Create(subView, CenterY, Equal, this, CenterY, 1f, 0f).WithAutomaticIdentifier(),
						VMaxHeight = NSLayoutConstraint.Create(subView, Height, LessThanOrEqual, this, Height, 1f, 0f).WithAutomaticIdentifier()
					};
					_subviewConstraints.Add(newConstraints);
					this.EnsureAdd(newConstraints.AllConstraints());
				}
			}
		}

		private static bool SubviewsAreDifferent(UIView[] subviewsA, UIView[] subviewsB)
		{
			if (subviewsA.Length != subviewsB.Length)
			{
				return true;
			}
			for (int i = 0; i < subviewsB.Length; i++)
			{
				if (subviewsA[i] != subviewsB[i])
				{
					return true;
				}
			}
			return false;
		}

		#region Nested Types

		private class SubviewConstraints
		{
			public NSLayoutConstraint HCenterX { get; set; }
			public NSLayoutConstraint VCenterY { get; set; }
			public NSLayoutConstraint VMaxHeight { get; set; }
			public NSLayoutConstraint[] AllConstraints() => new[] { HCenterX, VCenterY, VMaxHeight };
		}

		#endregion Nested Types

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				var allConstraints = _subviewConstraints?.SelectMany(o => o.AllConstraints()).ToList();
				if (allConstraints != null)
				{
					this.EnsureRemove(allConstraints);
					allConstraints.ForEach(c => c.Dispose());
					_subviewConstraints.Clear();
				}
				_subviewConstraints = null;
				_subviews = null;
			}
			base.Dispose(disposing);
		}
	}
}
