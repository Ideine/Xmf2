using System;
using Foundation;
using UIKit;

namespace Xmf2.Commons.iOS.Extensions
{
    public static class ViewExtensions
    {
        public static UITextField TextField(this UISearchBar searchBar)
        {
            var textField = searchBar.ValueForKey(new NSString("_searchField")) as UITextField;
            return textField;
        }
    }
}
