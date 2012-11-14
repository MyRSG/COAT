using System;
using System.Collections.Generic;
using System.Web.Mvc;
using COAT.Helper;
using COAT.Models;
using COAT.Util.Extension;

namespace COAT.Controllers
{
    public abstract class COATApproverController : COATController
    {
        protected abstract IEnumerable<SelectListItem> RollbackToList { get; }
        protected abstract int PendingSalesStatusId { get; }

        public override ActionResult Details(string id)
        {
            ViewBag.RollBackToList = RollbackToList;
            return base.Details(id);
        }

        protected override string GetComment(FormCollection collection)
        {
            try
            {
                return collection["comment"];
            }
            catch
            {
                return string.Empty;
            }
        }

        protected override void OnApproving(Deal deal, FormCollection collection)
        {
            deal.ApproverId = GetCurrentMemberShipUser().COATUser.Id;
            deal.ApproveDate = DateTime.Now;
            deal.NotifySalesId = GetAssignToId(collection);
            UpdateDeal(deal,
                       new[] {"ChinaRegion", "ProvinceId", "Industry2Id", "NotifySalesId", "ApproverId", "ApproveDate"});
            base.OnApproving(deal, collection);
        }

        public override ActionResult Reject(Deal deal, FormCollection collection)
        {
            deal.ApproverId = GetCurrentMemberShipUser().COATUser.Id;
            deal.ApproveDate = DateTime.Now;
            deal.NotifySalesId = GetAssignToId(collection);
            UpdateDeal(deal, new[] {"NotifySalesId", "ApproverId", "ApproveDate"});
            Db.SaveChanges();
            return base.Reject(deal, collection);

           /* var view = base.Reject(deal, collection);
            SendRejectMail(deal, collection);
            return view;*/
        }

        public override ActionResult Wrong(Deal deal, FormCollection collection)
        {
            deal.NotifySalesId = GetAssignToId(collection);
            UpdateDeal(deal, new[] {"ChinaRegion", "ProvinceId", "Industry2Id"});
            Db.SaveChanges();
            return base.Wrong(deal, collection);
        }

        public ActionResult PendingSales(Deal deal, FormCollection collection)
        {
            deal.NotifySalesId = GetAssignToId(collection);
            UpdateDeal(deal, new[] {"ChinaRegion", "ProvinceId", "Industry2Id", "NotifySalesId"});

            Deal dbDeal = GetDeal(deal.Id);
            dbDeal.StatusId = PendingSalesStatusId;
            SaveApproveHistory(dbDeal.Id, "Wait for Sales Confirmation", GetComment(collection));
            Db.SaveChanges();

            string url = Url.AbsoluteAction("Details", "Deals", new {id = deal.Id});
            string tmp = new MailTempleteHepler().GetTemplete(MailTempleteHepler.SalesCofirm);
            string msg = string.Format(tmp, dbDeal.Name, url);
            MHelper.SendMail(
                new[] {GetAssignUser(collection).Email},
                new[] {GetCurrentMemberShipUser().Email},
                new[] {GetCurrentMemberShipUser().Email},
                "新的合作伙伴业务机会正在等待你的回复确认",
                msg);


            return Redirect("../Index");
        }

        protected void SendApprovedMail(Deal deal, FormCollection collection)
        {
            Deal dbDeal = GetDeal(deal.Id);
            string url = Url.AbsoluteAction("Details", "Deals", new {id = deal.Id});
            string tmp = new MailTempleteHepler().GetTemplete(MailTempleteHepler.DealApproved);
            string msg = string.Format(tmp, dbDeal.Customer.Name, url,deal.Partner.Name);
            MHelper.SendMail(
                new[] {GetAssignUser(collection).Email},
                new[] {GetCurrentMemberShipUser().Email},
                new[] {GetCurrentMemberShipUser().Email},
                "合作伙伴业务机会已经获得渠道部门批准",
                msg);
        }

        protected void SendRejectMail(Deal deal, FormCollection collection)
        {
            try
            {
                var dbDeal = GetDeal(deal.Id);
                var url = Url.AbsoluteAction("Details", "Deals", new { id = deal.Id });
                var tmp = new MailTempleteHepler().GetTemplete(MailTempleteHepler.DealRejected);
                var msg = string.Format(tmp, dbDeal.Customer.Name, url,deal.Partner.Name);
                MHelper.SendMail(
                    new string[] { GetAssignUser(collection).Email },
                    new string[] { GetCurrentMemberShipUser().Email },
                    new string[] { GetCurrentMemberShipUser().Email },
                    "合作伙伴业务机会已经被拒绝",
                    msg);
            }
            catch
            { }
        }
    }
}