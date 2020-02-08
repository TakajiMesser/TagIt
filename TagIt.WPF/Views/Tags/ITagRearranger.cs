using System.Windows;

namespace TagIt.WPF.Views.Tags
{
    public interface ITagRearranger
    {
        void RearrangeTag(string category, string tagName, DragEventArgs args);
        void RearrangeVideo(string tagName, string videoName, DragEventArgs args);
    }
}
