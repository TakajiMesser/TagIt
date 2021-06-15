﻿using Android.Content;
using System;
using System.Collections.Generic;

namespace TagIt.Droid.Adapters
{
    public abstract class MultiSelectListAdapter<T> : MultiSelectAdapter<T>
    {
        private IList<T> _items;

        public MultiSelectListAdapter(Context context, IList<T> items) : base(context) => _items = items;

        public override int ItemCount => _items.Count;

        public IEnumerable<T> SelectedItems
        {
            get
            {
                foreach (var position in SelectedPositions)
                {
                    yield return _items[position];
                }
            }
        }

        protected override T GetItemAt(int position) => _items[position];

        public void ReplaceItems(IList<T> items)
        {
            _items = items;
            NotifyDataSetChanged();
        }

        public void AddItem(T item)
        {
            _items.Add(item);
            NotifyItemInserted(_items.Count - 1);
        }

        public void AddItems(IEnumerable<T> items)
        {
            var startPosition = _items.Count;
            foreach (var item in items)
            {
                _items.Add(item);
            }

            NotifyItemRangeInserted(startPosition, _items.Count - startPosition);
        }

        public void RemoveItem(T item)
        {
            var index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                NotifyItemRemoved(index);
            }
        }

        protected SearchFilter<T> CreateSearchFilter(Func<T, string> func) => new SearchFilter(this, func);

        private class SearchFilter : SearchFilter<T>
        {
            private MultiSelectListAdapter<T> _adapter;
            private Func<T, string> _func;

            public SearchFilter(MultiSelectListAdapter<T> adapter, Func<T, string> func)
            {
                _adapter = adapter;
                _func = func;
            }

            protected override IList<T> GetItemsToFilter() => _adapter._items;
            protected override string GetSearchStringFromItem(T item) => _func(item);

            protected override void SetItemsFromResults(List<T> items)
            {
                _adapter._items = items;
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}