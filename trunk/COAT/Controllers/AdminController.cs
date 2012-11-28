using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController :COATController
    {

        public AdminController()
        {
            ApproveActionName = "ReAssign";
        }

        //
        // GET: /Admin/

        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "Admin";
            search.ActionName = "Index";
            search.IsHideStatus = true;
            ViewBag.SearchModel = search;

            ViewBag.Sort = sort;
            var deals = DealMgr.GetEndDeals()
                 .Where(a => a.Status.ActionName != "")
                 .Where(a => a.CreateDate <= DateTime.Today)
                 .Search(search);
            ViewBag.SummaryDeals = deals;
            return View(SortAndPagingDeal(deals, sort, page));
        }



        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }


        protected override int PreviousStatusId
        {
            get { return 0; }
        }

        protected override int NextStatusId
        {
            get
            {
                var firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PA");
                return firstOrDefault != null ? firstOrDefault.Id : 0;
            }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get { return null; }
        }
    }
}
