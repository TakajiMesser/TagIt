using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Fetch;
using TagIt.Shared.Models.Tags;
using TagIt.ViewModels.Tabs;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.ViewModels.Explorers
{
    public class ExplorerViewModel<T> : PanelViewModel, IExplorer where T : IContent
    {
        private IContentController _contentController;
        private ContentFetcher<T> _fetcher;
        private string _currentPath;

        private List<ContentViewModel> _children = new List<ContentViewModel>();
        private List<IImageButton> _selectedChildren = new List<IImageButton>();

        public ObservableCollection<ImageButtonViewModel> Children { get; set; } = new ObservableCollection<ImageButtonViewModel>();
        public IEnumerable<IImageButton> SelectedChildren
        {
            get => _selectedChildren;
            set
            {
                _selectedChildren.Clear();
                _selectedChildren.AddRange(value);
            }
        }

        //public PathSortStyles SortStyle { get; set; }

        public ToggleButtonSetViewModel TagToggleButtonSetViewModel { get; set; }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand => _backCommand ?? (_backCommand = new RelayCommand(
            p =>
            {
                if (!string.IsNullOrEmpty(_currentPath) && _fetcher.GetContentOrDefault(_currentPath) is IContent content && content.Parent is ContentSet<T> contentSet)
                {
                    _currentPath = contentSet.Path;
                    GenerateChildren(contentSet.Contents, TagToggleButtonSetViewModel.SelectedItems);
                }
                else
                {
                    _currentPath = null;
                    GenerateChildren(_fetcher.RootContents, TagToggleButtonSetViewModel.SelectedItems);
                }

                BackCommand.InvokeCanExecuteChanged();
            },
            p => !string.IsNullOrEmpty(_currentPath)
        ));

        private void GenerateChildren(IEnumerable<T> contents, IList<string> tagFilters)
        {
            _children.Clear();

            foreach (var content in contents)
            {
                _children.Add(new ContentViewModel(content, _contentController, this));
            }

            if (tagFilters.Count > 0)
            {
                Children = new ObservableCollection<ImageButtonViewModel>(_children.Where(c => tagFilters.Any(f => IsTagged(c.Content, f))));
            }
            else
            {
                Children = new ObservableCollection<ImageButtonViewModel>(_children);
            }
        }

        public void OnTagToggleButtonSetViewModelChanged()
        {
            TagToggleButtonSetViewModel.SelectionChanged += (s, args) =>
            {
                // TODO - Filter available videos based on selection
                if (args.Items.Any())
                {
                    Children = new ObservableCollection<ImageButtonViewModel>(_children.Where(c => args.Items.Any(i => IsTagged(c.Content, i))));
                }
                else
                {
                    Children = new ObservableCollection<ImageButtonViewModel>(_children);
                }

                BackCommand.InvokeCanExecuteChanged();
            };
        }

        protected bool IsTagged(IContent content, string tagName)
        {
            if (content is IContentSet<T> contentSet)
            {
                foreach (var child in contentSet.Contents)
                {
                    if (IsTagged(child, tagName))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return TagManager.Instance.IsTagged(tagName, content);
            }

            return false;
        }

        public async void UpdateFromModel(IContentController contentController, ContentFetcher<T> fetcher)
        {
            _contentController = contentController;
            _fetcher = fetcher;

            await _fetcher.Fetch();

            foreach (var content in _fetcher.RootContents)
            {
                _children.Add(new ContentViewModel(content, contentController, this));
            }

            SwapChildren(_children);
        }

        public void OpenContentSet(IContentSet contentSet)
        {
            if (contentSet is ContentSet<T> castContentSet)
            {
                GenerateChildren(castContentSet.Contents, TagToggleButtonSetViewModel.SelectedItems);
                _currentPath = contentSet.Path;
                BackCommand.InvokeCanExecuteChanged();
                //SwapChildren(_children);
            }
        }

        public void OnSortStyleChanged()
        {
            /*_children.Select(c => c.PathInfo).Refresh(SortStyle);
            var keySelector = PathInfoHelper.GetKeySelector(SortStyle);
            var sortedChildren = _children.OrderBy(c => keySelector(c.PathInfo)).ToList();
            //var sortedChildren = _children.OrderBy(c => c.PathInfo.GetKeySelector(SortStyle)).ToList();

            SwapChildren(sortedChildren);*/
        }

        private void SwapChildren(IEnumerable<ContentViewModel> items)
        {
            // TODO - Is this still necessary?
            items = items.ToList();

            _children.Clear();
            _children.AddRange(items);

            Children = new ObservableCollection<ImageButtonViewModel>(_children);
            BackCommand.InvokeCanExecuteChanged();
        }
    }
}