using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Video : IComparable<Video>
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Rating { get; set; }
        public int ReleasedYear { get; set; }

        public int CompareTo(Video other)
        {
            return (int)(Rating - other.Rating);
        }

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", Title, ReleasedYear, Rating, Genre);
        }


    }
}
