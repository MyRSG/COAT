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
    [Authorize(Roles = "Admin,ChannelApprover")]
    public class ChannelApproverController : COATApproverController
    {
        private const double DirectorSize = 40*1000;
        private int _nextStatusId;

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "ChannelApprover";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            COATMemebershipUser user = GetCurrentMemberShipUser();
            IQueryable<Deal> deals = DealMgr.GetChannelApproverDeals(user.Id).Search(search);
            return View(SortAndPagingDeal(deals, sort, page));
        }

        //[Authorize]
        //public ActionResult PendingSales(Deal deal, FormCollection collection)
        //{
        //    deal.NotifySalesId = GetAssignToId(collection);
        //    UpdateDeal(deal, new string[] { "ChinaRegion", "ProvinceId", "Industry2Id", "NotifySalesId" });

        //    var dbDeal = GetDeal(deal.Id);
        //    dbDeal.StatusId = db.Status.FirstOrDefault(a => a.ActionName == "PSC").Id;
        //    SaveApproveHistory(dbDeal.Id, "Wait for Sales Confirmation", GetComment(collection));
        //    db.SaveChanges();

        //    var url = Url.AbsoluteAction("Details", "Deals", new { id = deal.Id });
        //    var addtion = new MailTempleteHepler().GetTemplete(MailTempleteHepler.SalesCofirm);
        //    mHelper.SendMail(
        //        new string[] { GetAssignUser(collection).Email },
        //        new string[] { GetCurrentMemberShipUser().Email },
        //        new string[] { GetCurrentMemberShipUser().Email },
        //        "A new ORP Deal is waitting for your confirmation",
        //        string.Format("Name:{0}\r\nURL:{1}\r\n{2}", dbDeal.Name, url, addtion));


        //    return Redirect("../Index");
        //}

        protected override void OnApproving(Deal deal, FormCollection collection)
        {
            SetNextStatus(collection);
            base.OnApproving(deal, collection);
        }

        protected override void OnApproved(Deal deal, FormCollection collection)
        {
            base.OnApproved(deal, collection);
            Deal dbDeal = GetDeal(deal.Id);

            if (dbDeal.Status.ActionName == "PDA")
                return;

            SendApprovedMail(deal, collection);
        }

        private void SetNextStatus(FormCollection collection)
        {
            double total = GetTotalSize(collection);
            if (total >= DirectorSize)
            {
                Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PDA");
                if (firstOrDefault != null)
                    _nextStatusId = firstOrDefault.Id;
                return;
            }

            Status orDefault = Db.Status.FirstOrDefault(a => a.ActionName == "A");
            if (orDefault != null)
                _nextStatusId = orDefault.Id;
        }

        private double GetTotalSize(FormCollection collection)
        {
            double total = 0.0;
// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var pair in GetProductsActive(collection))
// ReSharper restore LoopCanBeConvertedToQuery
            {
                if (!pair.Value) continue;
                var firstOrDefault = Db.DealProducts.FirstOrDefault(a => a.Id == pair.Key);
                if (firstOrDefault != null)
                    total += firstOrDefault.Price;
            }
            return total;
        }

        #region "Abstract Implemention"

        protected override int PreviousStatusId
        {
            get
            {
                Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PA");
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
                return 0;
            }
        }

        protected override int NextStatusId
        {
            get { return _nextStatusId; }
        }

        protected override int PendingSalesStatusId
        {
            get
            {
                Status firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PSC");
                if (firstOrDefault != null)
                    return firstOrDefault.Id;
                return 0;
            }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                List<User> list = Db.Users
                    .Where(u => u.BusinessRoleId == BusinessRoleIds.InsideSales
                                || u.BusinessRoleId == BusinessRoleIds.NameAccountSales
                                || u.BusinessRoleId == BusinessRoleIds.VolumeSales)
                    .OrderBy(a => a.Name)
                    .ToList();
                return
                    list.Select(
                        u => new SelectListItem {Text = u.Name, Value = u.Id.ToString(CultureInfo.InvariantCulture)});
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
                    .Select(
                        u =>
                        new SelectListItem
                            {Text = u.GetNameRoleString(), Value = u.Id.ToString(CultureInfo.InvariantCulture)});
            }
        }

        #endregion
    }
}