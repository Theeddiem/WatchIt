using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logic
{
    public static class Utilities
    {
       public static List<string> GetFilePaths()
       {

            List<string> filePaths = new List<string>();
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
                                filePaths.Add(filePath);
                            }

                        }
                }

          return filePaths;
       }
       public static string TrimFileName(string i_FileName)
        {
            string strEnd1080 = "1080";
            string strEnd720 = "720";

            int Start, End;

            if (i_FileName.Contains(strEnd720))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd720, Start);
                i_FileName = i_FileName.Substring(Start, End - Start);
            }

            else if (i_FileName.Contains(strEnd1080))
            {
                Start = 0;
                End = i_FileName.IndexOf(strEnd1080, Start);
                i_FileName = i_FileName.Substring(Start, End - Start);
            }

            i_FileName = i_FileName.Replace(".", " ");

            i_FileName = removeLastSpaces(i_FileName);
            return i_FileName;
        }
       private static string removeLastSpaces(string i_Key)
        {
            for (int i = i_Key.Length-1; i >= 0; i--)
            {
                if (char.IsSeparator(i_Key[i]))
                {
                    i_Key = i_Key.TrimEnd();
                    i--;
                }

                else
                    break;
            }

            return i_Key;
        }
    }
}
