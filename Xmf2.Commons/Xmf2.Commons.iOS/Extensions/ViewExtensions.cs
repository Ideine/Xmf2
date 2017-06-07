using System;
using Foundation;
namespace UIKit
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
