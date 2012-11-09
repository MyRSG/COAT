using System;

namespace COAT.Data.Export
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportAttribute : Attribute
    {
        public ExportAttribute()
        {
            Name = string.Empty;
            Order = int.MaxValue;
        }

        public int Order { get; set; }
        public string Name { get; set; }
    }
}