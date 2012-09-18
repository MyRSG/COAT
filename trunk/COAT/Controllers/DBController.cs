using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAT.Models;
using COAT.Security;
using System.Web.Security;

namespace COAT.Controllers
{
    public abstract class BaseController : Controller
    {
        protected COATEntities db = new COATEntities();

        protected COATMemebershipUser GetCurrentMemberShipUser()
        {
            if (!Request.IsAuthenticated)
                return null;

            return Membership.GetUser(true) as COATMemebershipUser;
        }

        protected void UpdateProducts(Dictionary<int, bool> productActive, bool saveChange = false)
        {
            foreach (var pair in productActive)
            {
                var product = db.DealProducts.FirstOrDefault(a => a.Id == pair.Key);
                product.IsActive = pair.Value;
            }

            if (saveChange)
            {
                db.SaveChanges();
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

            db.ApprovalHistories.AddObject(rec);
            if (saveChange)
            {
                db.SaveChanges();
            }
        }

        protected Deal GetDeal(string id)
        {
            return db.Deals.FirstOrDefault(a => a.Id == id);
        }
    }
}
