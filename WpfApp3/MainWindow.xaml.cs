﻿using Logic;
using OMDbApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;


namespace MainProgramUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TMDbClient client;
        private List<Logic.Video> m_MoviesFound = new List<Logic.Video>();

        public MainWindow()
        {
            InitializeComponent();
            client = new TMDbClient("a959178bb3475c959db8941953d19bad");
        }

        private void UpdateMoviesAndSeriesListBox()
        {
            try
            {
                for (int i = 0; i < getDataListBox.Items.Count; i++)
                {
                    string fullFileName = getDataListBox.Items[i].ToString();
                    SearchContainer<SearchMovie> results  = client.SearchMovieAsync(fullFileName).Result;

                    if (results.TotalResults > 0)
                    {
                        SearchMovie result = results.Results[0];

                        if (result.MediaType == MediaType.Movie)
                        {
                            foundAMovie(result.Id, (FileInfo)getDataListBox.Items[i]);
                        }
                    }

                    else
                    {
                       if(!forceSearch(fullFileName, (FileInfo)getDataListBox.Items[i],results))
                        {
                            getDataListBox.Items[i] += " : NOT FOUND!!! try changing the file name";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool forceSearch(string i_FullFileName, FileInfo i_CurrentItem, SearchContainer<SearchMovie> i_Results)
        {
            bool found = false;

            for (int j = i_FullFileName.Length - 1; j >= 0; j--)
            {
                if (char.IsSeparator(i_FullFileName[j]))
                {
                    i_FullFileName = i_FullFileName.Substring(0, j);
                    i_Results = client.SearchMovieAsync(i_FullFileName).Result;

                    if (i_Results.TotalResults > 0)
                    {
                        SearchMovie result = i_Results.Results[0];

                        if (result.MediaType == MediaType.Movie)
                        {
                            foundAMovie(result.Id, i_CurrentItem);
                        }
                        found = true;
                        break;
                    }

                }
            }

            return found;
        }

        private void foundAMovie(int i_MovieId,FileInfo path)
        {
            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(i_MovieId).Result;

            StringBuilder genres = new StringBuilder("");

            foreach (var item in movie.Genres)
            {
                genres.Append(item.Name + ", ");
            }

            DateTime ReleaseYear = (DateTime)movie.ReleaseDate;
            string imagePosterPath = string.Format("https://image.tmdb.org/t/p/original{0}", movie.PosterPath);
          
            Movie title = new Movie(movie.Title, genres.ToString(), movie.VoteAverage, ReleaseYear.Year, imagePosterPath, movie.ImdbId,path.ImagePath);

            if (!m_MoviesFound.Any(n => n.Title == title.Title))
            {
                m_MoviesFound.Add(title);
                MoviesListBox.Items.Add(title);

            }
        }
     
        private void GetMoviesFromPc_Click(object sender, RoutedEventArgs e)
        {
            bool toUpdate = false;

            try
            {        
                foreach (FileInfo item in Utilities.GetMoviesFromPc())
                {
                    if (!getDataListBox.Items.Cast<FileInfo>().Any(x => x.Title == item.Title))
                    {
                        getDataListBox.Items.Add(item);
                        toUpdate = true;
                    }
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            if (toUpdate)
            {
               UpdateMoviesAndSeriesListBox();
            }

        }

        private void SortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

          ComboBoxItem select = (ComboBoxItem)SortTypeComboBox.SelectedValue;

            if((string)select.Content == "By Year")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.ReleasedYear).ToList();
            }

            if ((string)select.Content == "By Rating")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.Rating).ToList();
            }

            if ((string)select.Content == "By Genre")
            {
                m_MoviesFound = m_MoviesFound.OrderBy(w => w.Genre).ToList();
            }

            MoviesListBox.Items.Clear();

            foreach (var item in m_MoviesFound)
            {
                MoviesListBox.Items.Add(item);
            }

        }

        private void ClearListBtn_Click(object sender, RoutedEventArgs e)
        {
            getDataListBox.Items.Clear();
        }

        private void MoviesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (MoviesListBox.SelectedItem != null)
            {
                string url = (MoviesListBox.SelectedItem as Movie).ImagePath;
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(url);
                bi.EndInit();
                CoverImage.Source = bi;
            }
        }

        private void ImdbBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                string imdbSite = string.Format("https://www.imdb.com/title/{0}", (MoviesListBox.SelectedItem as Movie).ImdbId);
                System.Diagnostics.Process.Start(imdbSite);
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesListBox.SelectedItem != null)
            {
                System.Diagnostics.Process.Start((MoviesListBox.SelectedItem as Movie).FilePath);
            }
        }
    }
}

