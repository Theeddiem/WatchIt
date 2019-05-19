using ModelLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace ViewModel
{
    public delegate void CurrentActionMade();

    public class ViewModelGlue
    {

        public ObservableCollection<Movie> MoviesFound { get; set; }
        public ObservableCollection<FileInfo> StoredFilesInPc { get; set; }
        public SaveSetting CurrentSettings { get; }
        private TMDbClient m_Client;
        public MovieChangePageManager MovieChangePageManager { get; set; }


        public ViewModelGlue()
        {
            m_Client = new TMDbClient("a959178bb3475c959db8941953d19bad");
            MoviesFound = new ObservableCollection<Movie>();
            StoredFilesInPc = new ObservableCollection<FileInfo>();
            CurrentSettings = new SaveSetting(MoviesFound,StoredFilesInPc);
            MovieChangePageManager = new MovieChangePageManager(m_Client);
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (CurrentSettings.LoadMovies())
            {
                MoviesFound = CurrentSettings.MoviesFound;
              //  new Thread(LoadImageForEachMovie).Start();
               LoadImageForEachMovie();
            }


            if (CurrentSettings.LoadFiles())
            {
                StoredFilesInPc = CurrentSettings.StoredFilesInPc;
            }
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

        public void ChangeMovies(Movie i_IncorrentMovie,Movie i_CorrectMovie)
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

        public void SortMovies(string i_SelectedValue)
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
                    filterd = filterd.OrderBy(w => w.Genre).ToList();

                }

                MoviesFound.Clear();

                foreach (var item in filterd)
                {
                    MoviesFound.Add(item);
                }
            }

        }

        public bool OpenFolder(Video i_Video)
        {

            bool execute = false;

            if (checkIfSelected(i_Video))
            {
                string folderPath = Path.GetDirectoryName(i_Video.FilePath);
                if (Directory.Exists(folderPath))
                {
                    System.Diagnostics.Process.Start(folderPath);
                }
                execute = true;
            }

            return execute;
        }

        public bool PlayFile(Video i_Video)
        {
            bool execute = false;

            if (checkIfSelected(i_Video))
            {
                string filePath = i_Video.FilePath;
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(filePath);
                }
                execute = true;
            }

            return execute;
        }

        public bool OpenImdbSite(Video i_Video)
        {
            bool execute = false;

            if(checkIfSelected(i_Video))
            {
                string imdbSite = string.Format("https://www.imdb.com/title/{0}", i_Video.ImdbId);
                System.Diagnostics.Process.Start(imdbSite);
                execute = true;
            }

            return execute; 

        }

        public bool checkIfSelected(Video i_Video)
        {
            bool selected = false; 

            if(i_Video != null)
            {
                selected = true;
            }

            return selected;
        }
    }
}
