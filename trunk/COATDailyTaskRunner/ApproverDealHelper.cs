using System;
using System.Collections.Generic;
using System.Linq;
using COAT.Models;
using COAT.Util.IDS;

namespace COATDailyTaskRunner
{
    internal class ApproverDealHelper : IDealHelper
    {
        private const int WarningDays = 7;
        private const int EmergencyDays = 14;

        private readonly string _actionName;
        private readonly User _user;
        private readonly COATEntities _db = new COATEntities();

        public ApproverDealHelper(User user)
        {
            _actionName = user.SystemRoleId == SystemRoleIds.ChannelApprover ? "PCA" : "PSA";
            _user = user;
        }

        public IEnumerable<Deal> Deals
        {
            get { return _db.Deals.Where(d => d.Status.ActionName == _actionName && d.ApproverId == _user.Id).ToArray(); }
        }

        #region IDealHelper Members

        public Deal[] LessThan7Days
        {
            get { return Deals.Where(d => DayPassed(d.CreateDate) <= WarningDays).ToArray(); }
        }

        public Deal[] MoreThan7Days
        {
            get
            {
                return
                    Deals.Where(d => DayPassed(d.CreateDate) > WarningDays && DayPassed(d.CreateDate) <= EmergencyDays).
                        ToArray();
            }
        }

        public Deal[] MoreThan14Days
        {
            get { return Deals.Where(d => DayPassed(d.CreateDate) > EmergencyDays).ToArray(); }
        }

        public int Count
        {
            get { return Deals.Count(); }
        }

        #endregion

        public static int DayPassed(DateTime? date)
        {
            DateTime date2 = date ?? DateTime.Today;
            return (DateTime.Today - date2).Days;
        }
    }
}