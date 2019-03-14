using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Controls
{
	public class UIByCaseView<TCaseEnum> : UIView
	{
		private readonly Xmf2Disposable _disposable;

		private bool _currentCaseOnceSet = false;
		private TCaseEnum _currentCase;
		private Dictionary<TCaseEnum, UIView> _viewByCase;
		private Dictionary<UIView, NSLayoutConstraint[]> _constraintsByView;

		public UIByCaseView(Dictionary<TCaseEnum, UIView> viewByCase)
		{
			_disposable = new Xmf2Disposable();
			_currentCase = default(TCaseEnum);
			_viewByCase = viewByCase;
			_constraintsByView = new Dictionary<UIView, NSLayoutConstraint[]>();
		}

		private NSLayoutConstraint[] GetContraintsFor(UIView view)
		{
			if (_constraintsByView.TryGetValue(view, out var constraints))
			{
				return constraints;
			}
			else
			{
				var newConstraints = new NSLayoutConstraint[]
				{
					NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f).WithAutomaticIdentifier().DisposeWith(_disposable),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterY , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f).WithAutomaticIdentifier().DisposeWith(_disposable),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.Width   , NSLayoutRelation.Equal, view, NSLayoutAttribute.Width  , 1f, 0f).WithAutomaticIdentifier().DisposeWith(_disposable),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.Height  , NSLayoutRelation.Equal, view, NSLayoutAttribute.Height , 1f, 0f).WithAutomaticIdentifier().DisposeWith(_disposable),
				};
				_constraintsByView.Add(view, newConstraints);
				return newConstraints;
			}
		}

		public UIByCaseView<TCaseEnum> WithCase(TCaseEnum caseValue)
		{
			if (   !_currentCaseOnceSet
				|| !_viewByCase.Comparer.Equals(_currentCase, caseValue))
			{
				_currentCase = caseValue;
				this.SetNeedsUpdateConstraints();//<-- request os to call method UpdateConstraints at the most appropriate moment.
			}
			return this;
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();

			var view = _viewByCase[_currentCase];
			if (this.Subviews.FirstOrDefault() != view)
			{
				this.EnsureRemove(_constraintsByView.SelectMany(x => x.Value));
				this.EnsureRemove(this.Subviews);
				view.TranslatesAutoresizingMaskIntoConstraints = false;
				this.AddSubview(view);
				this.AddConstraints(GetContraintsFor(view));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_currentCase = default(TCaseEnum);
				_viewByCase = null;
				_constraintsByView = null;
				_disposable.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
