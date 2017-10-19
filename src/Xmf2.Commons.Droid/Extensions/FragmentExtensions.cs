using System;

public static class FragmentExtensions
{
	public static Android.Support.V4.App.FragmentManager GetSupportFragmentManager(this Android.App.Activity activity)
	{
		var appCompatActivity = activity as Android.Support.V7.App.AppCompatActivity;
		return appCompatActivity == null ? null : appCompatActivity.SupportFragmentManager;
	}

	public static FragmentType FindFragmentByTag<FragmentType>(this Android.Support.V4.App.FragmentManager fm, string tag)
		where FragmentType : Android.Support.V4.App.Fragment
	{
		return fm.FindFragmentByTag(tag) as FragmentType;
	}
}
