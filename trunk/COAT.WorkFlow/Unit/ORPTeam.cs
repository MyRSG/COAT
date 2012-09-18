using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class ORPTeam : BaseUnit
    {
        protected override int CurrentStep
        {
            get { return WorkFlowStep.ORPTeam; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            var orpArgs = (ORPTeamApprArgs)args;
            deal.ORPType = orpArgs.ORPType;
            deal.Switch(deal.ORPType == ORPType.SMB ? WorkFlowStep.ISOAdmin : WorkFlowStep.SalesOperation);
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }



    }
}
