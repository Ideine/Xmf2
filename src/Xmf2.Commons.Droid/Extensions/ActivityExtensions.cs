using System;
using Android.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace Xmf2.Commons.Droid.Extensions
{
	public static class ActivityExtensions
	{
		public static void SetSupportActionBar(this Activity activity, Toolbar toolbar)
		{
			if (activity is AppCompatActivity appCompatActivity)
			{
				appCompatActivity.SetSupportActionBar(toolbar);
			}
		}

		public static void SetActionBarTitle(this Activity activity, string title)
		{
			if (activity is AppCompatActivity appCompatActivity)
			{
				appCompatActivity.SupportActionBar.Title = title;
			}
		}

		public static void ShowNavigationBackButton(this Activity activity, bool show)
		{
			if (activity != null && !activity.IsFinishing && activity is AppCompatActivity appCompatActivity)
			{
				appCompatActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(show);
			}
		}
	}
}
