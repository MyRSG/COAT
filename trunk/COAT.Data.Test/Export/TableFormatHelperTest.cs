using COAT.Data.Export;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace COAT.Data.Test
{

    [TestClass()]
    public class TableFormatHelperTest
    {
        [TestMethod()]
        public void TableFormatHelperConstructorTest()
        {
            var type = typeof(Object);
            var target = new TableFormatHelper(type);
            Assert.AreEqual(type, target.ExportType);
        }

        [TestMethod()]
        public void TableFormatHelperConstructorTest1()
        {
            var obj = new object();
            var target = new TableFormatHelper(obj);
            Assert.AreEqual(obj.GetType(), target.ExportType);
        }

        [TestMethod()]
        public void GetTableSchemaTest()
        {
            var obj = new ExportTestObject();
            var target = new TableFormatHelper(obj);
            var expected = new ColunmPropertyPair[]{
                new ColunmPropertyPair{ PropertyName="OrderValue", ColunmName = "OrderValue", Order=1},
                new ColunmPropertyPair{ PropertyName="DefaultValue", ColunmName = "DefaultValue", Order=int.MaxValue},
                new ColunmPropertyPair{ PropertyName="NameValue", ColunmName = "MyName", Order=int.MaxValue}
               
            };
            IEnumerable<ColunmPropertyPair> actual = target.GetTableSchema();

            int count = 0;
            foreach (var p in actual)
            {
                Assert.AreEqual(expected[count++], p);
            }

        }
    }
}
