using System;
using System.Windows.Input;
using Xmf2.Commons.Extensions;
using UIKit;
using Xmf2.Commons.MvxExtends.Touch.Layout;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public class NavBarWithTwoActions : UIView
	{
		private bool _layoutDone;

		private readonly UIView _container;

		#region Public Properties

		public Action LeftAction { get; set; }

		public Action RightAction { get; set; }

		public ICommand LeftCommand
		{
			get { return null; }
			set { LeftAction = () => value?.TryExecute(); }
		}

		public ICommand RightCommand
		{
			get { return null; }
			set { RightAction = () => value?.TryExecute(); }
		}

		public string LeftActionTitle
		{
			get { return LeftButton?.Title(UIControlState.Normal); }
			set { LeftButton.WithTitle(value); }
		}

		public string RightActionTitle
		{
			get { return RightButton?.Title(UIControlState.Normal); }
			set { RightButton.WithTitle(value); }
		}

		public string TextTitle
		{
			get { return Title?.Text; }
			set { Title.WithText(value); }
		}

		public UIFont TextTitleFont
		{
			get { return Title?.Font; }
			set { Title.WithFont(value); }
		}

		public bool HasLeftAction
		{
			get { return LeftButton?.Hidden ?? false; }
			set { LeftButton.Hidden = !value; }
		}

		public bool HasRightAction
		{
			get { return RightButton?.Hidden ?? false; }
			set { RightButton.Hidden = !value; }
		}

		public UIButton LeftButton { get; }

		public UIButton RightButton { get; }

		public UILabel Title { get; }

		#endregion Public Properties

		public NavBarWithTwoActions(string withLeftImage)
		{
			bool hasBackImage = !string.IsNullOrEmpty(withLeftImage);

			this.LeftButton  = this.CreateButton().WithTitle(LeftActionTitle).OnClick(OnClickLeftButton);
			this.RightButton = this.CreateButton().WithTitle(RightActionTitle).OnClick(OnClickRightButton);
			this.Title = this.CreateLabel().WithText(TextTitle).WithAlignment(UITextAlignment.Center);

			if (hasBackImage)
			{
				this.LeftButton.WithImage(withLeftImage);
				//LeftButton.ContentEdgeInsets = LeftButton.ContentEdgeInsets.Add(right: 4);
				//LeftButton.TitleEdgeInsets = new UIEdgeInsets(0, 4, 0, -4);
			}
			else
			{
				this.LeftButton.ContentEdgeInsets = new UIEdgeInsets(0, 8, 0, 8);
				this.RightButton.ContentEdgeInsets = new UIEdgeInsets(0, 8, 0, 8);
			}

			_container = this.CreateView()
							 .WithSubviews(Title, LeftButton, RightButton);

			this.Add(_container);
		}

		public void AutoLayout()
		{
			if (!_layoutDone)
			{
				_layoutDone = true;
				ApplyAutoLayout();
			}
		}

		protected virtual void ApplyAutoLayout()
		{
			//Vertical Layout
			this.AnchorTop(_container, LayoutConsts.UIStatusBar_DefaultHeight)
				.AnchorBottom(_container);

			_container.CenterVertically(LeftButton)
					  .CenterVertically(Title)
					  .CenterVertically(RightButton)
					  .FillHeight(LeftButton)
					  .FillHeight(RightButton);

			//Horizontal Layout
			this.CenterAndFillWidth(_container);
			_container.AnchorLeft(LeftButton)
					  .AnchorRight(RightButton)
					  .CenterAndFillWidth(Title);
		}

		public virtual void OnClickLeftButton()
		{
			LeftAction?.Invoke();
		}

		public virtual void OnClickRightButton()
		{
			RightAction?.Invoke();
		}

		#region Dispose

		private bool _disposed;
		protected override void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				try
				{
					if (disposing)// Release managed resources.
					{
						LeftAction = null;
						RightAction = null;
					}
					// Release unmanaged resources...
					_disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		#endregion Dispose

		#region Static methods helper

		public static NavBarWithTwoActions Create(string title)
		{
			return new NavBarWithTwoActions(null)
			{
				TextTitle = title,
				HasLeftAction = false,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions CreateSimpleBack(string title, string leftText)
		{
			return new NavBarWithTwoActions(null)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string withBackImage = null)
		{
			return new NavBarWithTwoActions(withBackImage)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string rightText, string withBackImage = null)
		{
			return new NavBarWithTwoActions(withBackImage)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				RightActionTitle = rightText
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, Action leftAction, string withBackImage = null)
		{
			return new NavBarWithTwoActions(withBackImage)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				LeftAction = leftAction,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string rightText, Action leftAction, Action rightAction, string withBackImage = null)
		{
			return new NavBarWithTwoActions(withBackImage)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				RightActionTitle = rightText,
				LeftAction = leftAction,
				RightAction = rightAction,
			};
		}

		#endregion  Static methods helper
	}
}