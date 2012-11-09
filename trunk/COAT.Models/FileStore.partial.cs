using System.IO;

namespace COAT.Models
{
    public partial class FileStore
    {
        public string FilePath
        {
            get { return Path.Combine(Directory, FileName); }
        }

        public string FileName
        {
            get
            {
                string patten = Extension.Contains(".") ? "{0}{1}" : "{0}.{1}";
                return string.Format(patten, Id, Extension);
            }
        }
    }
}