using COAT.WorkFlow;
using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit;
using COAT.WorkFlow.Unit.Args;
using COAT.WorlFlow.Test.ForTestOnlyObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.WorlFlow.Test
{
    [TestClass]
    public class TestSalesTeam : TestBaseUnit
    {
        #region Overrides of TestBaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.SalesTeam; }
        }

        protected override BaseUnit Unit
        {
            get { return new SalesTeam(); }
        }

        #endregion

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

            var testAnswer = new TestAnswer { QuestionId = 1, Answer = "Test Answer" };
            var args = new SalesTeamApprArgs { QuestionId = testAnswer.QuestionId, Answer = testAnswer.Answer, Comment = Comment };
            var approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            AssertAnswer(d, approval, testAnswer);
            Assert.AreEqual(WorkFlowStep.Approve, d.CurrentStep);

            d.Size = SalesTeam.DirectorSize;
            approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.Approve, d.CurrentStep);

            d.Size = SalesTeam.DirectorSize + 0.1;
            approval = Unit.Approve(d, args);

            AssertApproveApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.ChannelDirector, d.CurrentStep);

        }

        [TestMethod]
        public void TestDeclineDeal()
        {
            var d = CreateDeal();

            var args = new SalesTeamApprArgs { Comment = Comment };
            var approval = Unit.Decline(d, args);

            AssertDeclineApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.Decline, d.CurrentStep);
        }

        [TestMethod]
        public void TestAssignWrongDeal()
        {
            var d = GetDealFromChannelManager();

            var args = new SalesTeamApprArgs { Comment = Comment };
            var approval = Unit.AssignWrong(d, args);

            AssertAssignWrongApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.ChannelManager, d.CurrentStep);

            d = GetDealFromInsideSales();
            approval = Unit.AssignWrong(d, args);
            AssertAssignWrongApproval(d, approval);
            Assert.AreEqual(WorkFlowStep.InsideSales, d.CurrentStep);

        }

        private IDeal GetDealFromChannelManager()
        {
            var deal = new TestChannelManager().CreateDeal();
            deal.Id = 10;
            new ChannelManager().Approve(deal, new BaseApprArgs { Comment = Comment });
            return deal;
        }

        private IDeal GetDealFromInsideSales()
        {
            var deal = new TestInsideSales().CreateDeal();
            deal.Id = 11;
            new InsideSales().Approve(deal, new BaseApprArgs { Comment = Comment });
            return deal;
        }

        private void AssertAnswer(IDeal deal, IApproval app, IAnswer answer)
        {
            var actureAnswer = WorkFlowManager.WorkFlowFactory.GetAnswer(deal.Id, app.Id);
            Assert.AreEqual(deal.Id, actureAnswer.DealId);
            Assert.AreEqual(app.Id, actureAnswer.ApprovalId);
            Assert.AreEqual(answer.QuestionId, actureAnswer.QuestionId);
            Assert.AreEqual(answer.Answer, actureAnswer.Answer);
        }
    }

}
