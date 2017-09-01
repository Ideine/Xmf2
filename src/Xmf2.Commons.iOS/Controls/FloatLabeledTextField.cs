using System;
using UIKit;
using CoreGraphics;
using Foundation;
using System.ComponentModel;

namespace Xmf2.Commons.iOS.Controls
{
    [Register("FloatLabeledTextField"), DesignTimeVisible(true)]
    public class FloatLabeledTextField : UITextField
    {
        private UILabel _floatingLabel;

        public event EventHandler TextCleared;

        public UIColor FloatingLabelTextColor { get; set; }
        public UIColor FloatingLabelActiveTextColor { get; set; }

        public UIFont FloatingLabelFont
        {
            get { return _floatingLabel.Font; }
            set { _floatingLabel.Font = value; }
        }

        public FloatLabeledTextField(IntPtr p) : base(p)
		{
			this.InitFloatingLabel();
		}

        public FloatLabeledTextField() : base()
		{
			this.InitFloatingLabel();
		}

        public FloatLabeledTextField(CGRect frame)
            : base(frame)
        {
			this.InitFloatingLabel();
        }

		private void InitFloatingLabel()
		{
			_floatingLabel = new UILabel()
			{
				Alpha = 0.0f
			};

			AddSubview(_floatingLabel);
			FloatingLabelTextColor = UIColor.Gray;
			FloatingLabelActiveTextColor = UIColor.Blue;
			FloatingLabelFont = UIFont.BoldSystemFontOfSize(12);
		}

        public override string Placeholder
        {
            get { return base.Placeholder; }
            set
            {
				//TODO: Contournement pour ne pas avoir une largeur suffisante évitant les points de suspension. Voir pourquoi on a une mauvais largeur de calculée...
				//...plutôt que d'utiliser ce contournement.
				const float SECURITY_MARGIN = 10;
				base.Placeholder	= value.ToUpper();
                _floatingLabel.Text		 = value.ToUpper();
                _floatingLabel.TextColor = UIColor.White;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame =
                    new CGRect(x:0,
							   y:		_floatingLabel.Font.LineHeight,
							   width:	_floatingLabel.Frame.Size.Width + SECURITY_MARGIN,
							   height:	_floatingLabel.Frame.Size.Height);
            }
        }

		/// <summary>
		/// This method return the computed rectangle for drawing the text.
		/// </summary>
		public override CGRect TextRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
                return base.TextRect(forBounds);

			var result = InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
			return result;
		}

		/// <summary>
		/// Returns the rectangle to display the editable text.
		/// </summary>
		public override CGRect EditingRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
                return base.EditingRect(forBounds);

            var result = InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
			return result;
        }

        public override CGRect ClearButtonRect(CGRect forBounds)
        {
            var rect = base.ClearButtonRect(forBounds);

            if (_floatingLabel == null)
                return rect;

            return new CGRect(
                rect.X, rect.Y + _floatingLabel.Font.LineHeight / 2.0f,
                rect.Size.Width, rect.Size.Height);
        }

		private bool IsPlaceholderShouldFloat()
		{
			return !string.IsNullOrEmpty(Text);
		}

		private bool _isFloating = true;

		private void HandleChange()
		{
            Action updateLabel = () =>
            {
                if (IsPlaceholderShouldFloat())
                {
					_floatingLabel.Alpha = 1.0f;
					_floatingLabel.TextColor = FloatingLabelTextColor;
					_floatingLabel.Frame =
                        new CGRect(
                            x:		_floatingLabel.Frame.Location.X,
                            y:		2.0f,
                            width:	_floatingLabel.Frame.Size.Width,
                            height:	_floatingLabel.Frame.Size.Height);
					_isFloating = true;
				}
                else
                {
                    _floatingLabel.Alpha = 0.0f;
					_floatingLabel.TextColor = FloatingLabelTextColor;
					_floatingLabel.Frame =
                        new CGRect(
                            x:		_floatingLabel.Frame.Location.X,
                            y:		_floatingLabel.Font.LineHeight,
                            width:	_floatingLabel.Frame.Size.Width,
							height: _floatingLabel.Frame.Size.Height);
					_isFloating = false;
				}
            };

            if (IsFirstResponder)
            {
                _floatingLabel.TextColor = FloatingLabelActiveTextColor;
				if (IsPlaceholderShouldFloat() == _isFloating)
                {
                    updateLabel();
                }
                else
                {
                    UIView.Animate(
                        0.3f, 0.0f,
                        UIViewAnimationOptions.BeginFromCurrentState
                        | UIViewAnimationOptions.CurveEaseOut,
                        () => updateLabel(),
                        () => { });
                }
            }
            else
            {
                _floatingLabel.TextColor = FloatingLabelTextColor;
                updateLabel();
            }
		}

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
			this.HandleChange();
        }


		public void ClearText()
        {
            Text = string.Empty;
            TextCleared?.Invoke(this, EventArgs.Empty);
        }

        private static CGRect InsetRect(CGRect rect, UIEdgeInsets insets)
        {
            return new CGRect(
                rect.X + insets.Left,
                rect.Y + insets.Top,
                rect.Width - insets.Left - insets.Right,
                rect.Height - insets.Top - insets.Bottom);
        }
    }
}