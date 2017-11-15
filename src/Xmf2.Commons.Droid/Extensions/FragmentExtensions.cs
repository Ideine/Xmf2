using System;
using Android.Support.V7.App;

public static class FragmentExtensions
{
	public static Android.Support.V4.App.FragmentManager GetSupportFragmentManager(this Android.App.Activity activity)
	{
		return activity is AppCompatActivity appCompatActivity ? appCompatActivity.SupportFragmentManager : null;
	}

	public static TFragmentType FindFragmentByTag<TFragmentType>(this Android.Support.V4.App.FragmentManager fm, string tag)
		where TFragmentType : Android.Support.V4.App.Fragment
	{
		return fm.FindFragmentByTag(tag) as TFragmentType;
	}

	public static void ShowFragment(this AppCompatActivity activity, Android.Support.V4.App.Fragment fragment, int container, bool addToBackStack = false)
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