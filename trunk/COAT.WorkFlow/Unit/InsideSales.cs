using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class InsideSales : BaseUnit
    {
        #region Overrides of BaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.InsideSales; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            deal.Switch(WorkFlowStep.SalesTeam);
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }

        #endregion
    }
}
