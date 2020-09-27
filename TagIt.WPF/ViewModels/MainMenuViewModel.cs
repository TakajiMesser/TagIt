using Microsoft.Win32;
using System;
using System.IO;
using TagIt.Shared.Models.Local;
using TagIt.WPF.Models;
using TagIt.WPF.ViewModels.Contents;
using TagIt.WPF.Views.Factories;

namespace TagIt.WPF.ViewModels
{
    public class MainMenuViewModel : ViewModel
    {
        private RelayCommand _openVideoCommand;
        private RelayCommand _settingsCommand;

        public IWindowFactory WindowFactory { get; set; }
        public IContentController ContentController { get; set; }

        public RelayCommand OpenVideoCommand
        {
            get
            {
                return _openVideoCommand ?? (_openVideoCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog("mp4", "Video Files|*.mp4", ProgramSettings.InitialVideoDirectory);
                        if (fileName != null)
                        {
                            ContentController.OpenContent(new LocalFile(fileName));
                        }
                    },
                    p => true
                ));
            }
        }

        public RelayCommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new RelayCommand(
                    p => WindowFactory.CreateSettingsWindow(),
                    p => true
                ));
            }
        }

        private string OpenDialog(string defaultExt, string filter, string initialDirectory)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = defaultExt,
                Filter = filter,
                InitialDirectory = NormalizedPath(initialDirectory)
            };

            return dialog.ShowDialog() == true
                ? dialog.FileName
                : null;
        }

        private string NormalizedPath(string path) => Path.GetFullPath(new Uri(path).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant();
    }
}
