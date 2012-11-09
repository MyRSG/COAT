using COAT.Models;
using COAT.Util.IDS;

namespace COAT.COATExtension
{
    public static class UserExtension
    {
        public static string GetNameRoleString(this User user)
        {
            string role = user.SystemRoleId == SystemRoleIds.ChannelApprover ? "Channel" : "Sales";
            return string.Format("{0} ({1})", user.Name, role);
        }
    }
}