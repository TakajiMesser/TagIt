using System.Windows;

namespace TagIt.WPF.Views.Factories
{
    public interface IWindow
    {
        WindowStyle WindowStyle { get; set; }
        WindowState WindowState { get; set; }

        object Content { get; set; }

        void Close();
    }
}
