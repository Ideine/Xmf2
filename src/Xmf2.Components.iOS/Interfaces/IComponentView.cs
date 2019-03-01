using System;
using UIKit;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.iOS.Interfaces
{
	public interface IComponentView : IDisposable
	{
		UIView View { get; }

		void SetState(IViewState state);
	}
}