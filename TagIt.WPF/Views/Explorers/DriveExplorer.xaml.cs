using System.Windows.Controls;

namespace TagIt.WPF.Views.Explorers
{
    public partial class DriveExplorer : Grid
    {
        public DriveExplorer()
        {
            InitializeComponent();
            ViewModel.TagToggleButtonSetViewModel = TagButtonSet.ViewModel;
        }
    }
}
