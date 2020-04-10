using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Core.iOS.Extensions;
using Xmf2.Core.Subscriptions;
using Xmf2.iOS.Extensions.Constraints;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Core.iOS.Controls
{
	public class UIByCaseView<TCaseEnum> : UIView
	{
		private readonly Xmf2Disposable _disposable;

		private readonly bool _currentCaseOnceSet = false;
		private TCaseEnum _currentCase;
		private ViewInfo _currentInfo;
		private readonly Dictionary<TCaseEnum, ViewInfo> _byCaseInfo;
		private bool _aggressiveViewDispose = false;

		public UIByCaseView(Dictionary<TCaseEnum, Func<UIView>> viewFactoryByCase)
		{
			_disposable = new Xmf2Disposable();
			_currentCase = default;

			_byCaseInfo = viewFactoryByCase.ToDictionary(
				keySelector: kvp => kvp.Key,
				elementSelector: kvp => new ViewInfo { NewViewFactory = kvp.Value }.DisposeWith(_disposable),
				comparer: viewFactoryByCase.Comparer
			);
		}

		public UIByCaseView<TCaseEnum> WithCase(TCaseEnum caseValue)
		{
			if (   !_currentCaseOnceSet
				|| !_byCaseInfo.Comparer.Equals(_currentCase, caseValue))
			{
				_currentCase = caseValue;
				this.SetNeedsUpdateConstraints();//<-- request os to call method UpdateConstraints at the most appropriate moment.
			}
			return this;
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();

			ViewInfo newInfo = _byCaseInfo[_currentCase];
			if (newInfo != _currentInfo)
			{
				this.EnsureRemove(_currentInfo?.GetContraintsFor(parentView: this));
				this.EnsureRemove(this.Subviews);
				if (_aggressiveViewDispose)
				{
					_currentInfo?.DisposeConstraints();
					_currentInfo?.DisposeView();
				}

				var newView = newInfo.GetView();
				newView.TranslatesAutoresizingMaskIntoConstraints = false;
				this.AddSubview(newView);
				this.AddConstraints(newInfo.GetContraintsFor(parentView: this));
				_currentInfo = newInfo;
			}
		}

		public UIByCaseView<TCaseEnum> WithAggressiveViewDispose(bool aggressive = true)
		{
			_aggressiveViewDispose = aggressive;
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_currentCase = default;
				_disposable.Dispose();
			}
			base.Dispose(disposing);
		}

		private class ViewInfo : IDisposable
		{
			public Func<UIView> NewViewFactory { private get; set; }
			private UIView _view;
			private NSLayoutConstraint[] _viewConstraints;

			internal UIView GetView() => this._view ?? (_view = NewViewFactory());

			internal NSLayoutConstraint[] GetContraintsFor(UIView parentView)
			{
				if (_viewConstraints is null)
				{
					_viewConstraints = new NSLayoutConstraint[]
					{
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.CenterX , NSLayoutRelation.Equal, this.GetView(), NSLayoutAttribute.CenterX, 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.CenterY , NSLayoutRelation.Equal, this.GetView(), NSLayoutAttribute.CenterY, 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.Width   , NSLayoutRelation.Equal, this.GetView(), NSLayoutAttribute.Width  , 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.Height  , NSLayoutRelation.Equal, this.GetView(), NSLayoutAttribute.Height , 1f, 0f).WithAutomaticIdentifier(),
					};
				}
				return _viewConstraints;
			}

			internal void DisposeView()
			{
				_view?.Dispose();
				_view = null;
			}
			internal void DisposeConstraints()
			{
				if (_viewConstraints is null)
				{
					return;
				}
				for (int i = 0; i < _viewConstraints.Length; i++)
				{
					_viewConstraints[i].Dispose();
				}
				_viewConstraints = null;
			}

			#region IDisposable Support

			private bool _disposedValue = false;
			protected virtual void Dispose(bool disposing)
			{
				if (!_disposedValue)
				{
					if (disposing)
					{
						NewViewFactory = null;
						DisposeView();
						DisposeConstraints();
					}
					_disposedValue = true;
				}
			}
			public void Dispose() => Dispose(true);

			#endregion IDisposable Support
		}
	}
}
