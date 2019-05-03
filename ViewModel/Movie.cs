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
                 initializeTitle();
                 initializeGenres();
                 initializeRating() ;
                 initializeReleasedYear();
                 initializeImagePathUrl();
                 initializeIMDbId();
                 initializeImageCoverImage();
                 initializeOverview();
               
            }
        }

        private void initializeOverview()
        {
            Overview =  ApiMovie.Overview;
        }

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", Title, ReleasedYear, Rating, Genre);
        }

        private void initializeTitle()
        {
            Title = ApiMovie.Title;
        }

        private void initializeRating()
        {
            Rating = ApiMovie.VoteAverage;
        }

        private void initializeGenres()
        {
            StringBuilder genres = new StringBuilder("");

            foreach (var item in ApiMovie.Genres)
            {
                genres.Append(item.Name + ", ");
            }

            string temp = genres.ToString();
            temp = temp.TrimEnd(',',' ');

            Genre = temp;
        }

        private void initializeIMDbId()
        {
            ImdbId = ApiMovie.ImdbId;
        }

        private void initializeReleasedYear()
        {

            DateTime dateYear = new DateTime(9999, 9, 9);
            if(ApiMovie.ReleaseDate != null) 
              dateYear = (DateTime)ApiMovie.ReleaseDate;



            ReleasedYear = dateYear.Year;
        }

        private void initializeImagePathUrl()
        {
            ImagePathUrl = string.Format("https://image.tmdb.org/t/p/original{0}", ApiMovie.PosterPath);
        }

        public void initializeImageCoverImage()
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(ImagePathUrl);
            bi.EndInit();

            CoverImage = bi;

        }

    }
}
