using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Rx.Extensions;

namespace Xmf2.Droid.Rx.ViewHandlers
{
	public class CoupleViewSelectedStateHandler : IDisposable
	{
		private IDisposable _currentDisposable;

		private View _viewForYes;
		private View _viewForNo;

		private Subject<bool?> _selected = new();
		public IObservable<bool?> Selected { get; }

		public bool? CurrentSelected { get; private set; }

		public CoupleViewSelectedStateHandler(View viewForYes, View viewForNo, bool? selected = null, bool enabled = true)
		{
			_viewForYes = viewForYes;
			_viewForNo = viewForNo;

			Selected = _selected.StartWith(selected).ToObservableForBinding();

			_currentDisposable = this.WhenAnyObservable(x => x.Selected)
				.ObserveOn(RxApp.TaskpoolScheduler)
				.Subscribe(x => CurrentSelected = x);

			UpdateUI(selected);

			if (enabled)
			{
				Initialize();
			}
		}


		public void SetEnabled(bool enabled)
		{
			_viewForYes.Enabled = _viewForNo.Enabled = enabled;
		}

		public void SetSelected(bool? selected)
		{
			_selected.OnNext(selected);
			UpdateUI(selected);
		}

		private void Initialize()
		{
			_viewForYes.Click += HandleYesClick;
			_viewForNo.Click += HandleNoClick;
		}

		private void HandleYesClick(object sender, EventArgs e)
		{
			var newViewForYesSelectedState = !_viewForYes.Selected;
			if (newViewForYesSelectedState)
			{
				_selected.OnNext(true);
				UpdateUI(true);
			}
		}

		private void HandleNoClick(object sender, EventArgs e)
		{
			var newViewForNoSelectedState = !_viewForNo.Selected;
			if (newViewForNoSelectedState)
			{
				_selected.OnNext(false);
				UpdateUI(false);
			}
		}

		private void UpdateUI(bool? selected)
		{
			if (selected.HasValue)
			{
				_viewForYes.Selected = selected.Value;
				_viewForNo.Selected = !_viewForYes.Selected;
			}
			else
			{
				_viewForYes.Selected = false;
				_viewForNo.Selected = false;
			}
		}

		public void Dispose()
		{
			if (_viewForYes != null)
			{
				_viewForYes.Click -= HandleYesClick;
				_viewForYes.Dispose();
				_viewForYes = null;
			}

			if (_viewForNo != null)
			{
				_viewForNo.Click -= HandleNoClick;
				_viewForNo.Dispose();
				_viewForNo = null;
			}

			_currentDisposable?.Dispose();
			_currentDisposable = null;

			_selected?.Dispose();
			_selected = null;
		}
	}
}