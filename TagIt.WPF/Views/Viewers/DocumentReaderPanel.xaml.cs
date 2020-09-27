using System.Windows.Controls;
using System.Windows.Threading;

namespace TagIt.WPF.Views.Viewers
{
    /// <summary>
    /// Interaction logic for DocumentReaderPanel.xaml
    /// </summary>
    public partial class DocumentReaderPanel : Grid
    {
        private DispatcherTimer _currentTimer;

        public DocumentReaderPanel()
        {
            InitializeComponent();

            ViewModel.ContentElement = Viewer;
            ViewModel.SetReader(Viewer);
        }
    }
}
