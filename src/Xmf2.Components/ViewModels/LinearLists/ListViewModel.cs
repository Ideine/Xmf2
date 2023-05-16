using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Exceptions;

namespace Xmf2.Components.ViewModels.LinearLists
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

		public virtual void SetItemSource(IReadOnlyList<TItem> itemModelList)
		{
			TCellViewModel[] viewModelArray = ViewModels.ToArray();
			_viewModels.Clear();
			Task.Run(async () =>
			{
				await Task.Delay(10000);
				foreach (TCellViewModel viewmodel in viewModelArray)
				{
					viewmodel.Dispose();
				}
			});

			if (itemModelList == null)
			{
				return;
			}

			int finalCount = _viewModels.Count + itemModelList.Count;

			_viewModels.AddRange(itemModelList.Select((itemModel, index) =>
			{
				TCellViewModel viewModel = Factory(itemModel);
				AdditionalItemBinds(itemModel, viewModel, index, finalCount);
				return viewModel;
			}));
		}

		protected virtual void AdditionalItemBinds(TItem itemModel, TCellViewModel itemViewModel, int position, int count)
		{
			//May be overriden.
		}

		protected override IViewState NewState()
		{
			return new ListViewState(_viewModels.ToArray().Select(x =>
			{
				IViewState state = x.ViewState();
				if (state is IEntityViewState result)
				{
					return result;
				}

				throw new DeveloperException($"Child state of ListViewModel must implement IEntityViewState, got {state?.GetType()}");
			}).ToList());
		}

		void IListViewModel.SetItemSource(IEnumerable<IEntity> itemModelList)
		{
			SetItemSource(itemModelList.Cast<TItem>().ToList());
		}
	}
}