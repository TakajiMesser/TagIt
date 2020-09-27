using System.Windows.Controls;

namespace TagIt.WPF.Views.Viewers
{
    public partial class BrowserPanel : Grid
    {
        public BrowserPanel()
        {
            InitializeComponent();
            ViewModel.SetWebBrowser(Browser);
        }
    }
}
