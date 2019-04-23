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
        public SaveSetting CurrentSettings { get; }
        private TMDbClient m_Client;


        public ViewModelGlue()
        {
            m_Client = new TMDbClient("a959178bb3475c959db8941953d19bad");
            MoviesFound = new ObservableCollection<Movie>();
            StoredFilesInPc = new ObservableCollection<FileInfo>();
            CurrentSettings = new SaveSetting(MoviesFound,StoredFilesInPc);
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
                GetVideoData();
            }

        }

        public void GetVideoData()
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
                        if (!forceSearch(fullFileName, StoredFilesInPc[i], results))
                        {
                            StoredFilesInPc[i].FileName += " : NOT FOUND!!! try changing the file name";
                        }
                    }
                }

                CurrentSettings.Save();
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
 //delegete to do
            return found;
        }

        private void foundAMovie(int i_MovieId, FileInfo i_Path)
        {
            Movie movie = new Movie();
            movie.ApiMovie = m_Client.GetMovieAsync(i_MovieId).Result;
            movie.InitializeClass();
            movie.FilePath = i_Path.FilePath;

            if (!MoviesFound.Any(n => n.Title == movie.Title))
            {
                MoviesFound.Add(movie);
                //MoviesListBox.Items.Add(movie);
            }
        }

        public void sortType(string i_SelectedValue)
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
}
