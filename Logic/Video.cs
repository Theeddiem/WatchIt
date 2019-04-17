using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Video
    {
        protected string m_Name;
        protected string m_Genre;
        protected double m_Rating;
        protected int m_ReleasedYear;

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", m_Name, m_ReleasedYear, m_Rating, m_Genre);
        }
    }
}
