using System;
using System.IO;
using TagIt.WPF.Models;
using TagIt.WPF.Models.Tags;
using TagIt.WPF.Models.Thumbnails;

namespace TagIt.WPF.Helpers
{
    public static class DirectoryHelper
    {
        public static void DeleteDirectory(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                fileInfo.Delete();
            }

            Directory.Delete(directoryPath);
        }
    }
}
