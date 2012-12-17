using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using COAT.Database;
using COAT.Models;
using COAT.Security;
using COAT.Util.Extension;
using COAT.Util.Log;
using COAT.Util.Mail;

namespace COAT.Controllers
{
    public abstract class BaseController : Controller
    {
        protected COATEntities Db = new COATEntities();
        protected DealEntityManager DealMgr = new DealEntityManager();
        protected COATMailHelper MHelper = new COATMailHelper(true);
        protected Logger Logger = new Logger();

        protected Deal GetDeal(string id)
        {
            return Db.Deals.FirstOrDefault(a => a.Id == id);
        }

        protected COATMemebershipUser GetCurrentMemberShipUser()
        {
            if (!Request.IsAuthenticated)
                return null;

            return Membership.GetUser(true) as COATMemebershipUser;
        }

        protected void UpdateDeal(Deal deal, string[] properties)
        {
            Deal dbDeal = Db.Deals.FirstOrDefault(a => a.Id == deal.Id);
            if (dbDeal == null)
                return;

            dbDeal.UpdateInclude(deal, properties);
        }

        protected void UpdateProducts(Dictionary<int, bool> productActive, bool saveChange = false)
        {
            foreach (var pair in productActive)
            {
                DealProduct product = Db.DealProducts.FirstOrDefault(a => a.Id == pair.Key);
                if (product != null) product.IsActive = pair.Value;
            }

            if (saveChange)
            {
                Db.SaveChanges();
            }
        }

        protected void SaveApproveHistory(string dealId, string type, string comment, bool saveChange = false)
        {
            var rec = new ApprovalHistory
                          {
                              DealId = dealId,
                              Type = type,
                              DateTime = DateTime.Now,
                              Comment = comment,
                              UserId = GetCurrentMemberShipUser().COATUser.Id
                          };

            Db.ApprovalHistories.AddObject(rec);
            if (saveChange)
            {
                Db.SaveChanges();
            }
        }
    }
}