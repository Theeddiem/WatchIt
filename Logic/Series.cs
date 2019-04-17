using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Series : Video
    {
        public Series(string i_Name, string i_Genere, string i_Rating, string i_ReleasedYear)
        {
            m_Name = i_Name;
            m_Genre = i_Genere;
            m_Rating = i_Rating;
            m_ReleasedYear = i_ReleasedYear;
        }
    }
}
