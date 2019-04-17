using Ookii.Dialogs.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using System.Collections.Generic;
using System.Net;
using OMDbApiNet;
using Logic;
using OMDbApiNet.Model;
using System.Text.RegularExpressions;
using TMDbLib.Client;

namespace MainProgramUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OmdbClient omdb;
        TMDbClient client;
        public MainWindow()
        {
            omdb = new OmdbClient("fdeaf500");
            client = new TMDbClient("APIKey");

            InitializeComponent();
        }

        private void UpdateMoviesAndSeriesListBox()
        {

            foreach (string name in getDataListBox.Items)
            {
                Item item = null;

                try
                {
                    item = omdb.GetItemByTitle(name.ToString());
                }
                finally
                {

                    Video title = null;

                    if (item.Type == "movie")
                    {
                        title = new Movie(item.Title, item.Genre, item.ImdbRating, item.Year);
                        MoviesListBox.Items.Add(title);
                    }

                    else if (item.Type == "series")
                    {

                        title = new Series(item.Title, item.Genre, item.ImdbRating, item.Year);
                        SeriesListBox.Items.Add(title);
                    }

                }

            }


            
        }


        private void GetMoviesFromPc_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog folderBrowser = new VistaFolderBrowserDialog();
            folderBrowser.ShowDialog();

            string choosenPath = folderBrowser.SelectedPath;

            try
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
                        string fileName = Path.GetFileNameWithoutExtension(filePath);
                        string s = trimMe(fileName);
                        getDataListBox.Items.Add(s);

                    }

                }


            }
            catch (System.Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            UpdateMoviesAndSeriesListBox();

        }

        private string trimMe(string i_FileName)
        {

                string strEnd1080 = "1080";
                string strEnd720 = "720";

                int Start, End;

                if ( i_FileName.Contains(strEnd1080))
                {
                    Start = 0;
                    End = i_FileName.IndexOf(strEnd1080, Start);
                    i_FileName = i_FileName.Substring(Start, End - Start);
                }

                else if( i_FileName.Contains(strEnd720))
                {
                    Start = 0;
                    End = i_FileName.IndexOf(strEnd720, Start);
                    i_FileName = i_FileName.Substring(Start, End - Start);
                }

                i_FileName = i_FileName.Replace(".", " ");



            i_FileName = RemoveDigits(i_FileName);
            return i_FileName;



        }

        public static string RemoveDigits(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        private void ClearListBtn_Click(object sender, RoutedEventArgs e)
        {
            getDataListBox.Items.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
      

            
        }
    }
}
