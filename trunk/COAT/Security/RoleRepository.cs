using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COAT.Database;

namespace COAT.Security
{
    public class RoleRepository
    {
        UserEntityManager userMgr = new UserEntityManager();
        RoleEntityManager roleMgr = new RoleEntityManager();

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var role = roleMgr.GetRole(roleName);

            if (usernameToMatch != null)
            {
                return role.Users.Where(u => u.Name == usernameToMatch).Select(u => u.Name).ToArray(); ;
            }

            return role.Users.Select(u => u.Name).ToArray();
        }

        public string[] GetAllRoles()
        {
            return roleMgr.GetAllRoles().Select(r => r.Name).ToArray();
        }

        public string[] GetRolesForUser(string username)
        {
            return new string[] { userMgr.FindUserByName(username).SystemRole.Name };
        }

        public string[] GetUsersInRole(string roleName)
        {
            return FindUsersInRole(roleName, null);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return userMgr.FindUserByName(username).SystemRole.Name == roleName;
        }

        public bool RoleExists(string roleName)
        {
            try
            {
                return roleMgr.GetRole(roleName) != null;
            }
            catch
            {
                return false;
            }

        }
    }
}