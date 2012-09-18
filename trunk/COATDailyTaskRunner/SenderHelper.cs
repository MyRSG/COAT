using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAT.Models;
using COAT.IDS;

namespace COATDailyTaskRunner
{
    class SenderHelper
    {
        COATEntities db = new COATEntities();

        public User[] ChannelApprovers
        {
            get { return db.Users.Where(a => a.SystemRoleId == SystemRoleIds.ChannelApprover).ToArray(); }
        }

        public User[] ChannelDirectors
        {
            get { return db.Users.Where(a => a.SystemRoleId == SystemRoleIds.ChannelDirector).ToArray(); }
        }

        public User[] SalesApprovers
        {
            get { return db.Users.Where(a => a.SystemRoleId == SystemRoleIds.SalesApprover).ToArray(); }
        }


    }
}
