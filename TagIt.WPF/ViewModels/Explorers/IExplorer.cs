using System.Collections.Generic;
using TagIt.Shared.Models.Contents;
using TagIt.WPF.ViewModels.Custom;

namespace TagIt.WPF.ViewModels.Explorers
{
    public interface IExplorer
    {
        void OpenContentSet(IContentSet contentSet);

        IEnumerable<IImageButton> SelectedChildren { get; }
    }
}