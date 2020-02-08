using System.Linq;
using TagIt.WPF.Helpers;
using TagIt.WPF.Models;
using TagIt.WPF.Views;

namespace TagIt.WPF.ViewModels.Settings
{
    public class SettingsWindowViewModel : ViewModel
    {
        private ProgramSettings _settings;

        private RelayCommand _cancelCommand;
        private RelayCommand _okCommand;

        public SettingsWindowViewModel()
        {
            _settings = ProgramSettings.Instance;
            CurrentSubset = _settings.Subsets.First();
        }

        public IPropertyDisplayer PropertyDisplayer { get; set; }
        public IWindow Window { get; set; }

        public ISettingsSubset CurrentSubset { get; set; }

        public RelayCommand OKCommand => _okCommand ?? (_okCommand = new RelayCommand(
            p =>
            {
                _settings.Commit();
                Window.Close();
            },
            p => true
        ));

        public RelayCommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(
            p => Window.Close()
        ));
    }
}