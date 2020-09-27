using System;
using TagIt.Shared.Models.Tags;
using TagIt.WPF.Models;

namespace TagIt.WPF.Helpers
{
    public static class FilePathHelper
    {
        internal static readonly string SOLUTION_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";

        public static string SETTINGS_PATH = SOLUTION_DIRECTORY + @"\TagIt.WPF\TagItSettings" + ProgramSettings.FILE_EXTENSION;
        public static string TAG_LIBRARY_PATH = SOLUTION_DIRECTORY + @"\TagIt.WPF\TagLibrary" + TagLibrary.FILE_EXTENSION;
        public static string THUMBNAIL_LIBRARY_DIRECTORY = SOLUTION_DIRECTORY + @"\TagIt.WPF\ThumbnailLibraries";
        public static string THUMBNAIL_CACHE_DIRECTORY = @"D:\GitHub\TagIt\ThumbnailLibraries\Cache";//@"C:\Users\Takaji\source\repos\TagIt.WPF\TagIt.WPF\ThumbnailLibraries\Temp";

        public static string INITIAL_LOCAL_DIRECTORY = @"D:\USBs\Blue";

        public static string FFMPEG_PATH = @"C:\Users\Takaji\Downloads\ffmpeg-20200113-7225479-win64-static\ffmpeg-20200113-7225479-win64-static\bin\ffmpeg.exe";
        //public static string TEST_THUMBNAIL_PATH = @"C:\Users\Takaji\source\repos\TagIt.WPF\TagIt.WPF\test_thumbnail";
    }
}
