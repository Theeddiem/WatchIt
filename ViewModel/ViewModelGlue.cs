using ModelLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace ViewModel
{
    public class ViewModelGlue
    {

        public ObservableCollection<Movie> MoviesFound { get; set; }
        public ObservableCollection<FileInfo> StoredFilesInPc { get; set; }
        public ObservableCollection<Movie> reSearchMoviesFound  { get; set; }
        public SaveSetting CurrentSettings { get; }
        private TMDbClient m_Client;

        public int PageNumber { get; set; }

        public ViewModelGlue()
        {
            m_Client = new TMDbClient("a959178bb3475c959db8941953d19bad");
            MoviesFound = new ObservableCollection<Movie>();
            StoredFilesInPc = new ObservableCollection<FileInfo>();
            CurrentSettings = new SaveSetting(MoviesFound,StoredFilesInPc);
            reSearchMoviesFound = new ObservableCollection<Movie>();
            PageNumber = 0;
        }

        public void LoadImageForEachMovie()
        {
            foreach (var item in MoviesFound)
            {
                item.initializeImageCoverImage();
            }
        }

        public void GetMoviesFromPc()
        {
            bool toUpdate = false;

            List<FileInfo> filesFound = new List<FileInfo>();
            List<FileInfo> newFilesFound = new List<FileInfo>();

            try
            {
                foreach (string filePath in Utilities.GetFilePaths())
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string finalFileName = Utilities.TrimFileName(fileName);
                    FileInfo newFile = new FileInfo(filePath, finalFileName);
                    filesFound.Add(newFile);
                }

                foreach (FileInfo item in filesFound)
                {
                    if (!StoredFilesInPc.Any(x => x.FileName == item.FileName))
                    {
                        StoredFilesInPc.Add(item);
                        newFilesFound.Add(item);
                        toUpdate = true;
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if(toUpdate)
            {
                GetVideoData(newFilesFound);
            }

        }

        public void GetVideoData(List<FileInfo> i_NewFilesFound)
        {
            try
            {
                for (int i = 0; i < i_NewFilesFound.Count; i++)
                {
                    string fullFileName = i_NewFilesFound[i].ToString();
                    SearchContainer<SearchMovie> results = m_Client.SearchMovieAsync(fullFileName).Result;

                    if (results.TotalResults > 0)
                    {
                        SearchMovie result = results.Results[0];

                        if (result.MediaType == MediaType.Movie)
                        {
                            foundAMovie(result.Id, i_NewFilesFound[i]);
                        }
                    }

                    else
                    {
                        if (!forceSearch(fullFileName, i_NewFilesFound[i], results))
                        {
                            i_NewFilesFound[i].FileName += " : NOT FOUND!!! try changing the file name";
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

        public void MoviesResults(Movie i_IncorrentMovie, int i_currentPage) // i_currentPage = 0,3,6,9,12;
        {

            if (PageNumber<0)
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

        private void createMovieInstance (int i_results,Movie i_IncorrentMovie, ObservableCollection<Movie> i_reSearchMoviesFound)
        {
            Movie movie = new Movie();
            movie.ApiMovie = m_Client.GetMovieAsync(i_results).Result;
            movie.InitializeClass();
            movie.FilePath = i_IncorrentMovie.FilePath;
            movie.CuttedFileName = i_IncorrentMovie.CuttedFileName;
            i_reSearchMoviesFound.Add(movie);
        }

        public void changeRefMovies(Movie i_IncorrentMovie,Movie i_CorrectMovie)
        {
            for (int i = 0; i < MoviesFound.Count; i++)
            {
                if (MoviesFound[i].Title == i_IncorrentMovie.Title)
                {
                    MoviesFound[i] = i_CorrectMovie;
                    break;
                }
            }
        }

        private void foundAMovie(int i_MovieId, FileInfo i_Path)
        {
            Movie movie = new Movie();
            movie.ApiMovie = m_Client.GetMovieAsync(i_MovieId).Result;
            movie.InitializeClass();
            movie.FilePath = i_Path.FilePath;
            movie.CuttedFileName = i_Path.FileName;

            if (!MoviesFound.Any(n => n.Title == movie.Title))
            {
                MoviesFound.Add(movie);
            }
        }

        public void sortType(string i_SelectedValue)
        {
            if (MoviesFound.Count > 1)
            {
                var filterd = MoviesFound.ToList();

                if (i_SelectedValue == "By Year")
                {
                    filterd = filterd.OrderByDescending(w => w.ReleasedYear).ToList();
                }

                if (i_SelectedValue == "By Rating")
                {
                    filterd = filterd.OrderByDescending(w => w.Rating).ToList();
                }

                if (i_SelectedValue == "By Genre")
                {
                    filterd = filterd.OrderByDescending(w => w.Genre).ToList();

                }

                MoviesFound.Clear();

                foreach (var item in filterd)
                {
                    MoviesFound.Add(item);
                }
            }

        }

        /// <summary>
        /// button on Ui
        /// </summary>

        public void OpenFolder(Video i_Video)
        {
            string folderPath = Path.GetDirectoryName(i_Video.FilePath);
            if (Directory.Exists(folderPath))
            {
                System.Diagnostics.Process.Start(folderPath);
            }
        }

        public void PlayFile(Video i_Video)
        {
            string filePath = i_Video.FilePath;
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(filePath);
            }
        }

        public void OpenImdbSite(Video i_Video)
        {
            string imdbSite = string.Format("https://www.imdb.com/title/{0}", i_Video.ImdbId);
            System.Diagnostics.Process.Start(imdbSite);
        }

    }
}
