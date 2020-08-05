using Android.Text;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewSpannedTextTargetBinding : MvxAndroidTargetBinding<TextView, ISpanned>
	{
		public TextViewSpannedTextTargetBinding(TextView view) : base(view) { }

		protected override void SetValueImpl(TextView target, ISpanned value)
		{
			target?.SetText(value, TextView.BufferType.Spannable);
		}
	}
}