using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FileInfo 
    {
        public string ImagePath { get; set; }

        public string Title { get; set; }

        public FileInfo(string i_ImagePath, string i_Title)
        {
            ImagePath = i_ImagePath;

            Title = i_Title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
