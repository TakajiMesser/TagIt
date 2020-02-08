using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.Views.Tags;

namespace TagIt.WPF.ViewModels.Tags.Trees
{
    public class TagCategoryViewModel : ViewModel
    {
        private ITagRearranger _tagRearranger;
        private List<TaggedViewModel> _tags = new List<TaggedViewModel>();

        private RelayCommand _removeCommand;
        private RelayCommand _dropCommand;

        private TagCategoryViewModel(string category)
        {
            Name = category;

            ContextMenu.Items.Add(new MenuItem()
            {
                Header = "Remove",
                Command = RemoveCommand
            });
        }

        public string Name { get; }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public ReadOnlyCollection<TaggedViewModel> Children { get; set; }

        public ContextMenu ContextMenu { get; set; } = new ContextMenu();

        public RelayCommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand(
            p => TagManager.Instance.RemoveCategory(Name)
        ));

        public RelayCommand DropCommand => _dropCommand ?? (_dropCommand = new RelayCommand(
            p =>
            {
                var args = (DragEventArgs)p;

                if (/*args.OriginalSource is TagBranchViewModel && */args.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    var tagName = args.Data.GetData(DataFormats.StringFormat) as string;
                    _tagRearranger.RearrangeTag(Name, tagName, args);

                    /*var originalSource = args.OriginalSource;
                    var source = args.Source;
                    var position = args.GetPosition(null);*/
                    //_rearranger.Rearrange((string)name, args);
                }
            },
            p => true
        ));

        public static TagCategoryViewModel CreateForCategory(string category, IContentController contentController, ITagRearranger tagRearranger)
        {
            var viewModel = new TagCategoryViewModel(category)
            {
                _tagRearranger = tagRearranger
            };

            foreach (var tag in TagManager.Instance.GetTags(category))
            {
                viewModel._tags.Add(new TaggedViewModel(tag, contentController, tagRearranger));
            }

            viewModel.Children = new ReadOnlyCollection<TaggedViewModel>(viewModel._tags);

            return viewModel;
        }

        public static TagCategoryViewModel CreateForUncategorized(IContentController contentController, ITagRearranger tagRearranger)
        {
            var viewModel = new TagCategoryViewModel("Uncategorized")
            {
                _tagRearranger = tagRearranger
            };

            foreach (var tag in TagManager.Instance.UncategorizedTags)
            {
                viewModel._tags.Add(new TaggedViewModel(tag, contentController, tagRearranger));
            }

            viewModel.Children = new ReadOnlyCollection<TaggedViewModel>(viewModel._tags);

            return viewModel;
        }

        /*public void UpdateFromModel(ITagProvider tagProvider, IVideoController videoController)
        {
            _videoController = videoController;
            _tagProvider = tagProvider;

            foreach (var tag in _tagProvider.GetAllTags())
            {
                _tagBranches.Add(new TagBranchViewModel(tag, _tagProvider, _videoController));
            }

            Children = new ReadOnlyCollection<TagBranchViewModel>(_tagBranches);
        }*/
    }
}
