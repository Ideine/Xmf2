using System;

namespace Xmf2.Components.Events
{
	public interface IEventBus
	{
		IDisposable Subscribe<TEvent>(Action callback) where TEvent : IEvent;
		IDisposable Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent;
		void Publish<TEvent>(TEvent evt) where TEvent : IEvent;
	}
	public interface IGlobalEventBus : IEventBus { }
}