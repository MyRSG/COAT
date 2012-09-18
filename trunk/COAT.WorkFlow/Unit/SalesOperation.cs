using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class SalesOperation : BaseUnit
    {
        protected override int CurrentStep
        {
            get { return WorkFlowStep.SalesOperation; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            var soArgs = (SalesOperationApprArgs)args;
            deal.Industry2 = soArgs.Industry2;
            deal.Province2 = soArgs.Province2;
            deal.CurrentStep = WorkFlowStep.ChannelManager;
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }
    }
}
