using System;
using Android.Widget;
using Android.Text;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewSpannedTextTargetBinding : MvxAndroidTargetBinding
	{
		public TextViewSpannedTextTargetBinding(TextView view) : base(view) { }

		public override Type TargetType => typeof(ISpanned);

		protected override void SetValueImpl(object target, object value)
		{
			if (value != null && !(value is ISpanned))
			{
				System.Diagnostics.Debug.WriteLine("Value '{0}' could not be parsed as a ISpanned", value);
			}

			TextView tvw = target as TextView;
			tvw?.SetText((ISpanned)value, TextView.BufferType.Spannable);
		}
	}
}