using System.Collections.Generic;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class DealProductsViewModel
    {
        public IEnumerable<DealProduct> DealProducts { get; set; }
        public ApproveHeaderType ApproveHeaderType { get; set; }
        public bool IsShowAssign { get; set; }
        public bool IsShowWrong { get; set; }
        public string CurrentStatusActionName { get; set; }
    }

    public enum ApproveHeaderType
    {
        Assign,
        Approve
    }
}