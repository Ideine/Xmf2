using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform.Platform;
using UIKit;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
    public class ItemsControl<TComponent, TData> : UIView
        where TComponent : BaseUIModelComponent<TData>, new()
        where TData : class
    {
        public class ItemSelectedEventArgs : EventArgs
        {
            public TData Item { get; }

            public ItemSelectedEventArgs(TData item)
            {
                Item = item;
            }
        }

        public bool DisableFeedback { get; set; }

        private readonly int _cellHeight;
        private readonly bool _hasSeparator;

        private readonly List<TComponent> _usingCells = new List<TComponent>();
        private readonly Queue<TComponent> _usableCells = new Queue<TComponent>();

        private readonly List<UIView> _usingSeparators = new List<UIView>();
        private readonly Queue<UIView> _usableSeparators = new Queue<UIView>();

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        private IReadOnlyList<TData> _items;
        public IReadOnlyList<TData> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    UpdateListView(value);
                }
            }
        }

        public bool CanSelectItems { get; set; } = true;

        public UIColor SeparatorColor { get; set; } = UIColor.DarkGray;

        public int SeparatorHeight { get; set; } = 1;

        public UIColor FeedbackColor { get; set; } = UIColor.LightGray;

        public Func<UIView> CreateSeparator { get; set; }

        private bool _canLayout;

        public ItemsControl(int cellHeight, bool hasSeparator = true)
        {
            _cellHeight = cellHeight;
            _hasSeparator = hasSeparator;
        }

        public void Layout()
        {
            _canLayout = true;
        }

        private void UpdateListView(IReadOnlyList<TData> items)
        {
            if (items == null)
            {
                items = new List<TData>();
            }

            int itemIndex = 0;
            //try rebinding currently used cells
            for (; itemIndex < _usingCells.Count && itemIndex < items.Count; itemIndex++)
            {
                _usingCells[itemIndex].Model = items[itemIndex];
            }

            //more cells than beacons ?
            if (itemIndex >= items.Count)
            {
                //send old cells & separators to reusable queue
                int removeLength = _usingCells.Count - itemIndex;
                if (removeLength > 0)
                {
                    for (int i = itemIndex; i < _usingCells.Count; ++i)
                    {
                        TComponent cell = _usingCells[i];
                        _usableCells.Enqueue(cell);
                        cell.RemoveFromSuperview();
                    }

                    _usingCells.RemoveRange(itemIndex, removeLength);

                    if (_hasSeparator)
                    {
                        //remove unneeded separators
                        int separatorIndex = Math.Max(0, itemIndex - 1);
                        for (int i = separatorIndex; i < _usingSeparators.Count; ++i)
                        {
                            UIView sep = _usingSeparators[i];
                            sep.RemoveFromSuperview();
                            _usableSeparators.Enqueue(sep);
                        }
                        _usingSeparators.RemoveRange(separatorIndex, Math.Min(_usingSeparators.Count, removeLength));
                    }

                    UIView lastView = _usingCells.LastOrDefault();
                    if (lastView != null)
                    {
                        this.AnchorBottom(lastView);
                    }
                }
            }
            else
            {
                //need more cells
                UIView topView = null;
                if (_usingCells.Count > 0)
                {
                    NSLayoutConstraint constraint = Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Bottom && x.SecondAttribute == NSLayoutAttribute.Bottom);
                    if (constraint != null)
                    {
                        RemoveConstraint(constraint);
                    }
                    topView = _usingCells.Last();
                }

                for (; itemIndex < items.Count; ++itemIndex)
                {
                    if (itemIndex > 0 && _hasSeparator)
                    {
                        //add separator
                        UIView sep = GetOrCreateSeparator();

                        Add(sep);
                        this.VerticalSpace(topView, sep, 0)
                            .CenterAndFillWidth(sep);

                        topView = sep;
                    }

                    //add beacon cell
                    TComponent cell = GetOrCreateCell(items[itemIndex]);

                    Add(cell);
                    this.CenterAndFillWidth(cell);

                    if (topView == null)
                    {
                        this.AnchorTop(cell);
                    }
                    else
                    {
                        this.VerticalSpace(topView, cell, 0);
                    }


                    topView = cell;
                }

                if (topView != null)
                {
                    this.AnchorBottom(topView);
                }
            }

            if (_canLayout)
            {
                LayoutIfNeeded();
            }
        }

        private UIView GetOrCreateSeparator()
        {
            if (_usableSeparators.Any())
            {
                UIView oldSep = _usableSeparators.Dequeue();
                _usingSeparators.Add(oldSep);
                return oldSep;
            }
            UIView sep = (this.CreateSeparator ?? this.CreateDefaultSeparator)();
            _usingSeparators.Add(sep);
            return sep;
        }

        private TComponent GetOrCreateCell(TData item)
        {
            if (_usableCells.Any())
            {
                TComponent oldCellContent = _usableCells.Dequeue();
                oldCellContent.Model = item;

                _usingCells.Add(oldCellContent);

                return oldCellContent;
            }

            TComponent cellContent = new TComponent();
            cellContent.Model = item;
            cellContent.AutoLayout();
            cellContent.Bind();
            _usingCells.Add(cellContent);

            if (CanSelectItems)
            {
                UITapGestureRecognizer gestureRecognizer = new UITapGestureRecognizer(ItemSelectedAction);
                cellContent.AddGestureRecognizer(gestureRecognizer);
            }
            return cellContent;
        }

        private void ItemSelectedAction(UITapGestureRecognizer recognizer)
        {
            try
            {
                TComponent cell = recognizer.View as TComponent;
                if (cell == null)
                {
                    return;
                }

                TData item = cell.Model;
                if (DisableFeedback)
                {
                    ItemSelected?.Invoke(this, new ItemSelectedEventArgs(item));
                }
                else
                {
                    UIView.Animate(0.1, () =>
                    {
                        cell.BackgroundColor = FeedbackColor;
                    }, () =>
                    {
                        ItemSelected?.Invoke(this, new ItemSelectedEventArgs(item));
                        UIView.Animate(0.1, () =>
                        {
                            cell.BackgroundColor = UIColor.Clear;
                        }, () => { });
                    });
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                MvxTrace.Trace($"ItemSelected: Exception: {ex}");
#endif
            }
        }

        private UIView CreateDefaultSeparator()
        {
            return new UIView() { BackgroundColor = SeparatorColor }.ConstrainHeight(SeparatorHeight);
        }
    }
}
