using System;
using System.Linq;
using COAT.Models;
using COAT.Util.IDS;
using COAT.Util.Values;

namespace COAT.Database
{
    public class DealEntityManager : BaseEntityManager<Deal>
    {
        public IQueryable<Deal> GetChannelAssignerDeals()
        {
            IQueryable<string> salesIds = Entities.SalesDealsViews.Select(a => a.Id);
            return Entities.Deals
                .Where(a => a.Status.ActionName == COATStatusValue.PendingAssign)
                .Where(a => !salesIds.Contains(a.Id))
                .Where(a => a.CreateDate <= DateTime.Today);
        }

        public IQueryable<Deal> GetChannelApproverDeals(int userId)
        {
            User user = Entities.Users.FirstOrDefault(a => a.Id == userId);
            return Entities.Deals
                .Where(a => a.Status.ActionName == COATStatusValue.PendingChannelApproval
                            || a.Status.ActionName == COATStatusValue.AwaitingSalesConfirmation)
                .Where(a => a.ApproverId == userId || SystemRoleIds.Admin == user.SystemRoleId);
        }

        public IQueryable<Deal> GetChannelDirectorDeals()
        {
            return Entities.Deals.Where(a => a.Status.ActionName == COATStatusValue.PendingDirectorApproval);
        }

        public IQueryable<Deal> GetSalesAssignerDeals()
        {
            IQueryable<string> salesIds = Entities.SalesDealsViews.Select(a => a.Id);
            return Entities.Deals
                .Where(a => a.Status.ActionName == COATStatusValue.PendingAssign)
                .Where(a => salesIds.Contains(a.Id))
                .Where(a => a.CreateDate <= DateTime.Today);
        }

        public IQueryable<Deal> GetSalesApproverDeals(int userId)
        {
            User user = Entities.Users.FirstOrDefault(a => a.Id == userId);
            return Entities.Deals.Where(a => a.Status.ActionName == COATStatusValue.PendingSalesApproval
                                             || a.Status.ActionName == COATStatusValue.AwaitingVolumeSalesConfirmation)
                .Where(a => a.ApproverId == userId || SystemRoleIds.Admin == user.SystemRoleId);
        }

        public IQueryable<Deal> GetDealsByUser(int userId)
        {
            User user = Entities.Users.FirstOrDefault(a => a.Id == userId);
            if (user != null && user.BusinessRoleId == BusinessRoleIds.InsideSales)
                return
                    Entities.Deals.Where(
                        a =>
                        a.ApproverId == userId || a.NotifySalesId == userId || SystemRoleIds.Admin == user.SystemRoleId);

            if (user != null && (user.BusinessRoleId == BusinessRoleIds.NameAccountSales
                                 || user.BusinessRoleId == BusinessRoleIds.VolumeSales))
                return Entities.Deals.Where(a => a.NotifySalesId == userId || SystemRoleIds.Admin == user.SystemRoleId);

            return Entities.Deals;
        }
    }
}