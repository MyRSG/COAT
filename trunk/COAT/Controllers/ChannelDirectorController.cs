using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid;
using COAT.Models;
using COAT.ViewModel.Shared;
using COAT.COATExtension;
using COAT.Extension;
using COAT.Helper;

namespace COAT.Controllers
{
    [Authorize(Roles = "Admin,ChannelDirector")]
    public class ChannelDirectorController : COATController
    {
        [Authorize]
        public ActionResult Index(DealSearchViewModel search, GridSortOptions sort, int? page)
        {
            search.ControllerName = "ChannelDirector";
            search.ActionName = "Index";
            ViewBag.SearchModel = search;
            ViewBag.Sort = sort;

            var deals = dealMgr.GetChannelDirectorDeals().Search(search);
            return View(SortAndPagingDeal(deals, sort, page));
        }

        protected override void OnApproving(Deal deal, FormCollection collection)
        {
            deal.DirectorId = GetCurrentMemberShipUser().COATUser.Id;
            deal.DirectorDate = System.DateTime.Now;
            UpdateDeal(deal, new string[] { "DirectorId", "DirectorDate" });
            base.OnApproving(deal, collection);
        }

        protected override void OnApproved(Deal deal, FormCollection collection)
        {
            base.OnApproved(deal, collection);

            var dbDeal = GetDeal(deal.Id);
            var url = Url.AbsoluteAction("Details", "Deals", new { id = deal.Id });
            var tmp = new MailTempleteHepler().GetTemplete(MailTempleteHepler.DealApproved);
            var msg = string.Format(tmp, dbDeal.Customer.Name, url);
            mHelper.SendMail(
                new string[] { deal.Notifier.Email, deal.Approver.Email },
                new string[] { GetCurrentMemberShipUser().Email },
                "合作伙伴业务机会已经获得渠道部门批准",
                msg);
        }

        public override ActionResult Reject(Deal deal, FormCollection collection)
        {
            deal.DirectorId = GetCurrentMemberShipUser().COATUser.Id;
            deal.DirectorDate = System.DateTime.Now;
            UpdateDeal(deal, new string[] { "DirectorId", "DirectorDate" });

            var rslt = base.Reject(deal, collection);
            var dbDeal = GetDeal(deal.Id);
            var url = Url.AbsoluteAction("Details", "Deals", new { id = deal.Id });
            mHelper.SendMail(
                new string[] { dbDeal.Approver.Email },
                new string[] { GetCurrentMemberShipUser().Email },
                "A new ORP Deal was rejectd by director in COAT",
                string.Format("Name:{0}\r\nURL:{1}", dbDeal.Name, url));

            return rslt;
        }

        protected override int PreviousStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "PCA").Id; }
        }

        protected override int NextStatusId
        {
            get { return db.Status.FirstOrDefault(a => a.ActionName == "A").Id; }
        }

        protected override IEnumerable<SelectListItem> AssignToList
        {
            get { return null; }
        }
    }
}
