using System;
using System.Linq;

namespace COAT.Models
{
    public partial class Deal
    {
        public double TotalPrice
        {
            get { return DealProducts.Where(a => a.IsActive).Sum(b => b.Price); }
        }

        public string DealSize
        {
            get
            {
                if (TotalPrice >= 40*1000)
                {
                    return ">=40K";
                }
                return TotalPrice > 15*1000 ? ">15K" : "<=15K";
            }
        }

        public string Industry2Name
        {
            get { return Industry2 != null ? Industry2.Name : string.Empty; }
        }

        public string ProvinceName
        {
            get { return Province != null ? Province.Name : string.Empty; }
        }

        public DateTime? ActionDate
        {
            get
            {
                if (Status.ActionName != "A" && Status.ActionName != "R")
                    return null;

                return DirectorDate ?? ApproveDate;
            }
        }
    }
}