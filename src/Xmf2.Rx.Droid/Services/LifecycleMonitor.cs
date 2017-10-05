using System;
using System.Diagnostics;
using Android.App;
using Android.Content;

namespace Xmf2.Rx.Droid.Services
{
	public interface ICurrentActivity
	{
		Activity Activity { get; }
	}

	public interface ILifecycleMonitor
	{
		void OnCreate(Activity activity);

		void OnStart(Activity activity);

		void OnRestart(Activity activity);

		void OnResume(Activity activity);

		void OnPause(Activity activity);

		void OnStop(Activity activity);

		void OnDestroy(Activity activity);
	}

	public class LifecycleMonitor : ILifecycleMonitor, ICurrentActivity
	{
		public Activity Activity { get; private set; }

		public LifecycleMonitor(Context context)
		{
			Activity = context as Activity;
		}

		public void OnCreate(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnCreate {activity.GetType().Name}");
			Activity = activity;
		}

		public virtual void OnStart(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnStart {activity.GetType().Name}");
			Activity = activity;
		}

		public virtual void OnRestart(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnRestart {activity.GetType().Name}");
			Activity = activity;
		}

		public virtual void OnResume(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnResume {activity.GetType().Name}");
			Activity = activity;
		}

		public virtual void OnPause(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnPause {activity.GetType().Name}");
		}

		public virtual void OnStop(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnStop {activity.GetType().Name}");
		}

		public virtual void OnDestroy(Activity activity)
		{
			Debug.WriteLine($"[AcivityLifeCycle] OnDestroy {activity.GetType().Name}");

			if (this.Activity == activity)
			{
				Activity = null;
			}
		}
	}
}
