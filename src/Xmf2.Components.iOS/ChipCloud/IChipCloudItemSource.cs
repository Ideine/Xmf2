using Xmf2.Components.iOS.ChipCloud.Cells;

namespace Xmf2.Components.iOS.ChipCloud
{
	public interface IChipCloudItemSource
	{
		int Count { get; }

		ChipCloudItemCell GetCell(ChipCloudView cloudView, int position);
	}
}
