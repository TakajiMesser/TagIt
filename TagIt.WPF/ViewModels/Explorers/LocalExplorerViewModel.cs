using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Local;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Helpers;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.ViewModels.Explorers
{
    public class LocalExplorerViewModel : ViewModel, IExplorer
    {
        private LocalFetcher _localFetcher;
        private string _currentPath;
        private IContentController _contentController;
        private List<ContentViewModel> _children = new List<ContentViewModel>();
        private List<IImageButton> _selectedChildren = new List<IImageButton>();

        public LocalExplorerViewModel()
        {
            _localFetcher = new LocalFetcher(FilePathHelper.INITIAL_LOCAL_DIRECTORY);
            _currentPath = FilePathHelper.INITIAL_LOCAL_DIRECTORY;
        }

        public ObservableCollection<ImageButtonViewModel> Children { get; set; }
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
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand(
                    p =>
                    {
                        if (_currentPath == FilePathHelper.INITIAL_LOCAL_DIRECTORY)
                        {
                            _currentPath = null;
                            GenerateChildren(_localFetcher.RootContents, TagToggleButtonSetViewModel.SelectedItems);
                        }
                        else
                        {
                            var currentContent = _localFetcher.GetContent(_currentPath);

                            if (currentContent.Parent is LocalFolder parentContent)
                            {
                                _currentPath = parentContent.Path;
                                GenerateChildren(parentContent.Children, TagToggleButtonSetViewModel.SelectedItems);
                            }
                        }
                        
                    },
                    p =>
                    {
                        return !string.IsNullOrEmpty(_currentPath);
                    }
                ));
            }
        }

        private void GenerateChildren(IEnumerable<IContent> contents, IList<string> tagFilters)
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
                    BackCommand.InvokeCanExecuteChanged();

                    /*var children = FolderViewModel.FilterChildren(_currentFolder.Items, _playlistController, _videoController, this, p =>
                    {
                        foreach (var tagName in args.Items)
                        {
                            if (IsTagged(p, tagName))
                            {
                                return true;
                            }
                        }

                        return false;
                    });

                    SwapChildren(children);*/
                }
                else
                {
                    Children = new ObservableCollection<ImageButtonViewModel>(_children);
                    BackCommand.InvokeCanExecuteChanged();
                    /*var children = FolderViewModel.CreateChildren(_currentFolder.Items, _playlistController, _videoController, this);
                    SwapChildren(children);*/
                }
            };
        }

        private bool IsTagged(IContent content, string tagName)
        {
            if (content is LocalFolder localFolder)
            {
                foreach (var child in localFolder.Children)
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

        public void UpdateFromModel(IContentController contentController)
        {
            _contentController = contentController;
            _localFetcher.Fetch();

            foreach (var content in _localFetcher.RootContents)
            {
                _children.Add(new ContentViewModel(content, contentController, this));
            }

            SwapChildren(_children);
            /*// For now, just load and add arbritrary components for testing purposes
            //Task.Run(() =>
            //{
                _libraryManager.Load();

                var baseFolderInfo = _libraryManager.GetBasePaths().First() as IFolderInfo;
                _currentFolder = baseFolderInfo;
                SwapChildren(FolderViewModel.CreateChildren(baseFolderInfo.Items, _playlistController, _videoController, this));
            //});*/
        }

        public void OpenContentSet(IContentSet contentSet)
        {
            if (contentSet is LocalFolder localFolder)
            {
                GenerateChildren(localFolder.Children, TagToggleButtonSetViewModel.SelectedItems);
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