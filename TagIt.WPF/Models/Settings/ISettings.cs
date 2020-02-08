using System.Collections.Generic;

namespace TagIt.WPF.Models
{
    public interface ISettings
    {
        List<ISettingsSubset> Subsets { get; }
    }
}
