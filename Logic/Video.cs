using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Video : IEquatable<Video>
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ImdbId { get; set; }
        public double Rating { get; set; }
        public int ReleasedYear { get; set; }
        public string ImagePath { get; set; }

        public bool Equals(Video other)
        {
            return this.Title == other.Title &&
             this.ReleasedYear == other.ReleasedYear;
        }

    
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", Title, ReleasedYear, Rating, Genre);
        }


    }
}
