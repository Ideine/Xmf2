using UIKit;
using CoreGraphics;
using Xmf2.Core.Subscriptions;
using System.Collections.Generic;
using Xmf2.Components.iOS.ChipCloud.Cells;

namespace Xmf2.Components.iOS.ChipCloud
{
	public class ChipCloudView : UIView
	{
		private Xmf2Disposable _disposer = new Xmf2Disposable();

		private LayoutProcessor _layoutProcessor;

		private List<ChipCloudItemCell> _views = new List<ChipCloudItemCell>();

		public IChipCloudItemSource Source { get; set; }

		public override CGRect Bounds
		{
			get => base.Bounds;
			set
			{
				base.Bounds = value;
				SetItems(_views);
			}
		}

		public int ItemHorizontalMargin { get; set; }

		public int ItemVerticalMargin { get; set; }

		public ChipCloudView()
		{
			_layoutProcessor = new LayoutProcessor(this).DisposeWith(_disposer);
		}

		public void ReloadData()
		{
			if (Source == null)
			{
				return;
			}

			List<ChipCloudItemCell> subviews = new List<ChipCloudItemCell>();
			for (int position = 0; position < Source.Count; position++)
			{
				ChipCloudItemCell cell = Source.GetCell(this, position);
				subviews.Add(cell);
			}

			SetItems(subviews);
		}

		public void SetItems(List<ChipCloudItemCell> subviews)
		{
			_views = null;

			if (subviews == null)
			{
				return;
			}

			_views = subviews;

			UpdateFrames();
		}

		private void ClearContent()
		{
			if (Subviews != null && Subviews.Length > 0)
			{
				foreach (var v in Subviews)
				{
					v.RemoveFromSuperview();
				}
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			UpdateFrames();
		}

		private void UpdateFrames()
		{
			_layoutProcessor.DesignChilds((float)Bounds.Width, _views, ItemHorizontalMargin, ItemVerticalMargin);

			foreach (UIView view in Subviews)
			{
				view.SetNeedsLayout();
				view.LayoutIfNeeded();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposer?.Dispose();
				_disposer = null;
			}
			base.Dispose(disposing);
		}
	}
}
