using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;
using Object = Java.Lang.Object;

namespace Xmf2.Components.Droid.List
{
	public class CommonAdapter : RecyclerView.Adapter
	{
		private readonly Xmf2Disposable _disposable = new();

		private List<Type> _stateList;
		protected Dictionary<Type, Func<string, IComponentView>> ComponentCreatorMap { get; private set; }

		public const int CELL_TYPE = 0x2a;
		public const int HEADER_START_TYPE_INDEX = 1000;
		public const int FOOTER_START_TYPE_INDEX = 2000;

		protected List<IComponentView> HeaderList { get; private set; } = new();
		private Dictionary<IComponentView, int?> _headersHeight = new();
		private Dictionary<IComponentView, IViewState> _headerStates = new();

		private List<IComponentView> _footerList = new();
		private Dictionary<IComponentView, IViewState> _footerStates = new();

		private Dictionary<IComponentView, ParallaxRecyclerViewHelper> _parallaxDictionary = new();
		private Dictionary<IComponentView, View> _stickyDictionary = new();

		private IReadOnlyList<IEntityViewState> _itemSource;

		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				using var callback = new DiffList(_itemSource?.ToArray() ?? new IEntityViewState[0], value?.ToArray() ?? new IEntityViewState[0]);
				using DiffUtil.DiffResult result = DiffUtil.CalculateDiff(callback);
				_itemSource = value;

				using var dispatcherCallback = new CustomDiffDispatcher(HeaderList.Count, this);
				result.DispatchUpdatesTo(dispatcherCallback);
			}
		}

		protected CommonAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public CommonAdapter(List<Type> stateList, Dictionary<Type, Func<string, IComponentView>> componentCreatorMap)
		{
			_stateList = stateList;
			ComponentCreatorMap = componentCreatorMap;
		}

		public CommonAdapter(Type type, Func<string, IComponentView> componentCreator)
		{
			_stateList = new List<Type>
			{
				type
			};
			ComponentCreatorMap = new Dictionary<Type, Func<string, IComponentView>>
			{
				{
					type, componentCreator
				}
			};
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			switch (holder)
			{
				case HeaderViewHolder headerViewHolder:
					IComponentView headerComponent = headerViewHolder.Component;
					if (_headerStates.TryGetValue(headerComponent, out IViewState headerState))
					{
						headerComponent.SetState(headerState);
					}

					break;
				case FooterViewHolder footerViewHolder:
					IComponentView footerComponent = footerViewHolder.Component;
					if (_footerStates.TryGetValue(footerComponent, out IViewState footerState))
					{
						footerComponent.SetState(footerState);
					}

					break;
				case CellViewHolder cellViewHolder:
					int pos = position - HeaderList.Count;
					if (pos < ItemSource.Count)
					{
						cellViewHolder.SetState(ItemSource[pos]);
					}

					break;
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			if (viewType >= FOOTER_START_TYPE_INDEX)
			{
				int footerPosition = viewType - FOOTER_START_TYPE_INDEX;
				IComponentView footer = _footerList[footerPosition];

				return new FooterViewHolder(footer, footer.View(parent)).DisposeViewWith(_disposable);
			}

			if (viewType >= HEADER_START_TYPE_INDEX)
			{
				int headerPosition = viewType - HEADER_START_TYPE_INDEX;
				IComponentView header = HeaderList[headerPosition];
				if (_stickyDictionary.TryGetValue(header, out View stickyView))
				{
					return new StickyViewHolder(stickyView).DisposeViewWith(_disposable);
				}
				else
				{
					View headerView = header.View(parent).DisposeViewWith(_disposable);
					HeaderViewHolder viewHolder = new HeaderViewHolder(header, headerView).DisposeViewWith(_disposable);

					if (_headersHeight.TryGetValue(header, out int? height) && height.HasValue)
					{
						ViewGroup.LayoutParams param = headerView.LayoutParameters;
						param!.Height = height.Value;
						headerView.LayoutParameters = param;
					}

					if (_parallaxDictionary.TryGetValue(header, out ParallaxRecyclerViewHelper helper))
					{
						helper.AddParallaxView(headerView);
					}

					return viewHolder;
				}
			}

			int index = viewType - CELL_TYPE;
			Type type = _stateList[index];
			IComponentView component = ComponentCreatorMap[type](Guid.NewGuid().ToString()).DisposeViewWith(_disposable);
			return new CellViewHolder(component, component.View(parent)).DisposeViewWith(_disposable);
		}

		public override int GetItemViewType(int position)
		{
			if (position < HeaderList.Count)
			{
				return HEADER_START_TYPE_INDEX + GetHeaderPositionInHeaderList(position);
			}

			if (position >= ItemCount - _footerList.Count)
			{
				return FOOTER_START_TYPE_INDEX + GetFooterPositionInFooterList(position);
			}

			int cellPosition = position - HeaderList.Count;
			int index = _stateList.IndexOf(ItemSource[cellPosition].GetType());
			return CELL_TYPE + index;
		}

		#region Add Header Footer

		public bool TryAddHeader(IComponentView header, int? height = null, int? index = null)
		{
			if (HeaderList.Contains(header))
			{
				return false;
			}
			else
			{
				if (height.HasValue)
				{
					_headersHeight[header] = height;
				}

				if (index.HasValue && index < HeaderList.Count)
				{
					HeaderList.Insert(index.Value, header);
					NotifyItemInserted(index.Value);
				}
				else
				{
					HeaderList.Add(header);
					NotifyDataSetChanged();
				}

				return true;
			}
		}

		public bool TryAddParallaxHeader(IComponentView header, ParallaxRecyclerViewHelper helper, int? height = null)
		{
			if (HeaderList.Contains(header))
			{
				return false;
			}
			else
			{
				HeaderList.Add(header);
				if (height.HasValue)
				{
					_headersHeight[header] = height;
				}

				_parallaxDictionary[header] = helper;
				NotifyDataSetChanged();
				return true;
			}
		}

		public bool TryAddStickyHeader(IComponentView component, View stickyView)
		{
			if (HeaderList.Contains(component))
			{
				return false;
			}
			else
			{
				HeaderList.Add(component);
				_stickyDictionary.Add(component, stickyView);
				NotifyDataSetChanged();
				return true;
			}
		}

		public bool TryAddFooter(IComponentView footer)
		{
			if (_footerList.Contains(footer))
			{
				return false;
			}
			else
			{
				_footerList.Add(footer);
				NotifyDataSetChanged();
				return true;
			}
		}

		#endregion

		#region Remove Header Footer

		public bool TryRemoveHeader(IComponentView header)
		{
			int index = HeaderList.IndexOf(header);
			if (index != -1)
			{
				bool res = HeaderList.Remove(header);

				if (res)
				{
					_headerStates.Remove(header);
					_parallaxDictionary.Remove(header);
					_headersHeight.Remove(header);
					_stickyDictionary.Remove(header);
					NotifyDataSetChanged();
 				}

				return res;
			}
			else
			{
				return false;
			}
		}

		public bool TryRemoveFooter(IComponentView footer)
		{
			int index = _footerList.IndexOf(footer);
			if (index != 1)
			{
				bool res = _footerList.Remove(footer);
				if (res)
				{
					_footerStates.Remove(footer);
					NotifyDataSetChanged();
				}

				return res;
			}
			else
			{
				return false;
			}
		}

		#endregion

		#region Update Header Footer

		public bool TryUpdateHeader(IComponentView component, IViewState viewState)
		{
			if (!HeaderList.Contains(component))
			{
				return false;
			}

			_headerStates[component] = viewState;
			try
			{
				component.SetState(viewState);
			}
			catch (NullReferenceException)
			{
				//NRE can be throw if the cell is not presented
			}

			return true;
		}

		public bool TryUpdateFooter(IComponentView component, IViewState viewState)
		{
			if (!_footerList.Contains(component))
			{
				return false;
			}

			_footerStates[component] = viewState;
			try
			{
				component.SetState(viewState);
			}
			catch (NullReferenceException)
			{
				//NRE can be throw if the cell is not presented
			}

			return true;
		}

		#endregion

		#region Position

		/// <summary>
		/// Get position of a header to the header list
		/// </summary>
		/// <param name="position">position in adapter list</param>
		/// <returns>position in header list</returns>
		private static int GetHeaderPositionInHeaderList(int position) => position;

		/// <summary>
		/// Get position of a footer to the footer list
		/// </summary>
		/// <param name="position">position in adapter list</param>
		/// <returns>position in footer list</returns>
		private int GetFooterPositionInFooterList(int position)
		{
			int cellCount = ItemCount;
			cellCount -= HeaderList.Count;
			cellCount -= _footerList.Count;

			return position - (cellCount + HeaderList.Count);
		}

		#endregion

		public override int ItemCount
		{
			get
			{
				int count = ItemSource?.Count ?? 0;

				count += HeaderList.Count;
				count += _footerList.Count;

				return count;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_stateList = null;
				ComponentCreatorMap = null;

				_parallaxDictionary = null;
				_headersHeight = null;
				_stickyDictionary = null;
				HeaderList = null;
				_headerStates = null;
				_footerList = null;
				_footerStates = null;

				_disposable.Dispose();
			}

			base.Dispose(disposing);
		}

		#region View Holder Lifecycle

		public override void OnViewAttachedToWindow(Object holder)
		{
			base.OnViewAttachedToWindow(holder);
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewAttachedToWindow();
			}
		}

		public override void OnViewDetachedFromWindow(Object holder)
		{
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewDetachedFromWindow();
			}

			base.OnViewDetachedFromWindow(holder);
		}

		public override void OnViewRecycled(Object holder)
		{
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewRecycled();
			}

			base.OnViewRecycled(holder);
		}

		#endregion
	}
}