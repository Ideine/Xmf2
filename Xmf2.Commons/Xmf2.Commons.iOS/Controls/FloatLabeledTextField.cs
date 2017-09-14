﻿using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;

namespace Xmf2.Commons.iOS.Controls
{
	[Register("FloatLabeledTextField"), DesignTimeVisible(true)]
	public class FloatLabeledTextField : UITextField
	{

		protected enum ActivationMode
		{
			OnFocus,
			OnFirstCharacter
		}

		protected enum StateEnum
		{
			Unknow, Normal, Disabled, Focused
		}

		private StateEnum _oldState;

		protected ActivationMode Mode;

		public override bool Enabled
		{
			get => base.Enabled;
			set
			{
				if (value != base.Enabled)
				{
					base.Enabled = value;
					HandleState();
				}
			}
		}

		private string _placeholder;
		public new string Placeholder
		{
			get => _placeholder;
			set
			{
				_placeholder = value;
				_floatingLabel.Text = value;
				_floatingLabel.SizeToFit();
				_floatingLabel.Frame =
					new CGRect(
						0, _floatingLabel.Font.LineHeight,
						_floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);

			}
		}

		public event EventHandler TextCleared;

		public event EventHandler BecomeFirstResponderEvent;
		public event EventHandler ResignFirstResponderEvent;

		private UILabel _floatingLabel;

		private Action _downFloatLabel;
		private Action _upFloatLabel;

		protected UIColor FloatingLabelNormalTextColor { get; set; }
		protected UIColor FloatingLabelFocusedTextColor { get; set; }
		protected UIColor FloatingLabelDisabledTextColor { get; set; }

		protected UIFont FloatingLabelNormalFont { get; set; }
		protected UIFont FloatingLabelFocusedFont { get; set; }
		protected UIFont FloatingLabelDisabledFont { get; set; }

		protected UIFont PlaceholderFont { get; set; }
		protected UIColor PlaceholderColor { get; set; }

		protected UIFont TextNormalFont { get; set; }
		protected UIFont TextDisabledFont { get; set; }
		protected UIFont TextFocusedFont { get; set; }

		protected UIColor TextNormalColor { get; set; }
		protected UIColor TextDisabledColor { get; set; }
		protected UIColor TextFocusedColor { get; set; }

		#region Constructor

		public FloatLabeledTextField(IntPtr p) : base(p)
		{
			this.InitFloatingLabel();
		}

		protected FloatLabeledTextField()
		{
			this.InitFloatingLabel();
		}

		#endregion

		private void InitFloatingLabel()
		{
			_floatingLabel = new UILabel();

			AddSubview(_floatingLabel);

			_downFloatLabel = () =>
			{
				Animate(0.3f, 0.0f,
						UIViewAnimationOptions.BeginFromCurrentState
						| UIViewAnimationOptions.CurveEaseOut,
						() =>
						{
							_floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X,
															  _floatingLabel.Font.LineHeight,
															  _floatingLabel.Frame.Size.Width,
															  _floatingLabel.Frame.Size.Height);
							_floatingLabel.Font = PlaceholderFont;
							_floatingLabel.TextColor = PlaceholderColor;
						},
						() => { });
			};

			_upFloatLabel = () =>
			{
				Animate(0.3f, 0.0f,
						UIViewAnimationOptions.BeginFromCurrentState
						| UIViewAnimationOptions.CurveEaseOut,
						() =>
						{
							HandleState(true);
							_floatingLabel.Frame = new CGRect(_floatingLabel.Frame.Location.X,
															  0.0f,
															  _floatingLabel.Frame.Size.Width,
															  _floatingLabel.Frame.Size.Height);
						},
						() => { });
			};
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			HandleState();
			HandleChange();
		}

		private void HandleChange()
		{
			switch (Mode)
			{
				case ActivationMode.OnFocus:
					if (IsFirstResponder)
					{
						_upFloatLabel();
					}
					else if (!string.IsNullOrEmpty(Text))
					{
						_upFloatLabel();
					}
					else
					{
						_downFloatLabel();
					}
					return;
				case ActivationMode.OnFirstCharacter:
					if (!string.IsNullOrEmpty(Text))
					{
						_upFloatLabel();
					}
					else
					{
						_downFloatLabel();
					}
					return;
			}

		}

		protected void HandleState(bool force = false)
		{
			if (IsFirstResponder)
			{
				if (force || _oldState != StateEnum.Focused)
				{
					_oldState = StateEnum.Focused;
					_floatingLabel.TextColor = FloatingLabelFocusedTextColor;
					_floatingLabel.Font = FloatingLabelFocusedFont;
					TextColor = TextFocusedColor;
					Font = TextFocusedFont;
				}
			}
			else if (Enabled)
			{
				if (force || _oldState != StateEnum.Normal)
				{
					_oldState = StateEnum.Normal;
					_floatingLabel.TextColor = FloatingLabelNormalTextColor;
					_floatingLabel.Font = FloatingLabelNormalFont;
					TextColor = TextNormalColor;
					Font = TextNormalFont;
				}
			}
			else
			{
				if (force || _oldState != StateEnum.Disabled)
				{
					_oldState = StateEnum.Disabled;
					_floatingLabel.TextColor = FloatingLabelDisabledTextColor;
					_floatingLabel.Font = FloatingLabelDisabledFont;
					TextColor = TextDisabledColor;
					Font = TextDisabledFont;
				}
			}
		}

		#region Rect

		public override CGRect TextRect(CGRect forBounds)
		{
			if (_floatingLabel == null)
			{
				return base.TextRect(forBounds);
			}
			else
			{
				return InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
			}
		}

		public override CGRect EditingRect(CGRect forBounds)
		{
			if (_floatingLabel == null)
			{
				return base.EditingRect(forBounds);
			}
			else
			{
				return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
			}
		}

		public override CGRect ClearButtonRect(CGRect forBounds)
		{
			var rect = base.ClearButtonRect(forBounds);

			if (_floatingLabel == null)
			{
				return rect;
			}
			else
			{
				return new CGRect(
					rect.X, rect.Y + _floatingLabel.Font.LineHeight / 2.0f,
					rect.Size.Width, rect.Size.Height);
			}
		}

		private static CGRect InsetRect(CGRect rect, UIEdgeInsets insets)
		{
			return new CGRect(
				rect.X + insets.Left,
				rect.Y + insets.Top,
				rect.Width - insets.Left - insets.Right,
				rect.Height - insets.Top - insets.Bottom);
		}

		#endregion


		#region Responder Override

		public override bool BecomeFirstResponder()
		{
			HandleState();
			BecomeFirstResponderEvent?.Invoke(this, EventArgs.Empty);
			return base.BecomeFirstResponder();
		}

		public override bool ResignFirstResponder()
		{
			HandleState();
			ResignFirstResponderEvent?.Invoke(this, EventArgs.Empty);
			return base.ResignFirstResponder();
		}

		#endregion

		public void ClearText()
		{
			Text = string.Empty;
			TextCleared?.Invoke(this, EventArgs.Empty);
		}



		#region Constructor Method

		public static FloatLabeledTextField CreateOnFocusTextField(UIColor textColor, UIFont textFont,
																   string placeholder,
																   UIColor cursorColor,
																   UIColor floatLabelColor, UIFont floatLabelFont,
																   UIColor textDisabledColor = null, UIFont textDisabledFont = null,
																   UIColor textFocusedColor = null, UIFont textFocusedFont = null,
																   UIColor floatLabelDisabledColor = null, UIFont floatLabelDisabledFont = null,
																   UIColor floatLabelFocusedColor = null, UIFont floatLabelFocusedFont = null,
																   UIColor placeholderColor = null, UIFont placeholderFont = null
																  )
					=> CreateTextField(ActivationMode.OnFocus,
									   textColor, textFont,
									   placeholder,
									   cursorColor,
									   floatLabelColor, floatLabelFont,
									   textDisabledColor, textDisabledFont,
									   textFocusedColor, textFocusedFont,
									   floatLabelDisabledColor, floatLabelDisabledFont,
									   floatLabelFocusedColor, floatLabelFocusedFont,
									   placeholderColor, placeholderFont);

		public static FloatLabeledTextField CreateOnFirstCharTextField(UIColor textColor, UIFont textFont,
																	   string placeholder,
																	   UIColor cursorColor,
																	   UIColor floatLabelColor, UIFont floatLabelFont,
																	   UIColor textDisabledColor = null, UIFont textDisabledFont = null,
																	   UIColor textFocusedColor = null, UIFont textFocusedFont = null,
																	   UIColor floatLabelDisabledColor = null, UIFont floatLabelDisabledFont = null,
																	   UIColor floatLabelFocusedColor = null, UIFont floatLabelFocusedFont = null, UIColor placeholderColor = null, UIFont placeholderFont = null
																	  )
					=> CreateTextField(ActivationMode.OnFirstCharacter,
									   textColor, textFont,
									   placeholder,
									   cursorColor,
									   floatLabelColor, floatLabelFont,
									   textDisabledColor, textDisabledFont,
									   textFocusedColor, textFocusedFont,
									   floatLabelDisabledColor, floatLabelDisabledFont,
									   floatLabelFocusedColor, floatLabelFocusedFont,
									   placeholderColor, placeholderFont);

		private static FloatLabeledTextField CreateTextField(ActivationMode mode,
															 UIColor textColor, UIFont textFont,
															 string placeholder,
															 UIColor cursorColor,
															 UIColor floatLabelColor, UIFont floatLabelFont,
															 UIColor textDisabledColor = null, UIFont textDisabledFont = null,
															 UIColor textFocusedColor = null, UIFont textFocusedFont = null,
															 UIColor floatLabelDisabledColor = null, UIFont floatLabelDisabledFont = null,
															 UIColor floatLabelFocusedColor = null, UIFont floatLabelFocusedFont = null,
															 UIColor placeholderColor = null, UIFont placeholderFont = null
															)
					=> new FloatLabeledTextField
					{
						Mode = mode,
						TintColor = cursorColor,
						TextNormalColor = textColor,
						TextNormalFont = textFont,
						FloatingLabelNormalTextColor = floatLabelColor,
						FloatingLabelNormalFont = floatLabelFont,

						TextDisabledColor = textDisabledColor ?? textColor,
						TextDisabledFont = textDisabledFont ?? textFont,
						TextFocusedColor = textFocusedColor ?? textColor,
						TextFocusedFont = textFocusedFont ?? textFont,

						FloatingLabelDisabledFont = floatLabelDisabledFont ?? floatLabelFont,
						FloatingLabelDisabledTextColor = floatLabelDisabledColor ?? floatLabelColor,
						FloatingLabelFocusedFont = floatLabelFocusedFont ?? floatLabelFont,
						FloatingLabelFocusedTextColor = floatLabelFocusedColor ?? floatLabelColor,

						PlaceholderFont = placeholderFont ?? floatLabelFont,
						PlaceholderColor = placeholderColor ?? floatLabelColor,
						Placeholder = placeholder
					};

		#endregion
	}
}