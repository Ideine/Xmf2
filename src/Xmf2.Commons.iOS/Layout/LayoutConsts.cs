namespace Xmf2.Commons.iOS.Layout
{
	/// <summary>
	/// Ensemble de constantes garanties par Apple.
	/// </summary>
	public static class LayoutConsts
	{
		//TODO: rajouter une méthode pour avoir le top d'un viewcontroller (iPhone X) et bottom
		
		//See : http://ivomynttinen.com/blog/ios-design-guidelines#tab-bar
		/// <summary>
		/// <see cref="UIKit.UITabBar/>.Frame.Height
		/// </summary>
		public const int UITabBar_DefaultHeight = 49;

		/// <summary>
		/// Epaisseur de la StatusBar (en haut de l'écran) selon le standard d'Apple.
		/// </summary>
		public const int UIStatusBar_DefaultHeight = 20;
	}
}