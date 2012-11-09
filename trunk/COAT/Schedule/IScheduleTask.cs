namespace COAT.Schedule
{
    public interface IScheduleTask
    {
        int RunTimes { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string Type { get; set; }
        string ErrorMessage { get; set; }
        void Run(object obj);
        void Success();
        void Error(string msg);
    }
}