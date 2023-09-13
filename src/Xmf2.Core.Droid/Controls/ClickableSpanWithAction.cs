using System;
using Android.Graphics;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Xmf2.Core.Droid.Helpers;

namespace Xmf2.Core.Droid.Controls
{
	public class ClickableSpanWithAction : ClickableSpan
	{
		private Action<View> _onClickAction;
		private readonly Android.Graphics.Color _textColor;
		private readonly bool _withUnderline;
		private readonly bool _isBold;

		protected ClickableSpanWithAction(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ClickableSpanWithAction(Action<View> onClickAction, int textColor, bool withUnderline = true, bool isBold = false) : this(onClickAction, textColor.ColorFromHex(), withUnderline, isBold) { }
		public ClickableSpanWithAction(Action<View> onClickAction, uint textColor, bool withUnderline = true, bool isBold = false) : this(onClickAction, textColor.ColorFromHex(), withUnderline, isBold) { }

		public ClickableSpanWithAction(Action<View> onClickAction, Android.Graphics.Color textColor, bool withUnderline = true, bool isBold = false)
		{
			_onClickAction = onClickAction;
			_textColor = textColor;
			_withUnderline = withUnderline;
			_isBold = isBold;
		}

		public override void OnClick(View widget) => _onClickAction?.Invoke(widget);

		public override void UpdateDrawState(TextPaint ds)
		{
			base.UpdateDrawState(ds);
			ds.Color = _textColor;
			ds.UnderlineText = _withUnderline;
			if(_isBold)
			{
				ds.SetTypeface(Typeface.DefaultBold);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_onClickAction = null;
			}

			base.Dispose(disposing);
		}
	}
}