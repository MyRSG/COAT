using System.Collections.Generic;
using System.Web.Mvc;
using COAT.Models;

namespace COAT.ViewModel.Shared
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public bool IsSetRegion { get; set; }

        public string RegionDropdownListHeader
        {
            get { return "cus_region_"; }
        }

        public string RegionDropdownListName
        {
            get { return "cus_region_" + Customer.Id; }
        }

        public IEnumerable<SelectListItem> RegionSelectList
        {
            get
            {
                var regions = new List<SelectListItem>
                                  {
                                      GenerateSelectListItem("North"),
                                      GenerateSelectListItem("South"),
                                      GenerateSelectListItem("East"),
                                      GenerateSelectListItem("West")
                                  };
                return regions;
            }
        }

        private SelectListItem GenerateSelectListItem(string value)
        {
            return new SelectListItem {Text = value, Value = value, Selected = Customer.Region == value};
        }
    }
}