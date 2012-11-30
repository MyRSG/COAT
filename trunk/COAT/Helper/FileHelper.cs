using System.IO;
using System.Web;
using System.Web.Hosting;
using COAT.Models;

namespace COAT.Helper
{
    public class FileHelper
    {
        public const string ContractFolderName = "contracts";
        public const string RawDataFolderName = "rawdata";
        private readonly COATEntities _db = new COATEntities();
        public string FileRootPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Files");

        public FileHelper()
        {
            IntialStructure();
        }

        private string RawDataFolderPath
        {
            get { return Path.Combine(FileRootPath, RawDataFolderName); }
        }

        private string ContractFolderPath
        {
            get { return Path.Combine(FileRootPath, ContractFolderName); }
        }

        public FileStore SaveRawDateFile(HttpPostedFileBase file)
        {
            return SaveFile(file, RawDataFolderPath);
        }

        public FileStore SaveContractFile(HttpPostedFileBase file)
        {
            return SaveFile(file, ContractFolderPath);
        }

        public string GetPhysicalPath(string path)
        {
            return Path.Combine(FileRootPath, path);
        }

        private void IntialStructure()
        {
            if (!Directory.Exists(FileRootPath))
            {
                Directory.CreateDirectory(FileRootPath);
            }

            if (!Directory.Exists(RawDataFolderPath))
            {
                Directory.CreateDirectory(RawDataFolderPath);
            }

            if (!Directory.Exists(ContractFolderPath))
            {
                Directory.CreateDirectory(ContractFolderPath);
            }
        }

        private FileStore SaveFile(HttpPostedFileBase file, string folderPath)
        {
            try
            {
                var fs = new FileStore
                             {
                                 Extension = new FileInfo(file.FileName).Extension,
                                 MimeType = file.ContentType,
                                 Directory = folderPath,
                                 ContentLength = file.ContentLength,
                                 OriginalName = file.FileName
                             };
                _db.FileStores.AddObject(fs);
                _db.SaveChanges();

                file.SaveAs(fs.FilePath);

                return fs;
            }
            catch
            {
                return null;
            }
        }
    }
}