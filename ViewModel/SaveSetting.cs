using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ViewModel
{
    public class SaveSetting
    {
        public ObservableCollection<Movie> MoviesFound { get; set; }
        public ObservableCollection<FileInfo> StoredFilesInPc { get; set; }


        public SaveSetting(ObservableCollection<Movie> i_MoviesFound, ObservableCollection<FileInfo> i_StoredFilesInPc)
        {
            MoviesFound = i_MoviesFound;
            StoredFilesInPc = i_StoredFilesInPc;
        }

        public void Save()
        {
            saveMoviesFound();
            saveFilesFound();

        }

        private void saveMoviesFound()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Movie>));
            using (StreamWriter wr = new StreamWriter(@"C:\MoviesFound.xml"))
            {
                xs.Serialize(wr, MoviesFound);
            }
        }

        private void saveFilesFound()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<FileInfo>));
            using (StreamWriter wr = new StreamWriter(@"C:\FilesFound.xml"))
            {
                xs.Serialize(wr, StoredFilesInPc);
            }
        }

        public bool LoadMovies()
        {
            bool fileFound = false;

            if (File.Exists(@"C:\MoviesFound.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Movie>));
                using (StreamReader rd = new StreamReader(@"C:\MoviesFound.xml"))
                {
                    MoviesFound = xs.Deserialize(rd) as ObservableCollection<Movie>;
                }

                fileFound = true;

            }

            return fileFound;
        }


        public bool LoadFiles()
        {
            bool fileFound = false;

            if (File.Exists(@"C:\FilesFound.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<FileInfo>));
                using (StreamReader rd = new StreamReader(@"C:\FilesFound.xml"))
                {
                    StoredFilesInPc = xs.Deserialize(rd) as ObservableCollection<FileInfo>;
                }

                fileFound = true;

            }

            return fileFound;
        }


    }
}
