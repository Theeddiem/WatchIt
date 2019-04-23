using ModelLogic;
using System;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace ViewModel
{

    public class Movie : Video
    {

        [XmlIgnore]
        public TMDbLib.Objects.Movies.Movie ApiMovie { get; set; }

        public Movie()
        {
   
        }
        public void InitializeClass()
        {
            if (ApiMovie != null)
            {
                Title = initializeTitle();
                Genre = initializeGenres();
                Rating = initializeRating() ;
                ReleasedYear = initializeReleasedYear();
                ImagePathUrl = initializeImagePathUrl();
                ImdbId = initializeIMDbId();
                CoverImage = initializeImageCoverImage();
            }
        }


        //public BitmapSource getImage()
        //{
        //    return Convert.ToBase64String()
        //}

        //private string initializeTheImage()
        //{
        //    return Utilities.ImageToBase64(CoverImage);
        //}

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", Title, ReleasedYear, Rating, Genre);
        }

        private string initializeTitle()
        {
            return ApiMovie.Title;
        }

        private double initializeRating()
        {
            return ApiMovie.VoteAverage;
        }

        private string initializeGenres()
        {
            StringBuilder genres = new StringBuilder("");

            foreach (var item in ApiMovie.Genres)
            {
                genres.Append(item.Name + ", ");
            }

            string temp = genres.ToString();
            temp = temp.TrimEnd(',',' ');

            return temp;
        }

        private string initializeIMDbId()
        {
            return ApiMovie.ImdbId;
        }

        private int initializeReleasedYear()
        {
            DateTime ReleaseYear = (DateTime)ApiMovie.ReleaseDate;

            return ReleaseYear.Year;
        }

        private string initializeImagePathUrl()
        {
            return string.Format("https://image.tmdb.org/t/p/original{0}", ApiMovie.PosterPath);
        }

        public BitmapImage initializeImageCoverImage()
        {
            string url = this.ImagePathUrl;
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(url);
            bi.EndInit();

            CoverImage = bi;

            return bi;
        }

    }
}
