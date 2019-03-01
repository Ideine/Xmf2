using FFImageLoading.Transformations;
using FFImageLoading.Work;
using UIKit;

namespace Xmf2.Core.ImgLoading.iOS.Transformations
{
	public class GrayOverlayTransformation : TransformationBase
	{
		public override string Key => nameof(GrayOverlayTransformation);
		protected override UIImage Transform(UIImage sourceBitmap, string path, ImageSource source, bool isPlaceholder, string key)
		{
			//TODO: find how to add a gray overlay transformation
			return sourceBitmap;
		}
	}
}
