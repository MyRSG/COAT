using COAT.WorkFlow.Base;

namespace COAT.WorlFlow.Test.ForTestOnlyObject
{
    public class TestAnswer : IAnswer
    {
        #region Implementation of IAnswer

        public int DealId { get; set; }
        public int ApprovalId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }

        #endregion
    }
}
