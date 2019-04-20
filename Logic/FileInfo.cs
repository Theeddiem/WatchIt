using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class FileInfo 
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public FileInfo(string i_FilePath, string i_FileName)
        {
            FilePath = i_FilePath;

            FileName = i_FileName;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
