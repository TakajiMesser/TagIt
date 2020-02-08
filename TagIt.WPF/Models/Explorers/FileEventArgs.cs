namespace TagIt.WPF.Models.Explorers
{
    public class FileEventArgs
    {
        public string Name { get; }
        public string Path { get; }

        public FileEventArgs(string filePath)
        {
            Name = System.IO.Path.GetFileName(filePath);
            Path = filePath;
        }
    }
}
