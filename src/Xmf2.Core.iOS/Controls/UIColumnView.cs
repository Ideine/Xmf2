using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Core.iOS.Extensions;
using Xmf2.iOS.Extensions.Extensions;
using Xmf2.iOS.Extensions.Constraints;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class UIColumnView : UIView
	{
		private List<SubviewConstraints> _subviewConstraints;

		public UIColumnView()
		{
			_subviewConstraints = new List<SubviewConstraints>();
		}

		public void SetSubviews(UIView[] subviews)
		{
			if (SubviewsAreDifferent(subviews, Subviews))
			{
				this.EnsureRemove(Subviews);
				AddSubviews(subviews);
				SetNeedsUpdateConstraints();
			}
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();

			var subviews = Subviews.ToArray();

			this.EnsureRemove(_subviewConstraints.SelectMany(o => o.AllConstraints()));
			_subviewConstraints.Clear();

			for (int i = 0; i < subviews.Length; i++)
			{
				UIView subView = subviews[i];
				subView.TranslatesAutoresizingMaskIntoConstraints = false;
				float multiplier = ((i * 2f) + 1f) / subviews.Length;
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
					foreach (var constraint in allConstraints)
					{
						constraint.Dispose();
					}
					_subviewConstraints.Clear();
				}
				_subviewConstraints = null;
			}
			base.Dispose(disposing);
		}
	}
}
