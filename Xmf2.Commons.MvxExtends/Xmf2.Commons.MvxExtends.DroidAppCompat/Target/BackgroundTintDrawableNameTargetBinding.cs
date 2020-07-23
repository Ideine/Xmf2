using Android.Content.Res;
using Android.Views;
using Android.Support.V4.View;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Target
{
	public class BackgroundTintDrawableNameTargetBinding : MvxAndroidTargetBinding<View, string>
	{
		public BackgroundTintDrawableNameTargetBinding(View view) : base(view) { }

		protected override void SetValueImpl(View target, string value)
		{
			if (value == null)
			{
				return;
			}

			Resources resources = AndroidGlobals.ApplicationContext.Resources;
			int id = resources.GetIdentifier(value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
			if (id == 0)
			{
				System.Diagnostics.Debug.WriteLine($"Value '{value}' was not a known drawable name");
				return;
			}

			ColorStateList colorList = AndroidGlobals.ApplicationContext.GetColorStateList(id);

			if (target is ITintableBackgroundView tintableTarget)
			{
				tintableTarget.SupportBackgroundTintList = colorList;
			}
			else
			{
				target.BackgroundTintList = colorList;
			}
		}
	}
}