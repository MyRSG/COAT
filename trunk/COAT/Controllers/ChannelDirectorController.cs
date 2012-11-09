using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COAT.COATExtension;
using COAT.Helper;
using COAT.Models;
using COAT.Util.Extension;
using COAT.ViewModel.Shared;
using MvcContrib.UI.Grid;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,ChannelDirector")]
    public class ChannelDirectorController : COATController
    {
        protected override int PreviousStatusId
        {
            get
            {
                var firstOrDefault = Db.Status.FirstOrDefault(a => a.ActionName == "PCA");
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

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get { return null; }
        }

        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "ChannelDirector";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            IQueryable<Deal> deals = DealMgr.GetChannelDirectorDeals().Search(search);
            return View(SortAndPagingDeal(deals, sort, page));
        }

        protected override void OnApproving(Deal deal, FormCollection collection)
        {
            deal.DirectorId = GetCurrentMemberShipUser().COATUser.Id;
            deal.DirectorDate = DateTime.Now;
            UpdateDeal(deal, new[] {"DirectorId", "DirectorDate"});
            base.OnApproving(deal, collection);
        }

        protected override void OnApproved(Deal deal, FormCollection collection)
        {
            base.OnApproved(deal, collection);

            Deal dbDeal = GetDeal(deal.Id);
            string url = Url.AbsoluteAction("Details", "Deals", new {id = deal.Id});
            string tmp = new MailTempleteHepler().GetTemplete(MailTempleteHepler.DealApproved);
            string msg = string.Format(tmp, dbDeal.Customer.Name, url);
            MHelper.SendMail(
                new[] {deal.Notifier.Email, deal.Approver.Email},
                new[] {GetCurrentMemberShipUser().Email},
                "合作伙伴业务机会已经获得渠道部门批准",
                msg);
        }

        public override ActionResult Reject(Deal deal, FormCollection collection)
        {
            deal.DirectorId = GetCurrentMemberShipUser().COATUser.Id;
            deal.DirectorDate = DateTime.Now;
            UpdateDeal(deal, new[] {"DirectorId", "DirectorDate"});

            ActionResult rslt = base.Reject(deal, collection);
            Deal dbDeal = GetDeal(deal.Id);
            string url = Url.AbsoluteAction("Details", "Deals", new {id = deal.Id});
            MHelper.SendMail(
                new[] {dbDeal.Approver.Email},
                new[] {GetCurrentMemberShipUser().Email},
                "A new ORP Deal was rejectd by director in COAT",
                string.Format("Name:{0}\r\nURL:{1}", dbDeal.Name, url));

            return rslt;
        }
    }
}