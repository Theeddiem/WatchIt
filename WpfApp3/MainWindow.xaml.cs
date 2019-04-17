using Logic;
using OMDbApiNet;
using System;
using System.Windows;
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

        TMDbClient client;
        public MainWindow()
        {
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
            try
            {
       
                foreach (var item in Utilities.GetMoviesFromPc())
                {
                    getDataListBox.Items.Add(item);
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            UpdateMoviesAndSeriesListBox();

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
