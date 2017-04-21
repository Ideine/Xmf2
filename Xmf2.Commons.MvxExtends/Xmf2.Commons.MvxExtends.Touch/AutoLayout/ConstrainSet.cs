using System;
using System.Collections.Generic;
using UIKit;

namespace Xmf2.Commons.MvxExtends.Touch.AutoLayout
{
	public class ConstrainSet<TUIView> : IDisposable
	{
		public ConstrainSet(TUIView view)
		{
			this.View = view;
			this.Constraints = new List<NSLayoutConstraint>();
		}

		public TUIView View { get; private set; }

		public List<NSLayoutConstraint> Constraints { get; private set; }

		public ConstrainSet<TUIView> Activate()
		{
			NSLayoutConstraint.ActivateConstraints(this.Constraints.ToArray());
			return this;
		}

		public ConstrainSet<TUIView> Deactivate()
		{
			NSLayoutConstraint.DeactivateConstraints(this.Constraints.ToArray());
			return this;
		}

		#region IDisposable

		private bool _disposed = false;

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				try
				{
					if (disposing)// Release managed resources.
					{
						foreach (var constraint in this.Constraints)
						{
							constraint.Dispose();
						}
					}
					// Release unmanaged resources...
					this._disposed = true;
				}
				catch
				{
					//ignored
				}
			}
		}
		~ConstrainSet()
		{
			this.Dispose(false);
		}

		#endregion IDisposable
	}
}