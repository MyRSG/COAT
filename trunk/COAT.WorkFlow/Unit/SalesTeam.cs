using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class SalesTeam : BaseUnit
    {
        public const double DirectorSize = 4000.00d;

        #region Overrides of BaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.SalesTeam; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            deal.Switch(deal.Size > DirectorSize ? WorkFlowStep.ChannelDirector : WorkFlowStep.Approve);
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }

        #endregion

        protected override void AdditionOpeation(IDeal deal, IApproval approval, BaseApprArgs args)
        {
            var stArgs = (SalesTeamApprArgs)args;

            var answer = WorkFlowManager.WorkFlowFactory.CreateAnswer();
            answer.DealId = deal.Id;
            answer.ApprovalId = approval.Id;
            answer.QuestionId = stArgs.QuestionId;
            answer.Answer = stArgs.Answer;

            WorkFlowManager.DataFactory.SaveAnswer(answer);
        }


    }
}
