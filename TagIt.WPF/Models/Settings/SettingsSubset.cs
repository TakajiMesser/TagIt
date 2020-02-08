using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TagIt.WPF.Models
{
    public abstract class SettingsSubset : ISettingsSubset
    {
        public SettingsSubset(string name)
        {
            Name = name;
            LoadDefault();
        }

        public string Name { get; }

        protected abstract void LoadDefault();
    }
}
