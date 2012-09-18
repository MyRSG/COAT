using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COAT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var imp = new COAT.Data.Import.DealImportHelper(@"D:\deal.xls");
            //imp.ImportRawData();

            //var imp = new COAT.Data.Import.NameAccountImporttHelper(@"D:\nc.xlsx");
            //imp.ImportRawData();

            //var imp = new COAT.Data.Import.ExcutivedDealImportHelper(@"D:\exdeal.xls");
            //imp.ImportRawData();

            //var imp = new COAT.Data.Import.UserImportHelper(2, 1, @"D:\ca.xlsx");
            //imp.ImportRawData();

            ViewBag.Message = "Welcome to ASP.NET MVC!";
            ViewBag.Contract = new COAT.Models.COATEntities().FileStores;


            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            return View();
        }
    }
}
