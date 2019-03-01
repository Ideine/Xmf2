using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Xmf2.Core.Droid.Parallax;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.List
{
	public class ParallaxRecyclerViewHelper : RecyclerView.OnScrollListener
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private float _parallaxFactor;
		private View _view;
		private ParallaxedView _parallaxedView;

		public ParallaxRecyclerViewHelper(float parallaxFactor = 1.9F)
		{
			_parallaxFactor = parallaxFactor;
		}

		protected ParallaxRecyclerViewHelper(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public void AddParallaxView(View v)
		{
			_view = v;
			_parallaxedView = new ParallaxedView(_view).DisposeWith(_disposable);
		}

		private void HeaderParallax()
		{
			int top = -_view.Top;
			if (top >= 0)
			{
				_parallaxedView.SetOffset(top / _parallaxFactor);
				_parallaxedView.AnimateNow();
			}
		}

		public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled(recyclerView, dx, dy);
			HeaderParallax();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();
				_view = null;
			}

			base.Dispose(disposing);
		}
	}
}
