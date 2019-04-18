using Logic;
using OMDbApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    SearchContainer<SearchMovie> results = client.SearchMovieAsync(getDataListBox.Items[i].ToString()).Result;

                    if (results.TotalResults > 0 )
                    {
                        SearchMovie result = results.Results[0];

                        if (result.MediaType == MediaType.Movie)
                        {

                             foundMovie(result.Id, (FileInfo)getDataListBox.Items[i]);
                        }
                    }

                    else
                    {
                        getDataListBox.Items[i] += " : NOT FOUND!!! try changing the file name";
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void foundMovie(int i_MovieId,FileInfo path)
        {
            TMDbLib.Objects.Movies.Movie movie = client.GetMovieAsync(i_MovieId).Result;

            string genres = "";
            foreach (var item in movie.Genres)
            {
                genres += (item.Name + ", ");
            }

            DateTime ReleaseYear = (DateTime)movie.ReleaseDate;
            string imagePosterPath = string.Format("https://image.tmdb.org/t/p/original{0}", movie.PosterPath);
          
            Logic.Movie title = new Movie(movie.Title, genres, movie.VoteAverage, ReleaseYear.Year, imagePosterPath, movie.ImdbId,path.ImagePath);

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

        private void SortTypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

          ComboBoxItem s = (ComboBoxItem)SortTypeComboBox.SelectedValue;

            if((string)s.Content == "By Year")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.ReleasedYear).ToList();
            }

            if ((string)s.Content == "By Rating")
            {
                m_MoviesFound = m_MoviesFound.OrderByDescending(w => w.Rating).ToList();
            }

            if ((string)s.Content == "By Genre")
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

        private void Button_Click(object sender, RoutedEventArgs e)
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

