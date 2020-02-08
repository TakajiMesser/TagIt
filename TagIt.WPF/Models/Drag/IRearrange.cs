using System.Windows;

namespace TagIt.WPF.Models.Drag
{
    public interface IRearrange
    {
        void Rearrange(string name, DragEventArgs args);
    }
}
