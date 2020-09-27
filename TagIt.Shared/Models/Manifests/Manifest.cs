using System.Collections.Generic;
using System.IO;

namespace TagIt.Shared.Models.Manifests
{
    public abstract class Manifest
    {
        public Manifest(string filePath) => FilePath = filePath;

        public string FilePath { get; set; }

        public void Save()
        {
            using (var writer = new StreamWriter(FilePath, false))
            {
                foreach (var line in WriteLines())
                {
                    writer.WriteLine(line);
                }
            }
        }

        public void Load()
        {
            using (var reader = new StreamReader(FilePath))
            {
                var lines = GetLines(reader);
                ReadLines(lines);
            }
        }

        protected abstract IEnumerable<string> WriteLines();
        protected abstract void ReadLines(IEnumerable<string> lines);

        private IEnumerable<string> GetLines(StreamReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
