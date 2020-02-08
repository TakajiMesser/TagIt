using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.Views.Custom
{
    public class ImageButtonList : ListBox
    {
        public readonly static DependencyProperty SelectedImagesProperty = DependencyProperty.Register("SelectedImages", typeof(IEnumerable<IImageButton>), typeof(ImageButtonList),
            new PropertyMetadata(Enumerable.Empty<IImageButton>()));

        public ImageButtonList()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);

            SelectionChanged += (s, args) =>
            {
                SelectedImages = SelectedItems.Cast<IImageButton>();
            };
            /*ImageButtons = new ObservableCollection<IImageButton>();

            ImageButtons.Add(new TestImageButton("Test 01"));
            ImageButtons.Add(new TestImageButton("Test 02"));
            ImageButtons.Add(new TestImageButton("Test 03"));

            ItemsSource = ImageButtons;*/
        }

        //public ObservableCollection<IImageButton> ImageButtons { get; set; } 

        public IEnumerable<IImageButton> SelectedImages
        {
            get => (IEnumerable<IImageButton>)GetValue(SelectedImagesProperty);
            set
            {
                SetValue(SelectedImagesProperty, value);
            }
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            /*var container = element as ListBoxItem;
            var imageButton = item as IImageButton;

            if (imageButton != null && imageButton.SelectCommand != null)
            {
                imageButton.SelectCommand.Executed += (s, args) =>
                {
                    SelectedItem = item;
                };
                //imageButton.SelectCommand = 
            }*/
        }

        static ImageButtonList()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButtonList), new FrameworkPropertyMetadata(typeof(ImageButtonList)));
        }
    }
}
