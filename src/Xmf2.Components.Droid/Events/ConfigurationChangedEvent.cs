using System;
using Android.Content.Res;
using Xmf2.Components.Events;

namespace Xmf2.Components.Droid.Events
{
	public class ConfigurationChangedEvent : IEvent
	{
		public Configuration NewConfig { get; }

		public ConfigurationChangedEvent(Configuration newConfig)
		{
			NewConfig = newConfig;
		}
	}
}
