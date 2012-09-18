using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Web.Mvc;

namespace COAT.ViewModel.Shared
{
    public class DealProductViewModel
    {
        public DealProduct DealProduct { get; set; }
        public string ActionInputHead { get { return "dp_action_"; } }
        public string ActionInputName { get { return ActionInputHead + DealProduct.Id; } }
    }
}