using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class DealSummaryViewModel : IDealSummary
    {

        public IDealSummary Enterprise { get; set; }
        public IDealSummary Volume { get; set; }

        public DealSummaryViewModel(IEnumerable<Deal> deals)
        {
            var _Ids = new COATEntities().SalesDealsViews.Select(a => a.Id).ToArray();

            Enterprise = new SimpleDealSummary(deals.Where(d => !_Ids.Contains(d.Id)));
            Volume = new SimpleDealSummary(deals.Where(d => _Ids.Contains(d.Id)));
        }

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
    }
}