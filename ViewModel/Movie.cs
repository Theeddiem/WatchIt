using System;
using System.Text;

namespace ViewModel
{
    public class Movie : Video
    {
        public TMDbLib.Objects.Movies.Movie ApiMovie { get; set; }

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
            }
        }
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

            return genres.ToString();
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
    }
}
