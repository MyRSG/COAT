using System;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.Web.Security;

namespace COAT.Security
{
    public class COATRoleProvider : RoleProvider
    {
        private readonly RoleRepository _roleRepository = new RoleRepository();
        private string _applicationName;

        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name.Length == 0)
                name = "COATRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "COAT Memebership Provider");
            }

            base.Initialize(name, config);

            _applicationName = GetConfigValue(config["applicationName"],
                                              HostingEnvironment.ApplicationVirtualPath);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
// ReSharper disable RedundantJumpStatement
            return;
// ReSharper restore RedundantJumpStatement
        }

        public override void CreateRole(string roleName)
        {
// ReSharper disable RedundantJumpStatement
            return;
// ReSharper restore RedundantJumpStatement
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return _roleRepository.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string[] GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            return _roleRepository.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return _roleRepository.GetUsersInRole(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return _roleRepository.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
// ReSharper disable RedundantJumpStatement
            return;
// ReSharper restore RedundantJumpStatement
        }

        public override bool RoleExists(string roleName)
        {
            return _roleRepository.RoleExists(roleName);
        }

        //
        // A helper function to retrieve config values from the configuration file.
        //  
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
    }
}