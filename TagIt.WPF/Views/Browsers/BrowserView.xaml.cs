using System.Windows.Controls;

namespace TagIt.WPF.Views.Browsers
{
    public partial class BrowserView : Grid
    {
        public BrowserView()
        {
            InitializeComponent();
            ViewModel.SetWebBrowser(Browser);
        }
    }
}
