using System.Xml.Serialization;

namespace ViewModel
{
    public class FileInfo 
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public FileInfo() { } // paramtless constocur for needed for serlliaziotn

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
