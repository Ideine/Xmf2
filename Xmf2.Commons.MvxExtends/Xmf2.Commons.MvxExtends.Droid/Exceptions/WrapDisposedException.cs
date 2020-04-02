using System;
using Android.OS;

namespace Android
{
	public static class WrapDisposedException
	{
		public static void WrapForDisposedException(this object _, Action action, bool log = true)
		{
#if DEBUG
			if (!IsInUiThread())
			{
				throw new Exception($"Developper misuse of {nameof(WrapForDisposedException)}");
			}
#endif
			try
			{
				action();
			}
			catch (ObjectDisposedException e)
			{
				if (log)
				{
					System.Diagnostics.Debug.WriteLine(e);
				}
			}
			catch (NullReferenceException nre)
			{
				if (log)
				{
					System.Diagnostics.Debug.WriteLine(nre);
				}
			}
		}

		public static bool IsInUiThread()
		{
			return Looper.MyLooper() == Looper.MainLooper;
		}
	}
}