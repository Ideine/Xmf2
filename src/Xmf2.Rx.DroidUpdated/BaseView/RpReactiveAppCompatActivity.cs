using System;
using Android.Runtime;
using ReactiveUI;
using ReactiveUI.AndroidX;

namespace Xmf2.Rx.DroidUpdated.BaseView
{
	public class RpReactiveAppCompatActivity<TViewModel> : ReactiveAppCompatActivity, IViewFor<TViewModel>, ICanActivate
        where TViewModel : class
    {
        private TViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="RpReactiveAppCompatActivity{TViewModel}"/> class.
        /// </summary>
        protected RpReactiveAppCompatActivity() { }

        protected RpReactiveAppCompatActivity(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

        /// <inheritdoc/>
        public TViewModel ViewModel
        {
            get => _viewModel;
            set => this.RaiseAndSetIfChanged(ref _viewModel, value);
        }

        /// <inheritdoc/>
        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => _viewModel = (TViewModel)value;
        }
    }
}