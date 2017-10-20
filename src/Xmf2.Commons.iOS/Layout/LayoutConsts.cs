namespace Xmf2.Commons.iOS.Layout
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
		public const int UITabBar_DefaultHeight = 49;
		/// <summary>
		/// <see cref="UIKit.UINavigationBar"/>.Frame.Height in most situations. Height of standard NavigationBar may differ.
		/// See https://ivomynttinen.com/blog/ios-design-guidelines#nav-bar for more details.
		/// </summary>
		public const int UINavBar_DefaultHeight = 44;
		/// <summary>
		/// Epaisseur utilisée pour un séparateur visuel entre champs.
		/// </summary>
		public const int UISeparator_Height = 1;

		/// <summary>
		/// Epaisseur de la StatusBar (en haut de l'écran) selon le standard d'Apple.
		/// </summary>
		public const int UIStatusBar_DefaultHeight = 20;
	}
}