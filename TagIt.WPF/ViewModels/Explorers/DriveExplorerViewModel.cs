using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Drive;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.ViewModels.Explorers
{
    public class DriveExplorerViewModel : ViewModel, IExplorer
    {
        private DriveFetcher _driveFetcher = new DriveFetcher();
        private string _currentID;
        private IContentController _contentController;
        private List<ContentViewModel> _children = new List<ContentViewModel>();
        private List<IImageButton> _selectedChildren = new List<IImageButton>();

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
                        if (!string.IsNullOrEmpty(_currentID) && _driveFetcher.HasContent(_currentID))
                        {
                            _currentID = null;
                            GenerateChildren(_driveFetcher.RootContents, TagToggleButtonSetViewModel.SelectedItems);
                        }
                        else
                        {
                            var currentContent = _driveFetcher.GetContent(_currentID);

                            if (currentContent.Parent is DriveFolder parentFolder)
                            {
                                _currentID = parentFolder.ID;
                                GenerateChildren(parentFolder.Children, TagToggleButtonSetViewModel.SelectedItems);
                            }
                        }
                    },
                    p =>
                    {
                        return !string.IsNullOrEmpty(_currentID);
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
            if (content is DriveFolder driveFolder)
            {
                foreach (var child in driveFolder.Children)
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
            _driveFetcher.Fetch();

            foreach (var content in _driveFetcher.RootContents)
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
            if (contentSet is DriveFolder driveFolder)
            {
                _children.Clear();

                foreach (var content in driveFolder.Children)
                {
                    _children.Add(new ContentViewModel(content, _contentController, this));
                }

                var tagFilters = TagToggleButtonSetViewModel.SelectedItems;

                if (tagFilters.Count > 0)
                {
                    Children = new ObservableCollection<ImageButtonViewModel>(_children.Where(c => tagFilters.Any(f => IsTagged(c.Content, f))));
                }
                else
                {
                    Children = new ObservableCollection<ImageButtonViewModel>(_children);
                }

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