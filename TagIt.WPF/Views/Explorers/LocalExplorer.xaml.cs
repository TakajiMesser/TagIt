using System.Windows.Controls;

namespace TagIt.WPF.Views.Explorers
{
    public partial class LocalExplorer : Grid
    {
        public LocalExplorer()
        {
            InitializeComponent();
            ViewModel.TagToggleButtonSetViewModel = TagButtonSet.ViewModel;
        }
    }
}
