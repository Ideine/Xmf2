using System;
using Android.Views;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.Interfaces
{
	public interface IComponentView : IDisposable
	{
		View View(ViewGroup parent);

		void SetState(IViewState state);
	}
}