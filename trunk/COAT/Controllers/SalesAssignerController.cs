using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.Models;
using COAT.Util.IDS;
using COAT.ViewModel;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin, SalesAssigner,ChannelAssigner")]
    public class SalesAssignerController : COATController
    {
        //string currentStatus = "PA";

        private int _nextStatusId;

        public SalesAssignerController()
        {
            ApproveActionName = "Assign";
            Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PSA");
            if (firstOrDefault != null)
                _nextStatusId = firstOrDefault.Id;
        }

        protected override int PreviousStatusId
        {
            get { return 0; }
        }

        protected override int NextStatusId
        {
            get { return _nextStatusId; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                return Db.Users
                    .Where(
                        u =>
                        u.SystemRoleId == SystemRoleIds.ChannelApprover || u.SystemRoleId == SystemRoleIds.SalesApprover)
                    .OrderBy(u => u.Name)
                    .ToList()
                    .Select(u => new SelectListItem
                                     {
                                         Text = u.GetNameRoleString(),
                                         Value = u.Id.ToString(CultureInfo.InvariantCulture)
                                     });
            }
        }

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "SalesAssigner";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.AssignToList = AssignToList;
            ViewBag.Sort = sort;

            IQueryable<Deal> deals = DealMgr.GetSalesAssignerDeals().Search(search);
            return View(SortAndPagingDeal(deals, sort, page));
        }

        public ActionResult Views(string id)
        {
            return RedirectToAction("Details", "Deals", new {id});
        }

        [Authorize]
        [HttpPost]
        public RedirectResult MuitiAssign(AssignedDeal[] deals, FormCollection collection)
        {
            foreach (AssignedDeal ad in deals.Where(a => a.Selected))
            {
                Approve(new Deal
                            {
                                Id = ad.Id,
                                ProvinceId = ad.ProvinceId,
                                ChinaRegion = ad.Region,
                                Industry2Id = ad.Industry2Id,
                                ApproverId = ad.ApproverId
                            }
                        , collection);
            }

            return Redirect("Index");
        }

        protected override void OnApproving(Deal deal, FormCollection collection)
        {
            //DoSetRegion(collection);
            //db.SaveChanges();
            //deal.ApproverId = GetAssignToId(collection);

            SetNextStatus(deal.ApproverId);
            deal.AssignerId = GetCurrentMemberShipUser().COATUser.Id;
            deal.AssignDate = DateTime.Now;
            UpdateDeal(deal,
                       new[] {"ChinaRegion", "ProvinceId", "Industry2Id", "ApproverId", "AssignerId", "AssignDate"});
            base.OnApproving(deal, collection);
        }

        public void DoSetRegion(FormCollection collection)
        {
            var vm = new CustomerViewModel();
            foreach (string key in collection.AllKeys)
            {
                if (key.IndexOf(vm.RegionDropdownListHeader, StringComparison.Ordinal) != 0) continue;
                int cid = int.Parse(key.Remove(0, vm.RegionDropdownListHeader.Length));
                Customer cus = Db.Customers.FirstOrDefault(a => a.Id == cid);
                if (cus != null) cus.Region = collection[key];
                return;
            }

            throw new HttpRequestValidationException("Customer Region should not be empty !");
        }

        protected void SetNextStatus(int? userId)
        {
            if (userId == null)
            {
                Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PSA");
                if (firstOrDefault != null)
                    _nextStatusId = firstOrDefault.Id;
            }

            User user = Db.Users.FirstOrDefault(a => a.Id == userId);
            Status orDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PCA");
            if (orDefault != null)
            {
                Status @default = Db.Status.FirstOrDefault(a => a.ActionName == "PSA");
                if (@default != null)
                    _nextStatusId = user != null && user.SystemRoleId == SystemRoleIds.ChannelApprover
                                        ? orDefault.Id
                                        : @default.Id;
            }
        }
    }
}