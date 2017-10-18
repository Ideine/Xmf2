using System;
using Foundation;
using UIKit;

namespace Xmf2.iOS.Controls.ItemControls
{
	public abstract class BaseItemCell<TModel> : BaseItemCell, IUIModelComponent<TModel>, IDisposable where TModel : class
	{
		protected BaseItemCell() { }

		private object _dataContext;
		public virtual object DataContext
		{
			get => _dataContext;
			set => _dataContext = value;
		}

		public TModel Model
		{
			get => (TModel)DataContext;
			set => DataContext = value;
		}

		public int Position { get; set; }

		public UIColor SelectedColor { get; set; }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_dataContext = null;
			}
			base.Dispose(disposing);
		}
	}

	public class BaseItemCell : UIView, IUIComponent
	{
		[Export("requiresConstraintBasedLayout")]
		public static new bool RequiresConstraintBasedLayout() => true;

		public virtual void AutoLayout() { }

		public virtual void Bind() { }

		public virtual void ViewDidLoad() { }

		public virtual void ViewDidAppear() { }

		public virtual void ViewWillDisappear() { }

		public virtual void ViewDidDisappear() { }
	}
}

