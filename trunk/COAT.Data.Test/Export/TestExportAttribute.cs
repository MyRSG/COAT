using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using COAT.Data.Export;

namespace COAT.Data.Test
{
    [TestClass]
    public class TestExportAttribute
    {

        [TestMethod]
        public void TestDefaultValue()
        {
            var attr = GetExportAttribute("DefaultValue");
            Assert.AreEqual(string.Empty, attr.Name);
            Assert.AreEqual(int.MaxValue, attr.Order);
        }

        [TestMethod]
        public void TestNameValue()
        {
            var attr = GetExportAttribute("NameValue");
            Assert.AreEqual("MyName", attr.Name);
        }

        [TestMethod]
        public void TestOrderValue()
        {
            var attr = GetExportAttribute("OrderValue");
            Assert.AreEqual(1, attr.Order);
        }

        private Export.ExportAttribute GetExportAttribute(string propName)
        {
            return (Export.ExportAttribute)typeof(ExportTestObject)
                .GetProperty(propName)
                .GetCustomAttributes(typeof(Export.ExportAttribute), false)[0];
        }
    }
}
