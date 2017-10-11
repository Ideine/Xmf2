﻿using System;
using System.Windows.Input;
using CoreGraphics;
using UIKit;
using Xmf2.Commons.iOS.Layout;

namespace Xmf2.Commons.iOS.Controls
{
	public class NavBarWithTwoActions : UIView
	{

		private static readonly UIColor _DEFAULT_TEXT_HIGHLIGHT_COLOR = UIColorExtension.ColorFromHex(0x757575);
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

		public UIColor TitleColor
		{
			get { return Title?.TextColor; }
			set { Title.TextColor = value; }
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

		#endregion

		private static readonly CGAffineTransform _flipTransform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
		public NavBarWithTwoActions(string leftImage, string rightImage)
		{
			this.LeftButton = this.CreateButton().WithTitle(LeftActionTitle).OnClick(OnClickLeftButton).WithTextColorHighlight(_DEFAULT_TEXT_HIGHLIGHT_COLOR);
			this.RightButton = this.CreateButton().WithTitle(RightActionTitle).OnClick(OnClickRightButton).WithTextColorHighlight(_DEFAULT_TEXT_HIGHLIGHT_COLOR);
			this.Title = this.CreateLabel().WithText(TextTitle).WithAlignment(UITextAlignment.Center);

			bool hasLeftImage = !string.IsNullOrEmpty(leftImage);
			bool hasRightImage = !string.IsNullOrEmpty(rightImage);
			LeftButton.ContentEdgeInsets = new UIEdgeInsets(top: 0, left: 15, bottom: 0, right: 0);
			RightButton.ContentEdgeInsets = new UIEdgeInsets(top: 0, left: 0, bottom: 0, right: 15);
			if (hasLeftImage)
			{
				LeftButton.WithImage(leftImage);
			}
			if (hasRightImage)
			{
				RightButton.WithImage(rightImage);
			}
			_container = this.CreateView();
			_container.AddSubviews(Title, LeftButton, RightButton);

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
					  .FillHeight(RightButton)
					  ;

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

		#endregion

		#region Static methods helper

		public static NavBarWithTwoActions Create(string title)
		{
			return new NavBarWithTwoActions(null, null)
			{
				TextTitle = title,
				HasLeftAction = false,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions CreateSimpleBack(string title, string leftText)
		{
			return new NavBarWithTwoActions(null, null)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string leftImage = null)
		{
			return new NavBarWithTwoActions(leftImage, null)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string rightText, string leftImage = null)
		{
			return new NavBarWithTwoActions(leftImage, null)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				RightActionTitle = rightText
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, Action leftAction, string leftImage = null)
		{
			return new NavBarWithTwoActions(leftImage, null)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				LeftAction = leftAction,
				HasRightAction = false
			};
		}

		public static NavBarWithTwoActions Create(string title, string leftText, string rightText, Action leftAction, Action rightAction, string leftImage, string rightImage)
		{
			return new NavBarWithTwoActions(leftImage, rightImage)
			{
				TextTitle = title,
				LeftActionTitle = leftText,
				RightActionTitle = rightText,
				LeftAction = leftAction,
				RightAction = rightAction,
			};
		}

		#endregion
	}
}
