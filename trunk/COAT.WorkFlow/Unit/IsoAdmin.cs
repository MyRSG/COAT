using COAT.WorkFlow.Base;
using COAT.WorkFlow.Unit.Args;

namespace COAT.WorkFlow.Unit
{
    public class IsoAdmin : BaseUnit
    {
        #region Overrides of BaseUnit

        protected override int CurrentStep
        {
            get { return WorkFlowStep.ISOAdmin; }
        }

        protected override void ApproveDeal(IDeal deal, BaseApprArgs args)
        {
            var isoArgs = (IsoAdminApprArgs)args;
            deal.Industry2 = isoArgs.Industry2;
            deal.Province2 = isoArgs.Province2;
            deal.Switch(WorkFlowStep.InsideSales);
            deal.Save();
        }

        protected override void SendNotifcationMail(IDeal deal, BaseApprArgs args)
        {

        }

        #endregion
    }
}
