using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Path = System.IO.Path;

namespace Logic
{
    public static class Utilities
    {

       public static List<FileInfo> GetMoviesFromPc()
        {
            List<FileInfo> titles = new List<FileInfo>();

            VistaFolderBrowserDialog folderBrowser = new VistaFolderBrowserDialog();
            folderBrowser.ShowDialog();

                string choosenPath = folderBrowser.SelectedPath;

                if (choosenPath != "")
                {
                    List<string> dirs = (Directory.GetDirectories(choosenPath, "*", SearchOption.AllDirectories)).ToList();

                    dirs.Add(choosenPath);

                    foreach (string dir in dirs)
                    {
                    var allowedExtensions = new[] { ".mp4", ".avi", ".mkv" };
                    var files = Directory
                        .GetFiles(dir)
                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                        .ToList();




                    foreach (string filePath in files)
                        {
                            string fileName = Path.GetFileName(filePath);
                            string finalFileName = trimMe(fileName);
                            FileInfo newTitle = new FileInfo(filePath, finalFileName);
                            titles.Add(newTitle);
                        }

                    }
                }

        return titles;

       }

        public static string trimMe(string i_FileName)
        {
            string strEnd1080 = "1080";
            string strEnd720 = "720";
            string strEnd = ".";

            int Start, End;

            if (i_FileName.Contains(strEnd720))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd, Start);
                i_FileName = i_FileName.Substring(Start, End - Start);
            }

            else if (i_FileName.Contains(strEnd1080))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd1080, Start);
                i_FileName = i_FileName.Substring(Start, End - Start);
            }

            i_FileName = i_FileName.Replace(".", " ");

            i_FileName = RemoveDigits(i_FileName);
            return i_FileName;
        }

        private static string RemoveDigits(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }
    }
}
