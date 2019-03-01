using System;

namespace Xmf2.Components.Interfaces
{
	public interface IBusy
	{
		bool IsEnabled { get; }

		void Inc();

		void Dec();

		IDisposable EnableDisposable();
	}
}