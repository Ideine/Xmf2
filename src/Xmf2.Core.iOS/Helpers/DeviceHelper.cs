using System.Linq;
using UIKit;

namespace Xmf2.Core.iOS.Helpers
{
	public class DeviceHelper
	{
		private const string IPHONE_8_CDMA		= "iPhone10,1";
		private const string IPHONE_8_GSM		= "iPhone10,4";
		private const string IPHONE_8_PLUS_CDMA = "iPhone10,2";
		private const string IPHONE_8_PLUS_GSM	= "iPhone10,5";

		private static readonly string[] _lookLikeIPhoneXNames = new string[]
		{
			IPHONE_8_CDMA, IPHONE_8_GSM, IPHONE_8_PLUS_CDMA, IPHONE_8_PLUS_GSM
		};
		private static bool? _haveVirtualButton;
		public static bool HaveVirtualButton()
		{
			if (!_haveVirtualButton.HasValue)
			{
				var currentDevice = UIDevice.CurrentDevice;
				//First iPhone with virtual home button released was 'iPhone X with iOS 11'...
				//...so we have to hanlde virtual button only if os >= 11.
				if (currentDevice.CheckSystemVersion(11, 0))
				{
					_haveVirtualButton = currentDevice.Model.StartsWith("iPhone10")
									  || !_lookLikeIPhoneXNames.Contains(currentDevice.Model);//We must exclude iPhone 8, they start with 'iPhone10' but it's not 'iPhone X'.
				}
				else
				{
					_haveVirtualButton = false;
				}
			}
			return _haveVirtualButton.Value;
		}
	}
}