﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class DealListViewModel
    {
        public IEnumerable<Deal> Deals { get; set; }
        public bool HideEditLink { get; set; }
        public string LinkText { get; set; }
        public string ActionName { get; set; }
    }
}