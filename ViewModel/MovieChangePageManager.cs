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
        public int PageNumber { get; set; } = 0;
        public ObservableCollection<Movie> reSearchMoviesFound { get; set; }
        public TMDbClient m_Client { get; set; }
        public byte NumOfResultsWanted { get; set; } = 3;

        public MovieChangePageManager(TMDbClient i_Client)
        {
            reSearchMoviesFound = new ObservableCollection<Movie>();
            m_Client = i_Client;
        }

        public void NextPage(Video i_IncorrentMovie)
        {
            PageNumber++;
            MoiveReSearchResults(i_IncorrentMovie);
        }

        public void PreviousPage(Video i_IncorrentMovie)
        {
            if (PageNumber != 0) // makeing sure I won't go under 0 for pageing
            {
                PageNumber--;
                MoiveReSearchResults(i_IncorrentMovie);
            }
        }

        public void MoiveReSearchResults(Video i_IncorrentMovie) 
        {
                reSearchMoviesFound.Clear();
                string fileName = i_IncorrentMovie.CuttedFileName;
                SearchContainer<SearchMovie> results = m_Client.SearchMovieAsync(fileName).Result;

                for (int j = fileName.Length - 1; j >= 0; j--)
                {
                    if (char.IsSeparator(fileName[j]))
                    {
                        fileName = fileName.Substring(0, j);
                        results = m_Client.SearchMovieAsync(fileName).Result;

                        if (results.TotalResults / NumOfResultsWanted >= 1) // at least 3 results 
                        {
                            int perPage = PageNumber * NumOfResultsWanted + NumOfResultsWanted;
                            int index = PageNumber * NumOfResultsWanted;
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
