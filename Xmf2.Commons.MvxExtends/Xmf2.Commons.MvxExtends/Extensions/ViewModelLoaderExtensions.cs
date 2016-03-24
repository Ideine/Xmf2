using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Platform;


namespace Xmf2.Commons.MvxExtends.Extensions
{
    public static class ViewModelLoaderExtensions
    {
        public static T CreateVM<T>(this IMvxViewModelLoader vmLoader, object parameterValues = null)
        {
            var request = new MvxViewModelRequest()
            {
                ViewModelType = typeof(T),
                ParameterValues = parameterValues.ToSimplePropertyDictionary()
            };

            return (T)vmLoader.LoadViewModel(request, new MvxBundle());
        }
    }
}
