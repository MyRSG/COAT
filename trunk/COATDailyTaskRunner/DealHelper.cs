using COAT.Models;

namespace COATDailyTaskRunner
{
    internal interface IDealHelper
    {
        int Count { get; }
        Deal[] LessThan7Days { get; }
        Deal[] MoreThan7Days { get; }
        Deal[] MoreThan14Days { get; }
    }
}