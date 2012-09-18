using System.Linq;

namespace COAT.WorkFlow.Base
{
    public interface IWorkFlowFactory
    {
        IDeal CreateDeal();
        IApproval CreateApproval();
        IQueryable<IDeal> GetPendingDeals(SearchCondition sc);
        IDeal GetPendingDeal(SearchCondition sc);
        IApproval GetLastApproval(IDeal deal);
        IAnswer CreateAnswer();
        IAnswer GetAnswer(int p, int p_2);
    }
}
