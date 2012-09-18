using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.ViewModel
{
    public class AssignedDeal
    {
        public bool Selected { get; set; }
        public string Id { get; set; }
        public int Industry2Id { get; set; }
        public int ProvinceId { get; set; }
        public string Region { get; set; }
        public int ApproverId { get; set; }
    }
}
