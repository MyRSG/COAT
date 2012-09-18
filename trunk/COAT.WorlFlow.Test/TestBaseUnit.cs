using System;
using COAT.WorkFlow;
using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit;
using COAT.WorkFlow.Unit.Args;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.WorlFlow.Test
{
    public abstract class TestBaseUnit
    {
        protected const string Comment = "Test Deal";

        protected abstract int CurrentStep { get; }
        protected abstract BaseUnit Unit { get; }

        protected TestBaseUnit()
        {
            if (WorkFlowManager.WorkFlowFactory == null)
                WorkFlowManager.InjectWorkFlowFactory(new TestWorkFlowFactory());

            if (WorkFlowManager.DataFactory == null)
                WorkFlowManager.InjectDataFacotry(new TestDataFacoty());
        }

        public IDeal CreateDeal()
        {
            var d = WorkFlowManager.WorkFlowFactory.CreateDeal();
            d.Id = 
            d.CurrentStep = CurrentStep;
            return d;
        }

        protected void AssertApproveApproval(IDeal d, IApproval approval)
        {
            AssertApproval(d, approval, ApprovalAction.Approve);
        }

        protected void AssertDeclineApproval(IDeal d, IApproval approval)
        {
            AssertApproval(d, approval, ApprovalAction.Decline);
        }

        protected void AssertAssignWrongApproval(IDeal d, IApproval approval)
        {
            AssertApproval(d, approval, ApprovalAction.AssignWrong);
        }

        protected void AssertApproval(IDeal d, IApproval approval, int action)
        {
            Assert.AreEqual(d.Id, approval.DealId);
            Assert.AreEqual(CurrentStep, approval.Step);
            Assert.AreEqual(action, approval.ApprovalAction);
            Assert.AreEqual(Comment, approval.Comment);
        }
    }


}
