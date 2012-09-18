using COAT.WorkFlow;
using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit;
using COAT.WorkFlow.Unit.Args;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.WorlFlow.Test
{
    [TestClass]
    public class TestORPTeam : TestBaseUnit
    {
        #region Overrides of TestBaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.ORPTeam; }
        }

        protected override BaseUnit Unit
        {
            get { return new ORPTeam(); }
        }

        #endregion

        [TestMethod]
        public void TestCreateORPTeam()
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

            var args = new ORPTeamApprArgs { ORPType = ORPType.SMB, Comment = Comment };
            var approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(ORPType.SMB, d.ORPType);
            Assert.AreEqual(WorkFlowStep.ISOAdmin, d.CurrentStep);

            args = new ORPTeamApprArgs { ORPType = ORPType.Soluition, Comment = Comment };
            Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(ORPType.Soluition, d.ORPType);
            Assert.AreEqual(WorkFlowStep.SalesOperation, d.CurrentStep);

            args = new ORPTeamApprArgs { ORPType = ORPType.Dist, Comment = Comment };
            Unit.Approve(d, args);
            AssertApproveApproval(d, approval);
            Assert.AreEqual(ORPType.Dist, d.ORPType);
            Assert.AreEqual(WorkFlowStep.SalesOperation, d.CurrentStep);
        }

        [TestMethod]
        public void TestDeclineDeal()
        {
            var d = CreateDeal();
            d.ORPType = ORPType.SMB;

            var args = new ORPTeamApprArgs { Comment = Comment };
            var approval = Unit.Decline(d, args);

            AssertDeclineApproval(d, approval);
            Assert.AreEqual(ORPType.SMB, d.ORPType);
            Assert.AreEqual(WorkFlowStep.Decline, d.CurrentStep);
        }


    }

}
