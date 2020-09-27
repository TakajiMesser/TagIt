using System.Windows;

namespace TagIt.WPF.ViewModels.Viewers
{
    public interface IViewerViewModel
    {
        UIElement ContentElement { get; set; }

        void AttachContentElement();
        void DetachContentElement();
    }
}
