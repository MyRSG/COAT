namespace COAT.WorkFlow.Base
{
    static class DealExtension
    {
        public static IDeal Switch(this IDeal deal, int step)
        {
            deal.CurrentStep = step;
            return deal;
        }

        public static IDeal Save(this IDeal deal)
        {
            return WorkFlowManager.DataFactory.SaveDeal(deal);
        }
    }
}
