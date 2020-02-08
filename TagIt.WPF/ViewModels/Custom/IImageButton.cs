using System.Windows.Media.Imaging;

namespace TagIt.WPF.ViewModels.Custom
{
    public interface IImageButton
    {
        string Name { get; }
        BitmapSource Image { get; }

        RelayCommand SelectCommand { get; }
        RelayCommand OpenCommand { get; }
        RelayCommand DragCommand { get; }
    }
}
