using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TagIt.Shared.Models.Contents;
using TagIt.Shared.Models.Viewers;
using TagIt.ViewModels.Tabs;

namespace TagIt.WPF.ViewModels.Viewers
{
    public class ViewerViewModel<T> : PanelViewModel, IViewerViewModel where T : Viewer
    {
        private UIElement _element;
        private Panel _parent;

        protected T _viewer;

        public UIElement ContentElement
        {
            get => _element;
            set
            {
                _element = value;
                _parent = VisualTreeHelper.GetParent(value) as Panel;
            }
        }

        public void Open(IContent content) => _viewer.Open(content);

        public void AttachContentElement() => _parent.Children.Add(_element);

        public void DetachContentElement() => _parent.Children.Remove(_element);
    }
}
