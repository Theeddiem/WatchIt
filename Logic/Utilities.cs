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
       public static List<string> GetMoviesFromPc()
        {
            VistaFolderBrowserDialog folderBrowser = new VistaFolderBrowserDialog();
            folderBrowser.ShowDialog();

            string choosenPath = folderBrowser.SelectedPath;

            List<string> titles = new List<string>();

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
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string s = trimMe(fileName);
                        titles.Add(s);

                    }

                }
                return titles;
        }

        public static string trimMe(string i_FileName)
        {
            string strEnd1080 = "1080";
            string strEnd720 = "720";

            int Start, End;

            if (i_FileName.Contains(strEnd1080))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd1080, Start);
                i_FileName = i_FileName.Substring(Start, End - Start);
            }

            else if (i_FileName.Contains(strEnd720))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd720, Start);
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
