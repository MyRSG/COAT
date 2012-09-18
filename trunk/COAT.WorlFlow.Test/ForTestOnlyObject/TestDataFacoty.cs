using System.Collections.Generic;
using COAT.WorkFlow.Base;

namespace COAT.WorlFlow.Test
{
    class TestDataFacoty : IDataFactory
    {
        public List<IApproval> ApprovalList = new List<IApproval>();

        #region Implementation of IDataFactory

        public IDeal SaveDeal(IDeal deal)
        {
            return deal;
        }

        public IAnswer SaveAnswer(IAnswer answer)
        {
            return answer;
        }

        public IApproval SaveApproval(IApproval approval)
        {
            ApprovalList.Add(approval);
            return approval;
        }

        public IApproval[] GetApprovals()
        {
            return ApprovalList.ToArray();
        }

        #endregion
    }
}
