using System.Windows;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models;
using TagIt.WPF.Views;

namespace TagIt.WPF.ViewModels.Settings
{
    public interface IPropertyDisplayer
    {
        void SetPropertyVisibility(string propertyName, Visibility visibility);
    }
}