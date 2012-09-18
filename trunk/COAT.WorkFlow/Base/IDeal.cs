namespace COAT.WorkFlow.Base
{
    public interface IDeal
    {
        int Id { get; set; }
        int CurrentStep { get; set; }
        int ORPType { get; set; }
        int Industry2 { get; set; }
        int Province2 { get; set; }
        double Size { get; set; }
    }
}
