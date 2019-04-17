﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Movie : Video
    {
        public Movie(string i_Title, string i_Genere, double i_Rating, int i_ReleasedYear, string i_ImagePath,string i_ImdbId)
        {
            Title = i_Title;
            Genre = i_Genere;
            Rating = i_Rating;
            ReleasedYear = i_ReleasedYear;
            ImagePath = i_ImagePath;
            ImdbId = i_ImdbId;
        }

    }
}
