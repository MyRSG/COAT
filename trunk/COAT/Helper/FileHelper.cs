using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.IO;
using COAT.Models;

namespace COAT.Helper
{
    public class FileHelper
    {
        public const string _ContractFolderName = "contracts";
        public const string _RawDataFolderName = "rawdata";
        public string _FileRootPath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Files");

        public FileHelper()
        {
            IntialStructure();
        }

        public FileStore SaveRawDateFile(HttpPostedFileBase file)
        {
            return SaveFile(file, RawDataFolderPath);
        }

        public FileStore SaveContractFile(HttpPostedFileBase file)
        {
            return SaveFile(file, ContractFolderPath);
        }

        private void IntialStructure()
        {
            if (!Directory.Exists(_FileRootPath))
            {
                Directory.CreateDirectory(_FileRootPath);
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

        private string RawDataFolderPath
        {
            get { return Path.Combine(_FileRootPath, _RawDataFolderName); }
        }

        private string ContractFolderPath
        {
            get { return Path.Combine(_FileRootPath, _ContractFolderName); }
        }

        private FileStore SaveFile(HttpPostedFileBase file, string folderPath)
        {
            try
            {
                FileStore fs = new FileStore();
                fs.Extension = new FileInfo(file.FileName).Extension;
                fs.MimeType = file.ContentType;
                fs.Directory = folderPath;
                fs.ContentLength = file.ContentLength;
                fs.OriginalName = file.FileName;
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

        private COATEntities _db = new COATEntities();
    }
}