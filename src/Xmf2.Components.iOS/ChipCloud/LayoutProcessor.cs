using System;
using UIKit;
using System.Linq;
using System.Collections.Generic;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.iOS.ChipCloud.Cells;
using Xmf2.iOS.Extensions.Constraints;

namespace Xmf2.Components.iOS.ChipCloud
{
	public class LayoutProcessor : Xmf2Disposable
	{
		private UIView _container;

		public LayoutProcessor(UIView container)
		{
			_container = container;
		}

		public void DesignChilds(float containerWidth, List<ChipCloudItemCell> views, int hMargin = 0, int vMargin = 0)
		{
			if (containerWidth > 0)
			{
				Clear();

				Dictionary<int, List<ChipCloudItemCell>> itemsInRow = GetItemInRows(containerWidth, views, hMargin);
				int nbRow = itemsInRow.Keys.Count;

				foreach (var row in itemsInRow)
				{
					UIView rowView = new UIView().DisposeViewWith(this);
					_container.Add(rowView);
					DesignHorizontal(rowView, row.Value, hMargin);
				}
				DesignVertical(_container, _container.Subviews.ToList(), vMargin);
			}
		}

		public void DesignHorizontal(UIView container, List<ChipCloudItemCell> views, int hMargin = 0)
		{
			if (views?.Count > 0)
			{
				for (int i = 0; i < views.Count; i++)
				{
					ChipCloudItemCell currentView = views[i];
					container.Add(currentView);

					currentView.ConstrainMinWidth(currentView.Width);
					container.AnchorTop(currentView).AnchorBottom(currentView);

					if (i == 0)
					{
						container.AnchorLeft(currentView);
					}
					else
					{
						var precendentView = container.Subviews[i - 1];
						container.HorizontalSpace(precendentView, currentView, hMargin);
					}
				}
				container.IncloseFromRight(container.Subviews.Last());
			}
		}

		public static void DesignVertical(UIView container, List<UIView> views, int verticalMargin = 0)
		{
			if (views?.Count > 0)
			{
				for (int i = 0; i < views.Count; i++)
				{
					UIView currentView = views[i];

					container.CenterAndFillWidth(currentView);
					if (i == 0)
					{
						container.AnchorTop(currentView);
					}
					else
					{
						var precendentView = container.Subviews[i - 1];
						container.VerticalSpace(precendentView, currentView, verticalMargin);
					}
				}
				container.IncloseFromBottom(container.Subviews.Last());
			}
		}

		private static Dictionary<int, List<ChipCloudItemCell>> GetItemInRows(float containerWidth, List<ChipCloudItemCell> views, int hMargin = 0)
		{
			Dictionary<int, List<ChipCloudItemCell>> itemGrouped = new Dictionary<int, List<ChipCloudItemCell>>();
			int currentRow = 0;
			float currentWidth = 0;
			List<ChipCloudItemCell> _currentListInRow = new List<ChipCloudItemCell>();

			foreach (var currentView in views)
			{
				if (currentWidth + currentView.Width > containerWidth)
				{
					//on ajoute la liste courante 
					itemGrouped.Add(currentRow, _currentListInRow);

					//on passe à la ligne suivante
					currentRow++;
					_currentListInRow = new List<ChipCloudItemCell>();
					currentWidth = 0;
				}
				_currentListInRow.Add(currentView);
				currentWidth += (float)currentView.Width + hMargin;
			}
			itemGrouped.Add(currentRow, _currentListInRow);
			return itemGrouped;
		}

		private void Clear()
		{
			if (_container.Subviews != null && _container.Subviews.Length > 0)
			{
				foreach (var v in _container.Subviews)
				{
					v.RemoveFromSuperview();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_container = null;
			}
			base.Dispose(disposing);
		}
	}
}
