using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.Data.Export;
using COAT.Models;
using COAT.ViewModel.Deals;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize]
    public class DealsController : COATController
    {
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

        [Authorize]
        public ViewResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "Deals";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.Sort = sort;
            IQueryable<Deal> deals = DealMgr.GetDealsByUser(GetCurrentMemberShipUser().Id)
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
            bool canSearch = !string.IsNullOrWhiteSpace(searchModel.EndUserName) ||
                             !string.IsNullOrWhiteSpace(searchModel.OppName);

            ViewBag.Deals = Db.Deals
                .Where(a => canSearch)
                .Where(a => a.Id != id)
                .Where(
                    a =>
                    searchModel.EndUserName == null || a.Customer.NameCHS.Contains(searchModel.EndUserName ?? "") ||
                    a.Customer.NameENG.Contains(searchModel.EndUserName ?? ""))
                .Where(a => searchModel.OppName == null || a.Name.Contains(searchModel.OppName ?? ""));

            return View(Db.ExcutivedDeals
                            .Where(a => canSearch)
                            .Where(a => searchModel.OppName == null || a.OppName.Contains(searchModel.OppName ?? ""))
                            .Where(
                                a =>
                                searchModel.EndUserName == null || a.EndUserName.Contains(searchModel.EndUserName ?? "") ||
                                a.EndUserName2.Contains(searchModel.EndUserName ?? "")));
        }

        [Authorize]
        public ViewResult NameAccountCheck(NameAccountSearchModel searchModel)
        {
            ViewBag.Search = searchModel;
            return View(Db.NameAccounts.Where(a =>
                                              a.FullAccountName.Contains(searchModel.AccountName)
                                              || a.FullAccountNameLocal.Contains(searchModel.AccountName)
                                              || a.ParentAccountName.Contains(searchModel.AccountName)
                                              || a.ParentAccountNameLocal.Contains(searchModel.AccountName)
                            ).Take(5));
        }

        [Authorize]
        public ActionResult Export(DealSearchViewModel search)
        {
            IQueryable<Deal> deals = Db.Deals.Where(a => a.Status.ActionName != "").Search(search);
            ExportObject[] objs = new ExportGenerator(deals).Generate();
            return new ExcelResult<ExportObject>(objs.ToList());
        }

        [Authorize]
        public FileResult Download(int id)
        {
            FileStore fs = Db.FileStores.FirstOrDefault(a => a.Id == id);
            return fs != null ? File(fs.FilePath, fs.MimeType, fs.OriginalName) : null;
        }

        [Authorize]
        public void Fix()
        {
            IQueryable<Deal> deals = Db.Deals.Where(a => a.CreateDate > DateTime.Now);
            foreach (Deal d in deals)
            {
                d.CreateDate = GetFixDate(d.CreateDate);
            }

            Db.SaveChanges();
        }

        private DateTime? GetFixDate(DateTime? date)
        {
            if (date == null)
                return null;

            var d = (DateTime) date;
            return new DateTime(d.Year, d.Day, d.Month);
        }
    }
}