using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.LinearList;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.Controls.ChipCloud
{
	public class ChipCloudAdapter : BaseAdapter<IEntityViewState>
	{
		private readonly Dictionary<int, IComponentView> _componentDictionary;

		private IReadOnlyList<IEntityViewState> _itemSource;
		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				(List<Remove> removeList, List<Move> moveList, List<Add> addList) = DiffListUtil.DiffList(_itemSource?.ToArray() ?? Array.Empty<IEntityViewState>(), value?.ToArray() ?? Array.Empty<IEntityViewState>());
				_itemSource = value;
				UpdateComponentDictionary(removeList, moveList);
				ItemSourceChanged?.Invoke(removeList, moveList, addList);
			}
		}

		public Context Context { get; }

		public Func<string, IComponentView> ComponentCreator { get; }

		public event ListChanged ItemSourceChanged;

		public delegate void ListChanged(List<Remove> removeList, List<Move> moveList, List<Add> addList);

		public ChipCloudAdapter(Context context, Func<string, IComponentView> componentCreator)
		{
			Context = context;
			ComponentCreator = componentCreator;

			_componentDictionary = new Dictionary<int, IComponentView>();
		}

		protected ChipCloudAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public override IEntityViewState this[int position] => ItemSource[position];

		public override long GetItemId(int position) => position;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			IEntityViewState item = ItemSource[position];
			IComponentView comp = ComponentCreator(item.Id.ToString());
			_componentDictionary[position] = comp;
			View view = comp.View(parent);
			return view;
		}

		private void UpdateComponentDictionary(List<Remove> removeList, List<Move> moveList)
		{
			var oldDic = new Dictionary<int, IComponentView>(_componentDictionary);

			foreach (Remove remove in removeList)
			{
				_componentDictionary.Remove(remove.OldPos);
			}

			foreach (Move move in moveList.OrderBy(x => x.OldPos))
			{
				_componentDictionary[move.NewPos] = oldDic[move.OldPos];

				if (removeList.Count > 0 && moveList.All(x => x.NewPos != move.OldPos))
				{
					//CLA 17/05/2023 : remove element from dictionary only if an element has been removed (tail must be truncated by moving all next elements)
					// Furthermore, we need to check if there is no inversion between this element and another (in this case dictionary must keep elements)
					_componentDictionary.Remove(move.OldPos);
				}
			}
		}

		public void RefreshAllStates()
		{
			foreach (KeyValuePair<int, IComponentView> keyValue in _componentDictionary)
			{
				keyValue.Value.SetState(ItemSource[keyValue.Key]);
			}
		}

		public override int Count => ItemSource.Count;
	}
}
