using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using COAT.Helper;
using COAT.Models;

namespace COAT.Controllers
{
    public class FileController : Controller
    {
        private readonly FileHelper _fHelper = new FileHelper();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public FileResult Download(string filename)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ImportUsers()
        {
            ViewBag.InputNameList = GetUsersImportNameList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ImportUsers(
            HttpPostedFileBase chAss, HttpPostedFileBase chApp, HttpPostedFileBase chDir,
            HttpPostedFileBase saAss, HttpPostedFileBase saApp, HttpPostedFileBase saDir,
            HttpPostedFileBase visitor, HttpPostedFileBase ncSales, HttpPostedFileBase volSales)
        {
            ViewBag.InputNameList = GetUsersImportNameList();

            UploadAndImport(chAss, COATImportTask.TaskType.ImportChannelAssigner);
            UploadAndImport(chApp, COATImportTask.TaskType.ImportChannelApprover);
            UploadAndImport(chDir, COATImportTask.TaskType.ImportChannelDirector);
            UploadAndImport(saAss, COATImportTask.TaskType.ImportSalesAssigner);
            UploadAndImport(saApp, COATImportTask.TaskType.ImportSalesApprover);
            UploadAndImport(saDir, COATImportTask.TaskType.ImportSalesDirector);
            UploadAndImport(visitor, COATImportTask.TaskType.ImportVisitor);
            UploadAndImport(ncSales, COATImportTask.TaskType.ImportNameAccountSales);
            UploadAndImport(volSales, COATImportTask.TaskType.ImportVolumeSales);
            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ImportORPData()
        {
            ViewBag.InputNameList = GetORPDataImportNameList();

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ImportORPData(HttpPostedFileBase orpDeal, HttpPostedFileBase excDeal,
                                          HttpPostedFileBase ncList)
        {
            ViewBag.InputNameList = GetORPDataImportNameList();

            UploadAndImport(orpDeal, COATImportTask.TaskType.ImportORPDeal);
            UploadAndImport(excDeal, COATImportTask.TaskType.ImportExcutiveDeal);
            UploadAndImport(ncList, COATImportTask.TaskType.ImportNameAccountList);
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult UploadContract(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (HttpPostedFileBase file in files)
            {
                if (file == null || file.ContentLength == 0)
                    continue;

                _fHelper.SaveContractFile(file);
            }

            return View();
        }

        public void UploadAndImport(HttpPostedFileBase file, string taskType)
        {
            if (file == null || file.ContentLength == 0)
                return;

            FileStore fs = _fHelper.SaveRawDateFile(file);
            //stRunner.AddTask(new ImportTask { Type = taskType, FilePath = fs.FilePath });

            var task = new COATImportTask {Type = taskType, FilePath = fs.FilePath};
            var db = new COATEntities();
            db.ImportTasks.AddObject(task.ImportTask);
            db.SaveChanges();
            task.Run(null);
            task.Success();
            db.SaveChanges();
        }

        private dynamic[] GetORPDataImportNameList()
        {
            return new dynamic[]
                       {
                           new InputNameListItem {Text = "ORP Deal Raw Data", Name = "orpDeal"},
                           new InputNameListItem {Text = "Name Account List", Name = "ncList"},
                           new InputNameListItem {Text = "Excutived ORP Raw Data", Name = "excDeal"}
                       };
        }

        private dynamic[] GetUsersImportNameList()
        {
            return new dynamic[]
                       {
                           new InputNameListItem {Text = "Channel Assigner", Name = "chAss"},
                           new InputNameListItem {Text = "Channel Approver", Name = "chApp"},
                           new InputNameListItem {Text = "Channel Director", Name = "chDir"},
                           new InputNameListItem {Text = "Sales Assigner", Name = "saAss"},
                           new InputNameListItem {Text = "Sales Approver", Name = "saApp"},
                           new InputNameListItem {Text = "Sales Director", Name = "saDir"},
                           new InputNameListItem {Text = "Visitor", Name = "visitor"},
                           new InputNameListItem {Text = "Name Account Sales", Name = "ncSales"},
                           new InputNameListItem {Text = "Volume Sales", Name = "volSales"}
                       };
        }

        #region Nested type: InputNameListItem

        public class InputNameListItem
        {
            public string Text { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}