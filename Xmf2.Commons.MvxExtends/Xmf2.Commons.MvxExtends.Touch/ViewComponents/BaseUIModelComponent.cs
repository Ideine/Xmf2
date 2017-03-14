using System;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{

	public abstract class BaseUIModelComponent<TModel> : UIView, IUIModelComponent<TModel>, IMvxBindable, IDisposable
													where TModel : class
	{
		protected BaseUIModelComponent()
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

		public TModel Model
		{
			get { return (TModel)DataContext; }
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
