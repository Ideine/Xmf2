using Android.Widget;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ImageViewExtensions
	{
		public static string WithAspect(this string url, ImageView.ScaleType scaleType)
		{
			string parameterChar = url.Contains('?') ? "&" : "?";

			if (scaleType == ImageView.ScaleType.FitXy || scaleType == ImageView.ScaleType.CenterCrop)
			{
				return url + parameterChar + "aspect=Fill";
			}
			else if (scaleType == ImageView.ScaleType.FitCenter
					 || scaleType == ImageView.ScaleType.Center
					 || scaleType == ImageView.ScaleType.CenterInside
					 || scaleType == ImageView.ScaleType.FitEnd
					 || scaleType == ImageView.ScaleType.FitStart
					 || scaleType == ImageView.ScaleType.FitEnd)
			{
				return url + parameterChar + "aspect=Fit";
			}

			return url + parameterChar + "aspect=Undefined";
		}
	}
}

