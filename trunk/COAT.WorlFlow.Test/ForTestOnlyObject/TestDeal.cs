using COAT.WorkFlow.Base;

namespace COAT.WorlFlow.Test
{
    class TestDeal : IDeal
    {
        public int Id { get; set; }
        public int CurrentStep { get; set; }
        public int ORPType { get; set; }
        public int Industry2 { get; set; }
        public int Province2 { get; set; }
        public double Size { get; set; }

    }
}