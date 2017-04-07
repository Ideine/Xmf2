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

		public void Activate()
		{
			NSLayoutConstraint.ActivateConstraints(this.Constraints.ToArray());
		}

		public void Deactivate()
		{
			NSLayoutConstraint.DeactivateConstraints(this.Constraints.ToArray());
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