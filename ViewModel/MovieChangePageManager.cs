using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace ViewModel
{
    public class MovieChangePageManager
    {
        public int PageNumber { get; set; }
        public ObservableCollection<Movie> reSearchMoviesFound { get; set; }
        public TMDbClient m_Client { get; set; }

        public MovieChangePageManager(TMDbClient i_Client/*int i_PageNumber, ObservableCollection<Movie> i_reSearchMoviesFound, TMDbClient i_Client*/)
        {
            PageNumber = 0;
            reSearchMoviesFound = new ObservableCollection<Movie>();
            m_Client = i_Client;

        }

        public void MoiveReSearchResults(Video i_IncorrentMovie, int i_currentPage) // i_currentPage = 0,3,6,9,12;
        {


            if (PageNumber < 0)
            {
                PageNumber = 0;
            }
            reSearchMoviesFound.Clear();
            string fileName = i_IncorrentMovie.CuttedFileName;
            SearchContainer<SearchMovie> results = m_Client.SearchMovieAsync(fileName).Result;
            byte numOfResultsWanted = 3;

            for (int j = fileName.Length - 1; j >= 0; j--)
            {
                if (char.IsSeparator(fileName[j]))
                {
                    fileName = fileName.Substring(0, j);
                    results = m_Client.SearchMovieAsync(fileName).Result;

                    if (results.TotalResults / numOfResultsWanted >= 1) // at least 3 results 
                    {
                        int perPage = i_currentPage * numOfResultsWanted + numOfResultsWanted;
                        int index = i_currentPage * numOfResultsWanted;
                        while (index < results.TotalResults && index < perPage)
                        {
                            if (results.Results[index].MediaType == MediaType.Movie)
                            {
                                createMovieInstance(results.Results[index].Id, i_IncorrentMovie, reSearchMoviesFound);
                            }

                            index++;
                        }

                        break;

                    }

                    else // if less than 3 results, take all and that's it.
                    {
                        foreach (var item in results.Results)
                        {
                            if (item.MediaType == MediaType.Movie)
                            {
                                createMovieInstance(item.Id, i_IncorrentMovie, reSearchMoviesFound);
                            }
                        }

                    }
                    break;
                }
            }
        }

        private void createMovieInstance(int i_results, Video i_IncorrentMovie, ObservableCollection<Movie> i_reSearchMoviesFound)
        {
            Movie movie = new Movie();
            movie.ApiMovie = m_Client.GetMovieAsync(i_results).Result;
            movie.InitializeClass();
            movie.FilePath = i_IncorrentMovie.FilePath;
            movie.CuttedFileName = i_IncorrentMovie.CuttedFileName;
            i_reSearchMoviesFound.Add(movie);
        }

    }
}
