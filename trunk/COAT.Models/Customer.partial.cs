using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COAT.Models
{
    public partial class Customer
    {
        public string Name
        {
            get
            {
                if (NameENG == null)
                    return NameCHS;
                return NameENG;
            }
        }
    }
}