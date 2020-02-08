using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TagIt.Shared.Models.Tags;

namespace TagIt.WPF.ViewModels.Custom
{
    public class ToggleButtonSetViewModel : ViewModel
    {
        private List<string> _items = new List<string>();

        public ObservableCollection<string> Items { get; set; }
        public ObservableCollection<string> SelectedItems { get; private set; } = new ObservableCollection<string>();

        public event EventHandler<SelectionEventArgs> SelectionChanged;

        public ToggleButtonSetViewModel()
        {
            TagManager.Instance.TagAdded += (s, args) =>
            {
                _items.Add(args.Tag.Name);
                Items = new ObservableCollection<string>(_items);
            };

            foreach (var tag in TagManager.Instance.Tags)
            {
                _items.Add(tag.Name);
            }

            Items = new ObservableCollection<string>(_items);
        }

        public void UpdateSelection(IEnumerable<string> items)
        {
            SelectedItems = new ObservableCollection<string>(items);
            SelectionChanged?.Invoke(this, new SelectionEventArgs(items));
        }

        public class SelectionEventArgs : EventArgs
        {
            public IEnumerable<string> Items { get; }

            public SelectionEventArgs(IEnumerable<string> items) => Items = items;
        }
    }
}
