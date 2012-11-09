using System.Collections.Generic;
using System.Linq;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    internal class SimpleDealSummary : IDealSummary
    {
        public SimpleDealSummary(IEnumerable<Deal> deals)
        {
            Deals = deals;
        }

        public IEnumerable<Deal> Deals { get; protected set; }

        #region IDealSummary Members

        public int ToBeAssigned
        {
            get { return Deals.Count(a => a.Status.ActionName == "PA"); }
        }

        public int ToBeValidated
        {
            get { return Deals.Count(a => a.Status.ActionName == "PCA" || a.Status.ActionName == "PSA"); }
        }

        public int Approved
        {
            get { return Deals.Count(a => a.Status.ActionName == "A"); }
        }

        public int Declined
        {
            get { return Deals.Count(a => a.Status.ActionName == "R"); }
        }

        public int OnHold
        {
            get { return Deals.Count(a => a.Status.ActionName == "PSC"); }
        }

        public int Total
        {
            get { return Deals.Count(); }
        }

        #endregion
    }
}