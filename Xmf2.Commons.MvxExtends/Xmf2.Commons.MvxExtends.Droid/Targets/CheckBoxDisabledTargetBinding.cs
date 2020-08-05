using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class CheckBoxDisabledTargetBinding : MvxAndroidTargetBinding<CheckBox, bool>
	{
		public CheckBoxDisabledTargetBinding(CheckBox view) : base(view) { }

		protected override void SetValueImpl(CheckBox target, bool value)
		{
			target.Enabled = !value;
		}
	}
}