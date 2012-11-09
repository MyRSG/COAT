using System.Linq;
using COAT.Database;
using COAT.Models;

namespace COAT.Security
{
    public class RoleRepository
    {
        private readonly RoleEntityManager _roleMgr = new RoleEntityManager();
        private readonly UserEntityManager _userMgr = new UserEntityManager();

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            SystemRole role = _roleMgr.GetRole(roleName);

            return usernameToMatch != null
                       ? role.Users.Where(u => u.Name == usernameToMatch).Select(u => u.Name).ToArray()
                       : role.Users.Select(u => u.Name).ToArray();
        }

        public string[] GetAllRoles()
        {
            return _roleMgr.GetAllRoles().Select(r => r.Name).ToArray();
        }

        public string[] GetRolesForUser(string username)
        {
            return new[] {_userMgr.FindUserByName(username).SystemRole.Name};
        }

        public string[] GetUsersInRole(string roleName)
        {
            return FindUsersInRole(roleName, null);
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return _userMgr.FindUserByName(username).SystemRole.Name == roleName;
        }

        public bool RoleExists(string roleName)
        {
            try
            {
                return _roleMgr.GetRole(roleName) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}