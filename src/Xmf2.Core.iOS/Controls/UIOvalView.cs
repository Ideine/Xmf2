using Foundation;
using ObjCRuntime;
using UIKit;
using Xmf2.Core.iOS.Controls.Layers;

namespace Xmf2.Core.iOS.Controls
{
	public class UIOvalView : UIView
	{
		/// <remarks>
		/// Use of layerClass could be replaced by overriding draw method.
		/// See https://stackoverflow.com/a/42133959/1584823
		/// </remarks>
		[Export("layerClass")]
		public static Class GetLayerClass()
		{
			return new Class(typeof(OvalLayer));
		}

		public new OvalLayer Layer => (OvalLayer)base.Layer;
	}
}
