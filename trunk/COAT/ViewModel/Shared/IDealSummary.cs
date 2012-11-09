namespace COAT.ViewModel.Shared
{
    public interface IDealSummary
    {
        int ToBeAssigned { get; }
        int ToBeValidated { get; }
        int Approved { get; }
        int Declined { get; }
        int OnHold { get; }
        int Total { get; }
    }
}