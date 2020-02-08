using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.Views.Tags;

namespace TagIt.WPF.ViewModels.Tags.Trees
{
    public class TagTreeViewModel : ViewModel, ITagRearranger
    {
        private IContentController _contentController;
        private List<TagCategoryViewModel> _categories = new List<TagCategoryViewModel>();

        private RelayCommand _addCommand;

        //public ITagRearranger TagRearranger { get; set; }
        public string NewCategory { get; set; }

        public ReadOnlyCollection<TagCategoryViewModel> Children { get; set; }

        public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(
            p =>
            {
                if (!string.IsNullOrEmpty(NewCategory))
                {
                    if (!TagManager.Instance.HasCategory(NewCategory))
                    {
                        TagManager.Instance.CreateCategory(NewCategory);
                        //_categories.Add(TagCategoryViewModel.CreateForCategory(NewCategory, _tagProvider, _videoController, _playlistController, this));
                        //Children = new ReadOnlyCollection<TagCategoryViewModel>(_categories);
                    }

                    NewCategory = "";
                }
            },
            p => true
        ));

        public void UpdateFromModel(IContentController contentController)
        {
            _contentController = contentController;

            TagManager.Instance.TagsChanged += (s, args) => RefreshChildren();
            TagManager.Instance.CategoriesChanged += (s, args) => RefreshChildren();

            RefreshChildren();
        }

        public void RefreshChildren()
        {
            _categories.Clear();

            foreach (var category in TagManager.Instance.Categories)
            {
                _categories.Add(TagCategoryViewModel.CreateForCategory(category, _contentController, this));
            }

            if (TagManager.Instance.UncategorizedTags.Any())
            {
                _categories.Add(TagCategoryViewModel.CreateForUncategorized(_contentController, this));
            }

            Children = new ReadOnlyCollection<TagCategoryViewModel>(_categories);
        }

        public void RearrangeTag(string category, string tagName, DragEventArgs args)
        {
            // This could be the "Uncategorized" category view model
            var toCategory = _categories.FirstOrDefault(c => c.Name == category);

            // This could return NULL if the tag is uncategorized
            var fromTag = TagManager.Instance.GetTag(tagName);
            var fromCategory = _categories.FirstOrDefault(c => c.Name == fromTag.Category);

            if (fromCategory != null && toCategory.Name == "Uncategorized")
            {
                TagManager.Instance.UncategorizeTag(tagName);
                //RefreshChildren();
            }
            else if (fromCategory == null && toCategory.Name != "Uncategorized")
            {
                TagManager.Instance.CategorizeTag(tagName, category);
                //RefreshChildren();
            }
            else if (fromCategory != null && fromCategory.Name != toCategory.Name)
            {
                TagManager.Instance.CategorizeTag(tagName, category);
                //RefreshChildren();
            }
        }

        public void RearrangeVideo(string tagName, string videoName, DragEventArgs args)
        {

        }
    }
}
