using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAT.Data.Export
{
    public class ColunmPropertyPair
    {
        public string PropertyName { get; set; }
        public string ColunmName { get; set; }
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            var pairB = obj as ColunmPropertyPair;
            if (pairB == null)
                return false;

            return pairB.GetHashCode() == this.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", new object[] { this.PropertyName, this.ColunmName, this.Order });
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
