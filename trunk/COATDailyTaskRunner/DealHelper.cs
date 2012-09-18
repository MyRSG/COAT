using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Models;

namespace COATDailyTaskRunner
{
    interface IDealHelper
    {
        int Count { get; }
        Deal[] LessThan7Days { get; }
        Deal[] MoreThan7Days { get; }
        Deal[] MoreThan14Days { get; }
    }
}
