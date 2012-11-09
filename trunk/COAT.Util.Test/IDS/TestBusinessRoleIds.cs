using COAT.Util.IDS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COAT.Util.Test.IDS
{
    [TestClass]
    public class TestBusinessRoleIds
    {
        [TestMethod]
        public void TestBusinessRoleIdsValues()
        {
            Assert.AreEqual(1, BusinessRoleIds.ChannelManager);
            Assert.AreEqual(2, BusinessRoleIds.ChannelDirector);
            Assert.AreEqual(3, BusinessRoleIds.VolumeSales);
            Assert.AreEqual(4, BusinessRoleIds.InsideSales);
            Assert.AreEqual(5, BusinessRoleIds.SalesDirector);
            Assert.AreEqual(6, BusinessRoleIds.RSGDeveloper);
            Assert.AreEqual(7, BusinessRoleIds.Unkonwn);
            Assert.AreEqual(8, BusinessRoleIds.NameAccountSales);
        }
    }
}
