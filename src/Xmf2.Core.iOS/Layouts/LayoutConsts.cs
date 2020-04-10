namespace Xmf2.Core.iOS.Layouts
{
	/// <summary>
	/// Ensemble de constantes garanties par Apple.
	/// </summary>
	public class LayoutConsts
	{
		//See : http://ivomynttinen.com/blog/ios-design-guidelines#tab-bar
		/// <summary>
		/// <see cref="UIKit.UITabBar/>.Frame.Height
		/// </summary>
		public const float UITabBar_DefaultHeight = 49;
		/// <summary>
		/// <see cref="UIKit.UINavigationBar"/>.Frame.Height in most situations. Height of standard NavigationBar may differ.
		/// See https://ivomynttinen.com/blog/ios-design-guidelines#nav-bar for more details.
		/// </summary>
		public const float UINavBar_DefaultHeight = 44;
		/// <summary>
		/// Epaisseur utilis閑 pour un s閜arateur visuel entre champs.
		/// </summary>
		public const float UISeparator_Height = 1;

		/// <summary>
		/// Epaisseur de la StatusBar (en haut de l'閏ran) selon le standard d'Apple.
		/// </summary>
		public const float UIStatusBar_DefaultHeight = 20;

		public const float UIStatusBar_WithNotchHeight= 44;

		public const float NOTCH_EXTRA_SPACE = 12f;

	}
}
