using System;
using Windows.UI.Core;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.UWP
{
    public class WindowUIDispatcher : IUIDispatcher
    {
        private CoreDispatcher _dispatcher => Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

        public void OnMainThread(Action action)
        {
            _dispatcher?.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke());
        }
    }
}
