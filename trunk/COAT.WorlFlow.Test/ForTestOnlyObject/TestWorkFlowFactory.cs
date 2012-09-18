using System.Collections.Generic;
using System.Linq;
using COAT.WorkFlow;
using COAT.WorkFlow.Base;
using COAT.WorlFlow.Test.ForTestOnlyObject;

namespace COAT.WorlFlow.Test
{
    class TestWorkFlowFactory : IWorkFlowFactory
    {
        private readonly List<IDeal> _deals;

        public TestWorkFlowFactory()
        {
            _deals = new List<IDeal>();

            for (var index = 1; index < 3; index++)
            {
                _deals.Add(new TestDeal { Id = index });
            }
        }

        public IDeal CreateDeal()
        {
            return new TestDeal();
        }

        public IApproval CreateApproval()
        {
            return new TestApproval();
        }

        public IQueryable<IDeal> GetPendingDeals(SearchCondition sc)
        {
            return _deals.AsQueryable();
        }

        public IDeal GetPendingDeal(SearchCondition sc)
        {
            return _deals.AsQueryable().First(d => d.Id == sc.DealId);
        }

        public IApproval GetLastApproval(IDeal deal)
        {

            switch (deal.CurrentStep)
            {
                case WorkFlowStep.SalesOperation:
                    return new TestApproval { DealId = deal.Id, Step = WorkFlowStep.ORPTeam };
                case WorkFlowStep.ISOAdmin:
                    return new TestApproval { DealId = deal.Id, Step = WorkFlowStep.ORPTeam };
                case WorkFlowStep.ChannelManager:
                    return new TestApproval { DealId = deal.Id, Step = WorkFlowStep.SalesOperation };
                case WorkFlowStep.InsideSales:
                    return new TestApproval { DealId = deal.Id, Step = WorkFlowStep.ISOAdmin };
                case WorkFlowStep.ChannelDirector:
                    return new TestApproval { DealId = deal.Id, Step = WorkFlowStep.SalesTeam };
            }

            return WorkFlowManager.DataFactory.GetApprovals().Last(a => a.DealId == deal.Id);


        }

        public IAnswer CreateAnswer()
        {
            return new TestAnswer();
        }

        public IAnswer GetAnswer(int dealId, int approvaId)
        {
            return new TestAnswer { DealId = dealId, ApprovalId = approvaId, Answer = "Test Answer", QuestionId = 1 };
        }
    }
}
