using Logic;
using OMDbApiNet;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using TMDbLib.Client;
using TMDbLib.Objects.General;

using TMDbLib.Objects.Search;
using Path = System.IO.Path;

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
            client = new TMDbClient("a959178bb3475c959db8941953d19bad");

            InitializeComponent();
        }

        private void UpdateMoviesAndSeriesListBox()
        {
            try
            {
                for (int i = 0; i < getDataListBox.Items.Count; i++)
                {
                    SearchContainer<SearchMovie> results = client.SearchMovieAsync(getDataListBox.Items[i].ToString()).Result;

                    if (results.TotalResults > 0 )
                    {
                        SearchMovie result = results.Results[0];

                        Logic.Video title = null; ;

                        if (result.MediaType == MediaType.Movie)
                        {
                            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(result.Id).Result;

                            string genres = "";
                            foreach (var item in movie.Genres)
                            {
                                genres += (item.Name + ", ");
                            }

                            double S = movie.VoteAverage;

                            DateTime ReleaseYear = (DateTime)movie.ReleaseDate;

                            title = new Movie(movie.Title, genres, movie.VoteAverage, ReleaseYear.Year);
                            MoviesListBox.Items.Add(title);
                        }
                    }

                    else
                    {
                        getDataListBox.Items[i] += "Not found try changeing the file name";

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            catch (Exception ex)
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
