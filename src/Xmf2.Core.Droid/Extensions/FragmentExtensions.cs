#if __ANDROID_29__
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
#else
using Android.Support.V4.App;
using Android.Support.V7.App;
#endif

// ReSharper disable once CheckNamespace
public static class FragmentExtensions
{
	public static FragmentManager GetSupportFragmentManager(this Android.App.Activity activity)
	{
		return activity is AppCompatActivity appCompatActivity ? appCompatActivity.SupportFragmentManager : null;
	}

	public static Fragment GetTopFragment(this FragmentManager fm)
	{
		return fm.Fragments.Count <= 0 ? null : fm.Fragments[^1];
	}

	public static TFragmentType FindFragmentByTag<TFragmentType>(this FragmentManager fm, string tag)
		where TFragmentType : Fragment
	{
		return fm.FindFragmentByTag(tag) as TFragmentType;
	}

	public static void ShowFragment(this AppCompatActivity activity, Fragment fragment, int container, bool addToBackStack = false)
	{
		var transaction = activity.SupportFragmentManager.BeginTransaction();

		if (addToBackStack)
		{
			transaction.AddToBackStack(fragment.GetType().Name);
		}

		transaction?.Replace(container, fragment).CommitAllowingStateLoss();
		activity.SupportFragmentManager.ExecutePendingTransactions();
	}
}