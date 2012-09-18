using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Models;
using COAT.IDS;

namespace COAT.COATExtension
{
    public static class UserExtension
    {
        public static string GetNameRoleString(this User user)
        {
            var role = user.SystemRoleId == SystemRoleIds.ChannelApprover ? "Channel" : "Sales";
            return string.Format("{0} ({1})", user.Name, role);
        }
    }
}