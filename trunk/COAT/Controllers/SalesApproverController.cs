using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.Models;
using COAT.Security;
using COAT.Util.IDS;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,SalesApprover")]
    public class SalesApproverController : COATApproverController
    {
        protected override int PreviousStatusId
        {
            get
            {
                var firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PA");
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
                return 0;
            }
        }

        protected override int NextStatusId
        {
            get
            {
                var firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "A");
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
                return 0;
            }
        }

        protected override int PendingSalesStatusId
        {
            get
            {
                var firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PSCV");
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
                return 0;
            }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                return Db.Users
                    .Where(u => u.BusinessRoleId == BusinessRoleIds.InsideSales
                                || u.BusinessRoleId == BusinessRoleIds.NameAccountSales
                                || u.BusinessRoleId == BusinessRoleIds.VolumeSales)
                    .OrderBy(a => a.Name)
                    .ToList()
                    .Select(u => new SelectListItem {Text = u.Name, Value = u.Id.ToString(CultureInfo.InvariantCulture)});
            }
        }

        protected override IEnumerable<SelectListItem> RollbackToList
        {
            get
            {
                return Db.Users
                    .Where(
                        u =>
                        u.SystemRoleId == SystemRoleIds.ChannelApprover || u.SystemRoleId == SystemRoleIds.SalesApprover)
                    .OrderBy(a => a.Name)
                    .ToList()
                    .Select(u => new SelectListItem {Text = u.GetNameRoleString(), Value = u.Id.ToString(CultureInfo.InvariantCulture)});
            }
        }

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "SalesApprover";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            COATMemebershipUser user = GetCurrentMemberShipUser();
            IQueryable<Deal> deals = DealMgr.GetSalesApproverDeals(user.Id).Search(search);


            return View(SortAndPagingDeal(deals, sort, page));
        }

        protected override void OnApproved(Deal deal, FormCollection collection)
        {
            base.OnApproved(deal, collection);

            SendApprovedMail(deal, collection);
        }
    }
}