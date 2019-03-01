using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.LinearList
{
	public interface IAdapterWithSeparator
	{
		View CreateSeparator();
	}

	public class LinearListAdapterWithSeparator : LinearListAdapter, IAdapterWithSeparator
	{
		private readonly Func<View> _separatorFactory;

		public LinearListAdapterWithSeparator(Context context, Func<string, IComponentView> componentCreator, Func<View> separatorFactory) : base(context, componentCreator)
		{
			_separatorFactory = separatorFactory;
		}

		protected LinearListAdapterWithSeparator(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public View CreateSeparator() => _separatorFactory();
	}

	public class LinearListAdapter : BaseAdapter<IEntityViewState>
	{
		public Dictionary<int, IComponentView> ComponentDictionary { get; }

		private IReadOnlyList<IEntityViewState> _itemSource;
		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				(var removeList, var moveList, var addList) = DiffListUtil.DiffList(_itemSource?.ToArray() ?? new IEntityViewState[0], value?.ToArray() ?? new IEntityViewState[0]);
				_itemSource = value;
				UpdateComponentDictionary(removeList, moveList);
				ItemSourceChanged?.Invoke(removeList, moveList, addList);
			}
		}

		public Context Context { get; }

		public Func<string, IComponentView> ComponentCreator { get; }

		public event ListChanged ItemSourceChanged;

		public delegate void ListChanged(List<Remove> removeList, List<Move> moveList, List<Add> addList);

		public LinearListAdapter(Context context, Func<string, IComponentView> componentCreator)
		{
			Context = context;
			ComponentCreator = componentCreator;

			ComponentDictionary = new Dictionary<int, IComponentView>();
		}

		protected LinearListAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public override IEntityViewState this[int position] => ItemSource[position];

		public override long GetItemId(int position) => position;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = ItemSource[position];
			var comp = ComponentCreator(item.Id.ToString());
			ComponentDictionary[position] = comp;
			var view = comp.View(parent);
			return view;
		}

		private void UpdateComponentDictionary(List<Remove> removeList, List<Move> moveList)
		{
			var oldDic = new Dictionary<int, IComponentView>(ComponentDictionary);

			foreach (var remove in removeList)
			{
				ComponentDictionary.Remove(remove.OldPos);
			}

			foreach (var move in moveList)
			{
				ComponentDictionary[move.NewPos] = oldDic[move.OldPos];
			}
		}

		public void RefreshAllStates()
		{
			foreach (var keyValue in ComponentDictionary)
			{
				if (keyValue.Key < ItemSource.Count)
				{
					keyValue.Value.SetState(ItemSource[keyValue.Key]);
				}
			}
		}

		public override int Count => ItemSource.Count;
	}
}
