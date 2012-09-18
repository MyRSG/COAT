using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using COAT.Models.Util;
using COAT.Extension;

namespace COAT.Models
{
    public partial class Deal
    {
        public double TotalPrice
        {
            get
            {
                return DealProducts.Where(a => a.IsActive).Sum(b => b.Price);
            }
        }

        public string DealSize
        {
            get
            {
                if (TotalPrice >= 40 * 1000)
                {
                    return ">=40K";
                }
                else if (TotalPrice > 15 * 1000)
                {
                    return ">15K";
                }

                return "<=15K";
            }
        }

        public string Industry2Name
        {
            get
            {
                if (Industry2 != null)
                    return Industry2.Name;
                return string.Empty;
            }
        }

        public string ProvinceName
        {
            get
            {
                if (Province != null)
                    return Province.Name;
                return string.Empty;
            }
        }

        public DateTime? ActionDate
        {
            get
            {
                if (this.Status.ActionName != "A" && this.Status.ActionName != "R")
                    return null;

                if (this.DirectorDate != null)
                    return this.DirectorDate;

                return ApproveDate;
            }
        }
    }
}