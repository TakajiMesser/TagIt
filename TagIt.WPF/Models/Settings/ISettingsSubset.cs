using TagIt.WPF.Helpers;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TagIt.WPF.Models
{
    public interface ISettingsSubset
    {
        string Name { get; }
    }
}
