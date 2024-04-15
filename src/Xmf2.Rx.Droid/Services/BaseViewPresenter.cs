using System;
using System.Diagnostics;
using Android.Content;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Splat;
using Xmf2.Commons.Droid.Services;

namespace Xmf2.Rx.Droid.Services
{
	public class BaseViewPresenter
	{
		private readonly Lazy<ICurrentActivity> _currentActivity;

		protected Android.App.Activity CurrentActivity => _currentActivity.Value.Activity;

		public BaseViewPresenter()
		{
			_currentActivity = new Lazy<ICurrentActivity>(Locator.Current.GetService<ICurrentActivity>());
		}

		protected virtual void ShowView(Type viewType, bool clearHistory = false)
		{
			var intent = CreateIntentForViewType(viewType, clearHistory);
			if (intent != null)
			{
				Show(intent);
			}
		}

		protected virtual Intent CreateIntentForViewType(Type viewType, bool clearHistory = false)
		{
			var activity = CurrentActivity;
			if (activity == null)
			{
				Debug.WriteLine("Cannot Resolve current activity");
				return null;
			}
			var intent = new Intent(activity, viewType);
			if (clearHistory)
			{
				intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
			}
			return intent;
		}

		protected virtual void Show(Intent intent)
		{
			var activity = CurrentActivity;
			if (activity == null)
			{
				Debug.WriteLine("Cannot Resolve current activity");
				return;
			}
			activity.StartActivity(intent);
		}

		protected virtual void ShowView(DialogFragment view, string tag)
		{
			if (CurrentActivity is AppCompatActivity appCompatActivity && !appCompatActivity.IsFinishing)
			{
				var fm = appCompatActivity.SupportFragmentManager;

				var dialogView = fm.FindFragmentByTag(tag);

				if (dialogView != null)
				{
					fm.BeginTransaction().Remove(dialogView).CommitAllowingStateLoss();
					fm.ExecutePendingTransactions();
				}
				view.Show(fm, tag);
			}
		}

		public virtual void Close()
		{
			if (CurrentActivity == null)
			{
				Debug.WriteLine("Ignoring close for viewmodel - current page is not the view for the requested viewmodel");
				return;
			}
			CurrentActivity.Finish();
		}
	}
}
