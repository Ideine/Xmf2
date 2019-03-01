using Xmf2.Components.Events;

namespace Xmf2.Core.Droid.Helpers
{
	public class CollapseLevelEvent : IEvent
	{
		public float Level { get; }
		public bool WithBackground { get; }

		public CollapseLevelEvent(float level, bool withBackground)
		{
			Level = level;
			WithBackground = withBackground;
		}
	}
}