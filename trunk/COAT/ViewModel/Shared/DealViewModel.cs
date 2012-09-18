using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class DealViewModel
    {
        public Deal Deal { get; set; }
        public bool IsEditable { get; set; }
    }
}