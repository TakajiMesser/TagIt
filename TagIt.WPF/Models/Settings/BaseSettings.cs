using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TagIt.WPF.Models
{
    public abstract class BaseSettings<T> : ISettings where T : class, new()
    {
        protected BaseSettings() => Subsets.AddRange(GetSubsets());

        public List<ISettingsSubset> Subsets { get; } = new List<ISettingsSubset>();

        public abstract void Commit();

        protected abstract IEnumerable<ISettingsSubset> GetSubsets();

        protected void Save(string filePath)
        {
            using (var writer = XmlWriter.Create(filePath))
            {
                var serializer = new NetDataContractSerializer();
                serializer.WriteObject(writer, this);
            }
        }

        protected static T LoadOrDefault(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var reader = XmlReader.Create(filePath))
                {
                    var serializer = new NetDataContractSerializer();
                    return serializer.ReadObject(reader, true) as T;
                }
            }
            else
            {
                return new T();
            }
        }
    }
}
