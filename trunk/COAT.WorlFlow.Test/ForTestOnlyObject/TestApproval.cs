using System;
using COAT.WorkFlow.Base;

namespace COAT.WorlFlow.Test
{
    public class TestApproval : IApproval
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int Step { get; set; }
        public int ApprovalAction { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }

    }


}