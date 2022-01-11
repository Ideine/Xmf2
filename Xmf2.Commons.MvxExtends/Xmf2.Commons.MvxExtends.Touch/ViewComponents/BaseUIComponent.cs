using System;
using MvvmCross;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
    public abstract class BaseUIComponent<TViewModel> : UIView, IUIComponent<TViewModel>, IMvxBindable, IDisposable
													where TViewModel : BaseViewModel<object>
	{
		protected BaseUIComponent()
		{
			BindingContext = Mvx.Resolve<IMvxBindingContext>();
		}

		public virtual void AutoLayout()
		{
			
		}

		public virtual void Bind()
		{

		}

		public virtual void ViewDidAppear()
		{

		}

		public virtual void ViewDidLoad()
		{

		}

		public IMvxBindingContext BindingContext { get; set; }

		[MvxSetToNullAfterBinding]
		public object DataContext
		{
			get { return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}

		public TViewModel ViewModel
		{
			get { return (TViewModel)DataContext; }
			set { DataContext = value; }
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BindingContext.ClearAllBindings();
			}
			base.Dispose(disposing);
		}
	}
	
}
