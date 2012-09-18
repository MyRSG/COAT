using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using COAT.Models;
using COAT.IDS;
using COAT.ViewModel.Shared;
using COAT.COATExtension;
using COAT.Extension;
using COAT.Helper;


namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,ChannelApprover")]
    public class ChannelApproverController : COATApproverController
    {

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "ChannelApprover";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            var user = GetCurrentMemberShipUser();
            var deals = dealMgr.GetChannelApproverDeals(user.Id).Search(search);
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
            var dbDeal = GetDeal(deal.Id);

            if (dbDeal.Status.ActionName == "PDA")
                return;

            SendApprovedMail(deal, collection);

        }

        #region "Abstract Implemention"

        protected override int PreviousStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "PA").Id; }
        }

        protected override int NextStatusId
        {
            get { return _NextStatusId; }
        }

        protected override int PendingSalesStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "PSC").Id; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get
            {
                var list = db.Users
                       .Where(u => u.BusinessRoleId == BusinessRoleIds.InsideSales
                         || u.BusinessRoleId == BusinessRoleIds.NameAccountSales
                         || u.BusinessRoleId == BusinessRoleIds.VolumeSales)
                     .OrderBy(a => a.Name)
                     .ToList();
                return list.Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
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


        #endregion


        private void SetNextStatus(FormCollection collection)
        {
            var total = GetTotalSize(collection);
            if (total >= _DirectorSize)
            {
                _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "PDA").Id;
                return;
            }

            _NextStatusId = db.Status.FirstOrDefault(a => a.ActionName == "A").Id;
        }

        private double GetTotalSize(FormCollection collection)
        {
            double total = 0.0;
            foreach (var pair in GetProductsActive(collection))
            {
                if (pair.Value)
                    total += db.DealProducts.FirstOrDefault(a => a.Id == pair.Key).Price;
            }
            return total;
        }

        private void UpdateIndustry2(Deal deal)
        {
            var d = db.Deals.FirstOrDefault(a => a.Id == deal.Id);
            d.Industry2Id = deal.Industry2Id;
        }


        private double _DirectorSize = 40 * 1000;
        private int _NextStatusId = 0;
    }
}
