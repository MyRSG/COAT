using COAT.WorkFlow;
using COAT.WorkFlow.Unit;
using COAT.WorkFlow.Unit.Args;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.WorlFlow.Test
{
    /// <summary>
    /// Summary description for TestChannelDirector
    /// </summary>
    [TestClass]
    public class TestChannelDirector : TestBaseUnit
    {

        protected override int CurrentStep
        {
            get { return WorkFlowStep.ChannelDirector; }
        }

        protected override BaseUnit Unit
        {
            get { return new ChannelDirector(); }
        }

        [TestMethod]
        public void TestCreateChannelDirector()
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

            var args = new BaseApprArgs { Comment = Comment };
            var approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.Approve, d.CurrentStep);

        }

        [TestMethod]
        public void TestDeclineDeal()
        {
            var d = CreateDeal();

            var args = new BaseApprArgs { Comment = Comment };
            var approval = Unit.Decline(d, args);

            AssertDeclineApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.Decline, d.CurrentStep);
        }

        [TestMethod]
        public void TestAssignWrongDeal()
        {
            var d = CreateDeal();

            var args = new BaseApprArgs { Comment = Comment };
            var approval = Unit.AssignWrong(d, args);

            AssertAssignWrongApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.SalesTeam, d.CurrentStep);

        }

    }
}
