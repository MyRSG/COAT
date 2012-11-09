using System.Collections.Generic;
using System.Linq;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class DealSummaryViewModel : IDealSummary
    {
        public DealSummaryViewModel(IEnumerable<Deal> deals)
        {
            string[] ids = new COATEntities().SalesDealsViews.Select(a => a.Id).ToArray();

            var enumerable = deals as Deal[] ?? deals.ToArray();
            Enterprise = new SimpleDealSummary(enumerable.Where(d => !ids.Contains(d.Id)));
            Volume = new SimpleDealSummary(enumerable.Where(d => ids.Contains(d.Id)));
        }

        public IDealSummary Enterprise { get; set; }
        public IDealSummary Volume { get; set; }

        #region IDealSummary Members

        public int ToBeAssigned
        {
            get { return Enterprise.ToBeAssigned + Volume.ToBeAssigned; }
        }

        public int ToBeValidated
        {
            get { return Enterprise.ToBeValidated + Volume.ToBeValidated; }
        }

        public int Approved
        {
            get { return Enterprise.Approved + Volume.Approved; }
        }

        public int Declined
        {
            get { return Enterprise.Declined + Volume.Declined; }
        }

        public int OnHold
        {
            get { return Enterprise.OnHold + Volume.OnHold; }
        }

        public int Total
        {
            get { return Enterprise.Total + Volume.Total; }
        }

        #endregion
    }
}