using System;
namespace Xmf2.Commons.Droid.ChipClouds
{
	public interface ChipCloudObserver
	{
		void Update(ChipCloudObserver observer, object data);
	}

	public interface ChipCloudObsevable
	{
		void DeleteObservers();

		void Subscribe(ChipCloudObserver observer);

	}
}
