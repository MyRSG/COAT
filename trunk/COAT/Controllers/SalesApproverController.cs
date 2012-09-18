using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COAT.Models;
using MvcContrib.UI.Grid;
using COAT.IDS;
using COAT.ViewModel.Shared;
using COAT.COATExtension;
using COAT.Extension;
using COAT.Helper;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,SalesApprover")]
    public class SalesApproverController : COATApproverController
    {
        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "SalesApprover";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            var user = GetCurrentMemberShipUser();
            var deals = dealMgr.GetSalesApproverDeals(user.Id).Search(search);


            return View(SortAndPagingDeal(deals, sort, page));
        }

        protected override void OnApproved(Deal deal, FormCollection collection)
        {
            base.OnApproved(deal, collection);

            SendApprovedMail(deal, collection);
        }

        protected override int PreviousStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "PA").Id; }
        }

        protected override int NextStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "A").Id; }
        }

        protected override int PendingSalesStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "PSCV").Id; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                return db.Users
                    .Where(u => u.BusinessRoleId == BusinessRoleIds.InsideSales
                        || u.BusinessRoleId == BusinessRoleIds.NameAccountSales
                        || u.BusinessRoleId == BusinessRoleIds.VolumeSales)
                    .OrderBy(a => a.Name)
                    .ToList()
                    .Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            }
        }

        protected override IEnumerable<SelectListItem> RollbackToList
        {
            get
            {
                return db.Users
                  .Where(u => u.SystemRoleId == SystemRoleIds.ChannelApprover || u.SystemRoleId == SystemRoleIds.SalesApprover)
                  .OrderBy(a => a.Name)
                  .ToList()
                  .Select(u => new SelectListItem { Text = u.GetNameRoleString(), Value = u.Id.ToString() });
            }
        }

        private void UpdateIndustry2(Deal deal)
        {
            var d = db.Deals.FirstOrDefault(a => a.Id == deal.Id);
            d.Industry2Id = deal.Industry2Id;
        }
    }
}
