using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Data.Export;

namespace COAT.Data.Test
{
    class ExportTestObject
    {
        [Export]
        public string DefaultValue { get; set; }

        [Export(Name = "MyName")]
        public string NameValue { get; set; }

        [Export(Order = 1)]
        public string OrderValue { get; set; }

        public int NoExport { get; set; }

    }
}
