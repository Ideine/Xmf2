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
                base.Placeholder = value;

                _floatingLabel.Text = value.ToUpper(); ;
                _floatingLabel.TextColor = UIColor.White;
                _floatingLabel.SizeToFit();
                _floatingLabel.Frame =
                    new CGRect(
                        0, _floatingLabel.Font.LineHeight,
                        _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
            }
        }

        public override CGRect TextRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
                return base.TextRect(forBounds);

            return InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
        }

        public override CGRect EditingRect(CGRect forBounds)
        {
            if (_floatingLabel == null)
                return base.EditingRect(forBounds);

            return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
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

		private void HandleChange()
		{
            Action updateLabel = () =>
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    _floatingLabel.Alpha = 1.0f;
					_floatingLabel.TextColor = FloatingLabelTextColor;
					_floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            2.0f,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
                else
                {
                    _floatingLabel.Alpha = 0.0f;
					_floatingLabel.TextColor = FloatingLabelTextColor;
					_floatingLabel.Frame =
                        new CGRect(
                            _floatingLabel.Frame.Location.X,
                            _floatingLabel.Font.LineHeight,
                            _floatingLabel.Frame.Size.Width,
                            _floatingLabel.Frame.Size.Height);
                }
            };

            if (IsFirstResponder)
            {
                _floatingLabel.TextColor = FloatingLabelActiveTextColor;
                var shouldFloat = !string.IsNullOrEmpty(Text);
                var isFloating = _floatingLabel.Alpha == 1f;
                if (shouldFloat == isFloating)
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