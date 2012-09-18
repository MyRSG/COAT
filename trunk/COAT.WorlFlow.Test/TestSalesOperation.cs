using COAT.WorkFlow;
using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit;
using COAT.WorkFlow.Unit.Args;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.WorlFlow.Test
{
    [TestClass]
    public class TestSalesOperation : TestBaseUnit
    {
        #region Overrides of TestBaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.SalesOperation; }
        }

        protected override BaseUnit Unit
        {
            get { return new SalesOperation(); }
        }

        #endregion

        [TestMethod]
        public void TestCreateSalesOperation()
        {
            Assert.IsNotNull(Unit);
        }

        [TestMethod]
        public void TestGetPendingDeal()
        {
            var deals = Unit.GetPendingDeal();
            Assert.IsNotNull(deals);
        }

        [TestMethod]
        public void TestApproveDeal()
        {
            var d = CreateDeal();

            const int indsutry2 = 1;
            const int province2 = 1;
            var args = new SalesOperationApprArgs { Industry2 = indsutry2, Province2 = province2, Comment = Comment };
            var approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(indsutry2, d.Industry2);
            Assert.AreEqual(province2, d.Province2);
            Assert.AreEqual(WorkFlowStep.ChannelManager, d.CurrentStep);

        }

        [TestMethod]
        public void TestDeclineDeal()
        {
            var d = CreateDeal();

            var args = new SalesOperationApprArgs { Comment = Comment };
            var approval = Unit.Decline(d, args);

            AssertDeclineApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.Decline, d.CurrentStep);
        }

        [TestMethod]
        public void TestAssignWrongDeal()
        {
            var d = CreateDeal();

            var args = new SalesOperationApprArgs { Comment = Comment };
            var approval = Unit.AssignWrong(d, args);

            AssertAssignWrongApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.ORPTeam, d.CurrentStep);
        }


    }
}
