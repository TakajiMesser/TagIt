using System.Windows.Media.Imaging;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.ViewModels.Explorers
{
    public abstract class ImageButtonViewModel : ViewModel, IImageButton
    {
        public ImageButtonViewModel(string name, BitmapImage bitmapImage = null)
        {
            Name = name;
            Image = bitmapImage;
        }

        public string Name { get; }
        public BitmapSource Image { get; protected set; }

        public abstract RelayCommand SelectCommand { get; }
        public abstract RelayCommand OpenCommand { get; }
        public abstract RelayCommand DragCommand { get; }
    }
}