using System.Diagnostics;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Target
{
	public class BackgroundTintDrawableNameTargetBinding : MvxAndroidTargetBinding<View, string>
	{
		public BackgroundTintDrawableNameTargetBinding(View view) : base(view) { }

		protected override void SetValueImpl(View target, string value)
		{
			Resources resources = AndroidGlobals.ApplicationContext.Resources;
			int id = resources.GetIdentifier(value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
			if (id == 0)
			{
				Debug.WriteLine("Value '{0}' was not a known drawable name", value);
				return;
			}

			ColorStateList colorList = AndroidGlobals.ApplicationContext.GetColorStateList(id);

			ITintableBackgroundView tintableBackgroundView = null;
			try
			{
				tintableBackgroundView = target.JavaCast<ITintableBackgroundView>();
			}
			catch { }

			if (tintableBackgroundView != null)
			{
				tintableBackgroundView.SupportBackgroundTintList = colorList;
			}
			else
			{
				((Button)target).BackgroundTintList = colorList;
			}
		}
	}
}