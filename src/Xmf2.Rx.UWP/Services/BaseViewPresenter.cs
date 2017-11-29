using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Xmf2.Rx.UWP.Services
{
    public class BaseViewPresenter
    {
        public Frame CurrentPage => Window.Current.Content as Frame;
       
        public BaseViewPresenter() { }

        public virtual void ShowView<IType>(object parameter = null)
        {
            ShowView(typeof(IType), parameter);
        }

        public virtual void ShowView(Type viewType, object parameter)
        {
            try
            {
                CurrentPage?.Navigate(viewType, parameter); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error seen during navigation request to {viewType.Name} - error {exception.ToString()}");
            }
        }
    }
}
