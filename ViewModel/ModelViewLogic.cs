using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace ViewModel
{
    public class ModelViewLogic
    {
        public List<Logic.Video> MoviesFound { get; set; }
        public List<FileInfo> StoredFilesInPc { get; set; }
        private TMDbClient m_Client;

        public ModelViewLogic()
        {
            m_Client = new TMDbClient("a959178bb3475c959db8941953d19bad");
            MoviesFound = new List<Logic.Video>();
            StoredFilesInPc = new List<FileInfo>();
        }
        public bool getMoviesFromPc()
        {
            bool toUpdate = false;

            try
            {
                foreach (FileInfo item in Utilities.GetMoviesFromPc())
                {
                    if (!StoredFilesInPc.Any(x => x.FileName == item.FileName))
                    {
                        StoredFilesInPc.Add(item);
                        toUpdate = true;
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return toUpdate;
        }
        public void lalala()
        {
            try
            {
                for (int i = 0; i < StoredFilesInPc.Count; i++)
                {
                    string fullFileName = StoredFilesInPc[i].ToString();
                    SearchContainer<SearchMovie> results = m_Client.SearchMovieAsync(fullFileName).Result;

                    if (results.TotalResults > 0)
                    {
                        SearchMovie result = results.Results[0];

                        if (result.MediaType == MediaType.Movie)
                        {
                            foundAMovie(result.Id, StoredFilesInPc[i]);
                        }
                    }

                    else
                    {
                        if (!forceSearch(fullFileName, (FileInfo)StoredFilesInPc[i], results))
                        {
                            StoredFilesInPc[i].FileName += " : NOT FOUND!!! try changing the file name";
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                    i_Results = m_Client.SearchMovieAsync(i_FullFileName).Result;

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
        private void foundAMovie(int i_MovieId, FileInfo path)
        {
            Movie movie = new Movie();
            movie.ApiMovie = m_Client.GetMovieAsync(i_MovieId).Result;
            movie.InitializeClass();
            movie.FilePath = path.FilePath;

            if (!MoviesFound.Any(n => n.Title == movie.Title))
            {
                MoviesFound.Add(movie);
                //MoviesListBox.Items.Add(movie);
            }
        }

        public void sortType(string i_SelectedValue)
        {

            if (i_SelectedValue == "By Year")
            {
                MoviesFound = MoviesFound.OrderByDescending(w => w.ReleasedYear).ToList();
            }

            if (i_SelectedValue == "By Rating")
            {
                MoviesFound = MoviesFound.OrderByDescending(w => w.Rating).ToList();
            }

            if (i_SelectedValue ==  "By Genre")
            {
                MoviesFound = MoviesFound.OrderBy(w => w.Genre).ToList();
            }

        }


    }
}
