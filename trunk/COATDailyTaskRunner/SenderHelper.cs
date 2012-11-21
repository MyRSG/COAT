using System.Linq;
using COAT.Models;
using COAT.Util.IDS;

namespace COATDailyTaskRunner
{
    internal class SenderHelper
    {
        private readonly COATEntities _db = new COATEntities();

        public User[] ChannelApprovers
        {
            get { return _db.Users.Where(a => a.SystemRoleId == SystemRoleIds.ChannelApprover).ToArray(); }
        }

        public User[] ChannelDirectors
        {
            get { return _db.Users.Where(a => a.SystemRoleId == SystemRoleIds.ChannelDirector).ToArray(); }
        }

        public User[] SalesApprovers
        {
            get { return _db.Users.Where(a => a.SystemRoleId == SystemRoleIds.SalesApprover).ToArray(); }
        }

        public User[] Admins
        {
            get { return _db.Users.Where(a => a.SystemRoleId == SystemRoleIds.Admin).ToArray(); }
        }
    }
}