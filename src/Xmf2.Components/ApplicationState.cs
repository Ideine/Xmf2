using System;

namespace Xmf2.Components
{
	public static class ApplicationState
	{
		public static object Mutex { get; } = new object();

		public static event Action StateChanged;

		public static void RaiseStateChanged()
		{
			StateChanged?.Invoke();
		}
	}
}