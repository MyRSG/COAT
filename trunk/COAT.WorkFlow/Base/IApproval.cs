namespace COAT.WorkFlow.Base
{
    public interface IApproval
    {
        int Id { get; set; }
        int DealId { get; set; }
        int Step { get; set; }
        int ApprovalAction { get; set; }
        string Comment { get; set; }
        int UserId { get; set; }
        System.DateTime CreateDate { get; set; }

    }
}