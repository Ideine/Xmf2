using System;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;

namespace Xmf2.Core.Droid.Controls
{
	public class ClickableSpanWithAction : ClickableSpan
	{
		private Action<View> _onClickAction;
		private readonly Android.Graphics.Color _textColor;
		private readonly bool _withUnderline;

		protected ClickableSpanWithAction(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ClickableSpanWithAction(Action<View> onClickAction, Android.Graphics.Color textColor, bool withUnderline = true)
		{
			_onClickAction = onClickAction;
			_textColor = textColor;
			_withUnderline = withUnderline;
		}

		public override void OnClick(View widget) => _onClickAction?.Invoke(widget);

		public override void UpdateDrawState(TextPaint ds)
		{
			base.UpdateDrawState(ds);
			ds.Color = _textColor;
			ds.UnderlineText = _withUnderline;
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