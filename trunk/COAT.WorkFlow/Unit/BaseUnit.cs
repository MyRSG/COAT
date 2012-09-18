using System;
using System.Linq;
using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public abstract class BaseUnit
    {
        protected abstract int CurrentStep { get; }
        protected abstract void ApproveDeal(IDeal deal, BaseApprArgs args);
        protected abstract void SendNotifcationMail(IDeal deal, BaseApprArgs args);


        public virtual IQueryable<IDeal> GetPendingDeal()
        {
            return GetPendingDealsByStep(CurrentStep);
        }

        public virtual IApproval Approve(IDeal deal, BaseApprArgs args)
        {
            ApproveDeal(deal, args);
            var rslt = CreateApproval(deal.Id, args.ActionUser, ApprovalAction.Approve, args.Comment);
            AdditionOpeation(deal, rslt, args);
            SendNotifcationMail(deal, args);

            return rslt;
        }

        public virtual IApproval Decline(IDeal deal, BaseApprArgs args)
        {
            deal.Switch(WorkFlowStep.Decline);
            deal.Save();
            return CreateApproval(deal.Id, args.ActionUser, ApprovalAction.Decline, args.Comment);
        }

        public virtual IApproval AssignWrong(IDeal deal, BaseApprArgs args)
        {
            IApproval lastAppr = WorkFlowManager.WorkFlowFactory.GetLastApproval(deal);
            deal.Switch(lastAppr.Step);
            deal.Save();

            return CreateApproval(deal.Id, args.ActionUser, ApprovalAction.AssignWrong, args.Comment);
        }

        protected virtual void AdditionOpeation(IDeal deal, IApproval approval, BaseApprArgs args)
        {

        }

        protected IDeal GetPendingDeal(int id)
        {
            var sc = new SearchCondition { DealId = id };
            return WorkFlowManager.WorkFlowFactory.GetPendingDeal(sc);
        }

        protected IQueryable<IDeal> GetPendingDealsByStep(int step)
        {
            var sc = new SearchCondition { CurrentStep = step };
            return WorkFlowManager.WorkFlowFactory.GetPendingDeals(sc);
        }

        protected IApproval CreateApproval(int dealId, IUser actionUser, int approvalAction, string comment)
        {
            var approval = WorkFlowManager.WorkFlowFactory.CreateApproval();
            approval.DealId = dealId;
            approval.UserId = actionUser == null ? -1 : actionUser.Id;
            approval.Step = CurrentStep;
            approval.ApprovalAction = approvalAction;
            approval.Comment = comment;
            approval.CreateDate = DateTime.Now;

            WorkFlowManager.DataFactory.SaveApproval(approval);
            return approval;
        }

    }
}
