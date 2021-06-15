using System;

namespace TagIt.Droid.Helpers
{
    public static class FilePathHelper
    {
        internal static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        //public static string TAG_LIBRARY_PATH = SOLUTION_DIRECTORY + @"\TagIt.WPF\TagManifest" + TagManifest.FILE_EXTENSION;
        //public static string THUMBNAIL_LIBRARY_DIRECTORY = SOLUTION_DIRECTORY + @"\TagIt.WPF\ThumbnailLibraries";
        //public static string THUMBNAIL_CACHE_DIRECTORY = @"D:\GitHub\TagIt\ThumbnailLibraries\Cache";//@"C:\Users\Takaji\source\repos\TagIt.WPF\TagIt.WPF\ThumbnailLibraries\Temp";

        public static string INITIAL_LOCAL_DIRECTORY = Android.App.Application.Context.GetExternalFilesDir(string.Empty).AbsolutePath;
        public static string CACHE_DIRECTORY = Android.App.Application.Context.GetExternalFilesDir("Cache").AbsolutePath;
    }
}
