namespace COAT.WorkFlow.Base
{
    public interface IAnswer
    {
        int DealId { get; set; }
        int ApprovalId { get; set; }
        int QuestionId { get; set; }
        string Answer { get; set; }
    }
}