using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.Models;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;
using COAT.IDS;
using COAT.ViewModel;
using COAT.COATExtension;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin, SalesAssigner,ChannelAssigner")]
    public class SalesAssignerController : COATController
    {
        //string currentStatus = "PA";

        public SalesAssignerController()
        {
            _ApproveActionName = "Assign";
            _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "PSA").Id;
        }

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "SalesAssigner";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;

            ViewBag.AssignToList = AssignToList;
            ViewBag.Sort = sort;

            var deals = dealMgr.GetSalesAssignerDeals().Search(search);
            return View(SortAndPagingDeal(deals, sort, page));
        }

        public ActionResult Views(string id)
        {
            return RedirectToAction("Details", "Deals", new { id = id });
        }

        [Authorize]
        [HttpPost]
        public RedirectResult MuitiAssign(AssignedDeal[] deals, FormCollection collection)
        {
            foreach (var ad in deals.Where(a => a.Selected))
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
            deal.AssignDate = System.DateTime.Now;
            UpdateDeal(deal, new string[] { "ChinaRegion", "ProvinceId", "Industry2Id", "ApproverId", "AssignerId", "AssignDate" });
            base.OnApproving(deal, collection);
        }

        public void DoSetRegion(FormCollection collection)
        {
            var VM = new CustomerViewModel();
            foreach (var key in collection.AllKeys)
            {
                if (key.IndexOf(VM.RegionDropdownListHeader) == 0)
                {
                    var cid = int.Parse(key.Remove(0, VM.RegionDropdownListHeader.Length));
                    var cus = db.Customers.FirstOrDefault(a => a.Id == cid);
                    cus.Region = collection[key];
                    return;
                }
            }

            throw new HttpRequestValidationException("Customer Region should not be empty !");
        }

        protected override int PreviousStatusId
        {
            get { return 0; }
        }

        protected override int NextStatusId
        {
            get { return _NextStatusId; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                return db.Users
                     .Where(u => u.SystemRoleId == SystemRoleIds.ChannelApprover || u.SystemRoleId == SystemRoleIds.SalesApprover)
                     .OrderBy(u => u.Name)
                     .ToList()
                     .Select(u => new SelectListItem
                     {
                         Text = u.GetNameRoleString(),
                         Value = u.Id.ToString()
                     });
            }
        }

        protected void SetNextStatus(int? userId)
        {
            if (userId == null)
                _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "PSA").Id;

            var user = db.Users.FirstOrDefault(a => a.Id == userId);
            if (user.SystemRoleId == SystemRoleIds.ChannelApprover)
                _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "PCA").Id;
            else
                _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "PSA").Id;
        }

        private int _NextStatusId = 0;
    }
}
