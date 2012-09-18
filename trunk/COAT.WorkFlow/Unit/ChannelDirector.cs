using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class ChannelDirector : BaseUnit
    {
        #region Overrides of BaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.ChannelDirector; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            deal.Switch(WorkFlowStep.Approve);
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }

        #endregion
    }
}
