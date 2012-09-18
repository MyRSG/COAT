using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Models;

namespace COATDailyTaskRunner
{
    class DirectorDealHelper : IDealHelper
    {
        const int WarningDays = 7;
        const int EmergencyDays = 14;

        private COATEntities db = new COATEntities();
        private string _ActionName;

        public DirectorDealHelper()
        {
            _ActionName = "PDA";
        }

        public IEnumerable<Deal> Deals
        {
            get
            {
                return db.Deals.Where(d => d.Status.ActionName == _ActionName).ToArray();
            }
        }
        public Deal[] LessThan7Days
        {
            get
            {
                return Deals.Where(d => DayPassed(d.CreateDate) <= WarningDays).ToArray();
            }
        }
        public Deal[] MoreThan7Days
        {
            get
            {
                return Deals.Where(d => DayPassed(d.CreateDate) > WarningDays && DayPassed(d.CreateDate) <= EmergencyDays).ToArray();
            }
        }
        public Deal[] MoreThan14Days
        {
            get
            {
                return Deals.Where(d => DayPassed(d.CreateDate) > EmergencyDays).ToArray();
            }
        }

        public static int DayPassed(DateTime? date)
        {
            var date2 = date ?? DateTime.Today;
            return (DateTime.Today - date2).Days;
        }

        public int Count
        {
            get { return Deals.Count(); }
        }
    }
}
