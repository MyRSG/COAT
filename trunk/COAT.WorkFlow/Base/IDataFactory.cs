namespace COAT.WorkFlow.Base
{
    public interface IDataFactory
    {
        IDeal SaveDeal(IDeal deal);
        IAnswer SaveAnswer(IAnswer answer);
        IApproval[] GetApprovals();
        IApproval SaveApproval(IApproval approval);

    }
}
