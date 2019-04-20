using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class Video 
    {
        protected string m_Title;

        protected string m_Genre;

        protected string m_ImdbId;
 
        protected double m_Rating;    

        protected int m_ReleasedYear;   

        protected string m_ImagePath;
       
        protected string m_FilePath;

        public override string ToString()
        {
            return string.Format("{0}  {1}, {2} {3}", Title, ReleasedYear, Rating, Genre);
        }

        public virtual string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }
        public virtual string Genre
        {
            get { return m_Genre; }
            set { m_Genre = value; }
        }
        public virtual string ImdbId
        {
            get { return m_ImdbId; }
            set { m_ImdbId = value; }
        }
        public virtual double Rating
        {
            get { return m_Rating; }
            set { m_Rating = value; }
        }
        public virtual int ReleasedYear
        {
            get { return m_ReleasedYear; }
            set { m_ReleasedYear = value; }
        }

        public virtual string ImagePath
        {
            get { return m_ImagePath; }
            set { m_ImagePath = value; }
        }

        public virtual string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

    }
}
