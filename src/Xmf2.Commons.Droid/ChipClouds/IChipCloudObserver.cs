using System;
namespace Xmf2.Commons.Droid.ChipClouds
{
	public interface IChipCloudObserver
	{
		void Update(IChipCloudObserver observer, object data);
	}

	public interface IChipCloudObsevable
	{
		void DeleteObservers();

		void Subscribe(IChipCloudObserver observer);

	}
}
