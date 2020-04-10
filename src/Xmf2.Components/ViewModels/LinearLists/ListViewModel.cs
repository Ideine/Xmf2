using System;
using System.Collections.Generic;
using System.Linq;
using Xmf2.Components.Interfaces;
using Xmf2.Components.ViewModels;

namespace Xmf2.Core.LinearLists
{
	public interface IListViewModel
	{
		void SetItemSource(IEnumerable<IEntity> itemModelList);
	}

	public abstract class ListViewModel<TItem, TCellViewModel> : BaseComponentViewModel, IListViewModel
		where TCellViewModel : BaseComponentViewModel
		where TItem : IEntity
	{
		protected List<TCellViewModel> _viewModels;
		protected IReadOnlyList<TCellViewModel> ViewModels => _viewModels;

		protected abstract TCellViewModel Factory(TItem item);

		protected ListViewModel(IServiceLocator services) : base(services)
		{
			_viewModels = new List<TCellViewModel>();
		}

		public virtual void SetItemSource(IEnumerable<TItem> itemModelList)
		{
			_viewModels.ForEach(x => x.Dispose());
			_viewModels.Clear();

			if (itemModelList == null)
			{
				return;
			}

			_viewModels.AddRange(itemModelList.Select(itemModel =>
			{
				TCellViewModel viewModel = Factory(itemModel);
				return viewModel;
			}));
		}

		protected override IViewState NewState()
		{
			return new ListViewState(_viewModels.ConvertAll(x =>
			{
				IViewState state = x.ViewState();
				if (state is IEntityViewState result)
				{
					return result;
				}
				throw new InvalidOperationException($"Child state of ListViewModel must implement IEntityViewState, got {state.GetType()}");
			}));
		}

		void IListViewModel.SetItemSource(IEnumerable<IEntity> itemModelList)
		{
			SetItemSource((IEnumerable<TItem>)itemModelList);
		}
	}
}
