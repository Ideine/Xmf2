using System;
using UIKit;
using CoreGraphics;
using Xmf2.iOS.Extensions.Constraints;

namespace Xmf2.Components.iOS.ChipCloud.Cells
{
	public class ChipCloudItemCell : UIView
	{
		public UIView _content;

		public CGSize? ItemSize { get; set; }

		public float Width => ItemSize.HasValue ? (float)ItemSize.Value.Width : 0;

		public bool HasItemSize => ItemSize.HasValue && ItemSize.Value.Width != 0 && ItemSize.Value.Height != 0;

		public ChipCloudItemCell() { }

		public void SetContent(UIView contentView)
		{
			_content = contentView;
			Add(_content);
			AutoLayout();
		}

		protected virtual void AutoLayout()
		{
			this.Same(_content);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_content = null;
			}
			base.Dispose(disposing);
		}
	}
}
