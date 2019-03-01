using System;

namespace Xmf2.Components.Interfaces
{
	public interface IEntityViewState : IViewState
	{
		Guid Id { get; }
	}
}