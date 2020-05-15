using System;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;
using Object = Java.Lang.Object;

namespace Xmf2.Components.Droid.List
{
	public class CommonAdapter : RecyclerView.Adapter
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private List<Type> _stateList;
		private Dictionary<Type, Func<string, IComponentView>> _componentCreatorMap;

		public const int CELL_TYPE = 0x2a;
		public const int HEADER_START_TYPE_INDEX = 1000;
		public const int FOOTER_START_TYPE_INDEX = 2000;

		private List<IComponentView> _headerList = new List<IComponentView>();
		private Dictionary<IComponentView, int?> _headersHeight = new Dictionary<IComponentView, int?>();
		private Dictionary<IComponentView, IViewState> _headerStates = new Dictionary<IComponentView, IViewState>();

		private List<IComponentView> _footerList = new List<IComponentView>();
		private Dictionary<IComponentView, IViewState> _footerStates = new Dictionary<IComponentView, IViewState>();

		private Dictionary<IComponentView, ParallaxRecyclerViewHelper> _parallaxDictionary = new Dictionary<IComponentView, ParallaxRecyclerViewHelper>();
		private Dictionary<IComponentView, View> _stickyDictionary = new Dictionary<IComponentView, View>();

		private IReadOnlyList<IEntityViewState> _itemSource;

		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				using (var callback = new DiffList(_itemSource?.ToArray() ?? new IEntityViewState[0], value?.ToArray() ?? new IEntityViewState[0]))
				{
					using (DiffUtil.DiffResult result = DiffUtil.CalculateDiff(callback))
					{
						_itemSource = value;

						using (var dispatcherCallback = new CustomDiffDispatcher(_headerList.Count, this))
						{
							result.DispatchUpdatesTo(dispatcherCallback);
						}
					}
				}
			}
		}

		protected CommonAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public CommonAdapter(List<Type> stateList, Dictionary<Type, Func<string, IComponentView>> componentCreatorMap)
		{
			_stateList = stateList;
			_componentCreatorMap = componentCreatorMap;
		}

		public CommonAdapter(Type type, Func<string, IComponentView> componentCreator)
		{
			_stateList = new List<Type>
			{
				type
			};
			_componentCreatorMap = new Dictionary<Type, Func<string, IComponentView>>()
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
					int pos = position - _headerList.Count;
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
				IComponentView header = _headerList[headerPosition];
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
						param.Height = height.Value;
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
			IComponentView component = _componentCreatorMap[type](Guid.NewGuid().ToString()).DisposeViewWith(_disposable);
			return new CellViewHolder(component, component.View(parent)).DisposeViewWith(_disposable);
		}

		public override int GetItemViewType(int position)
		{
			if (position < _headerList.Count)
			{
				return HEADER_START_TYPE_INDEX + GetHeaderPositionInHeaderList(position);
			}

			if (position >= ItemCount - _footerList.Count)
			{
				return FOOTER_START_TYPE_INDEX + GetFooterPositionInFooterList(position);
			}

			int cellPosition = position - _headerList.Count;
			int index = _stateList.IndexOf(ItemSource[cellPosition].GetType());
			return CELL_TYPE + index;
		}

		#region Add Header Footer

		public bool TryAddHeader(IComponentView header, int? height = null, int? index = null)
		{
			if (_headerList.Contains(header))
			{
				return false;
			}
			else
			{
				if (height.HasValue)
				{
					_headersHeight[header] = height;
				}

				if (index.HasValue)
				{
					_headerList.Insert(index.Value, header);
					NotifyItemInserted(index.Value);
				}
				else
				{
					_headerList.Add(header);
					NotifyDataSetChanged();
				}

				return true;
			}
		}

		public bool TryAddParallaxHeader(IComponentView header, ParallaxRecyclerViewHelper helper, int? height = null)
		{
			if (_headerList.Contains(header))
			{
				return false;
			}
			else
			{
				_headerList.Add(header);
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
			if (_headerList.Contains(component))
			{
				return false;
			}
			else
			{
				_headerList.Add(component);
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
			int index = _headerList.IndexOf(header);
			if (index != -1)
			{
				bool res = _headerList.Remove(header);

				if (res)
				{
					_headerStates.Remove(header);
					_parallaxDictionary.Remove(header);
					_headersHeight.Remove(header);
					_stickyDictionary.Remove(header);
					NotifyItemRemoved(index);
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
				index = GetFooterPositionInItemList(index);
				bool res = _footerList.Remove(footer);
				if (res)
				{
					_footerStates.Remove(footer);
					NotifyItemRemoved(index);
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
			if (!_headerList.Contains(component))
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
		private int GetHeaderPositionInHeaderList(int position) => position;

		/// <summary>
		/// Get position of a footer to the footer list
		/// </summary>
		/// <param name="position">position in adapter list</param>
		/// <returns>position in footer list</returns>
		private int GetFooterPositionInFooterList(int position)
		{
			int cellCount = ItemCount;
			cellCount -= _headerList.Count;
			cellCount -= _footerList.Count;

			return position - (cellCount + _headerList.Count);
		}

		private int GetFooterPositionInItemList(int position)
		{
			return (ItemCount - _footerList.Count) + position;
		}

		#endregion

		public override int ItemCount
		{
			get
			{
				int count = ItemSource?.Count ?? 0;

				count += _headerList.Count;
				count += _footerList.Count;

				return count;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_stateList = null;
				_componentCreatorMap = null;

				_parallaxDictionary = null;
				_headersHeight = null;
				_stickyDictionary = null;
				_headerList = null;
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