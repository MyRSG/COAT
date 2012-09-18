using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using COAT.ViewModel.Deals;
using COAT.ViewModel.Shared;
using COAT.COATExtension;
using COAT.Data.Export;
using System;
using COAT.Models;
using COAT.Database;

namespace COAT.Controllers
{
    [Authorize]
    public class DealsController : COATController
    {
        DealEntityManager dealMgr = new DealEntityManager();

        [Authorize]
        public ViewResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "Deals";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.Sort = sort;
            var deals = dealMgr.GetDealsByUser(this.GetCurrentMemberShipUser().Id)
                .Where(a => a.Status.ActionName != "")
                .Where(a => a.CreateDate <= DateTime.Today)
                .Search(search);

            ViewBag.SummaryDeals = deals;
            return View(SortAndPagingDeal(deals, sort, page));
        }

        [Authorize]
        public ViewResult DuplicateDealCheck(DuplicateDealSearchModel searchModel, string id)
        {
            ViewBag.Search = searchModel;
            var canSearch = !string.IsNullOrWhiteSpace(searchModel.EndUserName) || !string.IsNullOrWhiteSpace(searchModel.OppName);

            ViewBag.Deals = db.Deals
                .Where(a => canSearch)
                .Where(a => a.Id != id)
                .Where(a => searchModel.EndUserName == null || a.Customer.NameCHS.Contains(searchModel.EndUserName ?? "") || a.Customer.NameENG.Contains(searchModel.EndUserName ?? ""))
                .Where(a => searchModel.OppName == null || a.Name.Contains(searchModel.OppName ?? ""));

            return View(db.ExcutivedDeals
                .Where(a => canSearch)
                .Where(a => searchModel.OppName == null || a.OppName.Contains(searchModel.OppName ?? ""))
                .Where(a => searchModel.EndUserName == null || a.EndUserName.Contains(searchModel.EndUserName ?? "") || a.EndUserName2.Contains(searchModel.EndUserName ?? "")));
        }

        [Authorize]
        public ViewResult NameAccountCheck(NameAccountSearchModel searchModel)
        {
            ViewBag.Search = searchModel;
            return View(db.NameAccounts.Where(a =>
                    a.FullAccountName.Contains(searchModel.AccountName)
                || a.FullAccountNameLocal.Contains(searchModel.AccountName)
                || a.ParentAccountName.Contains(searchModel.AccountName)
                || a.ParentAccountNameLocal.Contains(searchModel.AccountName)
                ).Take(5));
        }

        [Authorize]
        public ActionResult Export(DealSearchViewModel search)
        {
            var deals = db.Deals.Where(a => a.Status.ActionName != "").Search(search);
            var objs = new ExportGenerator(deals).Generate();
            return new ExcelResult<ExportObject>(objs.ToList());
        }

        [Authorize]
        public FileResult Download(int id)
        {
            var fs = db.FileStores.FirstOrDefault(a => a.Id == id);
            return File(fs.FilePath, fs.MimeType, fs.OriginalName);
        }

        [Authorize]
        public void Fix()
        {
            var deals = db.Deals.Where(a => a.CreateDate > DateTime.Now);
            foreach (var d in deals)
            {
                d.CreateDate = GetFixDate(d.CreateDate);
            }

            db.SaveChanges();
        }

        private DateTime? GetFixDate(DateTime? date)
        {
            if (date == null)
                return null;

            var d = (DateTime)date;
            return new DateTime(d.Year, d.Day, d.Month);
        }

        protected override int PreviousStatusId
        {
            get { return 0; }
        }

        protected override int NextStatusId
        {
            get { return 0; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get { return null; }
        }
    }
}
