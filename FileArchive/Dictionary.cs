using System;
using System.Collections.Generic;
using System.Text;

namespace FileArchive
{
    public class Extensions
    {
        public static List<string> Image = new List<string>{ "png", "jpeg", "jpg", "bmp", "tiff", "gif", "ppm", "pgm", "pbm", "pnm", "cgm", "svg", "webp" };
        public static List<string> Video = new List<string> { "mp4", "3gp", "ogg", "wmv", "webm", "flv", "avi", "mkv", "mpeg" };
        public static List<string> Sound = new List<string> { "mp3", "aac", "ac3", "eac3", "wma", "pcm" };
        public static List<string> Document = new List<string> { "pdf", "txt", "xlsx", "xls", "docx", "doc", "html", "htm", "odt", "ods", "ppt", "pptx" };
    }

    public class Folders
    {
        public static string ImagesDestination = @"D:\Downloads\Images";
        public static string VideosDestination = @"D:\Downloads\Videos";
        public static string DocumentsDestination = @"D:\Downloads\Documents";
        public static string SoundDestination = @"D:\Downloads\Sound";
        public static string RandomDestination = @"D:\Downloads\Random";
    }
}
