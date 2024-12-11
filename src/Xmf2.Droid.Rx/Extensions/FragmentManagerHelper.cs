using Android.App;
using AndroidX.AppCompat.App;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class FragmentManagerHelper
	{
		public static FragmentManager GetSupportFragmentManager(Activity activity)
		{
			return activity is AppCompatActivity appCompatActivity ? appCompatActivity.SupportFragmentManager : null;
		}

		public static TFragmentType FindFragmentByTag<TFragmentType>(FragmentManager fm, string tag)
			where TFragmentType : Fragment
		{
			return fm.FindFragmentByTag(tag) as TFragmentType;
		}
	}
}