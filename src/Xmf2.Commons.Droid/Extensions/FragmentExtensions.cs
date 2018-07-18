using System;
using Android.Support.V4.App;
using Android.Support.V7.App;

public static class FragmentExtensions
{
	public static FragmentManager GetSupportFragmentManager(this Android.App.Activity activity)
	{
		return activity is AppCompatActivity appCompatActivity ? appCompatActivity.SupportFragmentManager : null;
	}

	public static Fragment GetTopFragment(this FragmentManager fm)
	{
		if (fm.Fragments.Count <= 0)
		{
			return null;
		}
		else
		{
			return fm.Fragments[fm.Fragments.Count - 1];
		}
	}

	public static TFragmentType FindFragmentByTag<TFragmentType>(this FragmentManager fm, string tag)
		where TFragmentType : Fragment
	{
		return fm.FindFragmentByTag(tag) as TFragmentType;
	}

	public static void ShowFragment(this AppCompatActivity activity, Fragment fragment, int container, bool addToBackStack = false)
	{
		ProcessTransaction(activity.SupportFragmentManager, fragment, (transaction) =>
		{
			transaction?.Add(container, fragment).CommitAllowingStateLoss();
		}, addToBackStack);
	}

	public static void ReplaceFragment(this AppCompatActivity activity, Fragment fragment, int container, bool addToBackStack = false)
	{
		ProcessTransaction(activity.SupportFragmentManager, fragment, (transaction) =>
	   {
		   transaction?.Replace(container, fragment).CommitAllowingStateLoss();
	   }, addToBackStack);
	}

	public static void ReplaceFragment(this Fragment activity, Fragment fragment, int container, bool addToBackStack = false)
	{
		ProcessTransaction(activity.ChildFragmentManager, fragment, (transaction) =>
		{
			transaction?.Replace(container, fragment).CommitAllowingStateLoss();
		}, addToBackStack);
	}

	public static void ProcessTransaction(FragmentManager fragmentManager, Fragment fragment, Action<FragmentTransaction> exectuteTrancation, bool addToBackStack = false)
	{
		using (var transaction = fragmentManager.BeginTransaction())
		{
			if (addToBackStack)
			{
				transaction?.AddToBackStack(fragment.GetType().Name);
			}
			exectuteTrancation?.Invoke(transaction);
			fragmentManager.ExecutePendingTransactions();
		}
	}
}