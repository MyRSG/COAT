using Microsoft.VisualStudio.TestTools.UnitTesting;
using COAT.IDS;

namespace COAT.Util.Test.IDS
{
    [TestClass]
    public class TestSystemRoleIds
    {
        [TestMethod]
        public void TestTestSystemRoleIdsValues()
        {
            Assert.AreEqual(1, SystemRoleIds.Admin);
            Assert.AreEqual(2, SystemRoleIds.ChannelAssigner);
            Assert.AreEqual(3, SystemRoleIds.SalesAssigner);
            Assert.AreEqual(4, SystemRoleIds.ChannelApprover);
            Assert.AreEqual(5, SystemRoleIds.SalesApprover);
            Assert.AreEqual(6, SystemRoleIds.ChannelDirector);
            Assert.AreEqual(7, SystemRoleIds.SalesDirector);
            Assert.AreEqual(8, SystemRoleIds.Visitor);
            Assert.AreEqual(9, SystemRoleIds.Forbidden);
        }
    }
}
