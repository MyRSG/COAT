using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using System.Web.Mvc;

namespace COAT.ViewModel.Shared
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public bool isSetRegion { get; set; }

        public string RegionDropdownListHeader { get { return "cus_region_"; } }
        public string RegionDropdownListName { get { return "cus_region_" + Customer.Id; } }

        public IEnumerable<SelectListItem> RegionSelectList
        {
            get
            {
                List<SelectListItem> regions = new List<SelectListItem>();
                regions.Add(GenerateSelectListItem("North"));
                regions.Add(GenerateSelectListItem("South"));
                regions.Add(GenerateSelectListItem("East"));
                regions.Add(GenerateSelectListItem("West"));
                return regions;
            }
        }

        private SelectListItem GenerateSelectListItem(string value)
        {
            return new SelectListItem { Text = value, Value = value, Selected = Customer.Region == value };
        }
    }
}