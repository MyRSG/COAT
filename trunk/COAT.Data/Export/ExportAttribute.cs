using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Data.Export
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ExportAttribute : Attribute
    {
        public int Order { get; set; }
        public string Name { get; set; }

        public ExportAttribute()
        {
            Name = string.Empty;
            Order = int.MaxValue;
        }
    }
}
