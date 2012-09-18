using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace COAT.Security
{
    public class COATRoleProvider : RoleProvider
    {
        RoleRepository roleRepository = new RoleRepository();

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "COATRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "COAT Memebership Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"],
                        System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            return;
        }

        public override string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            return;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return roleRepository.FindUsersInRole(roleName, usernameToMatch);
        }

        public override string[] GetAllRoles()
        {
            return roleRepository.GetAllRoles();
        }

        public override string[] GetRolesForUser(string username)
        {
            return roleRepository.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return roleRepository.GetUsersInRole(roleName);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return roleRepository.IsUserInRole(username, roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            return;
        }

        public override bool RoleExists(string roleName)
        {
            return roleRepository.RoleExists(roleName);
        }

        //
        // A helper function to retrieve config values from the configuration file.
        //  
        string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        string _ApplicationName;
    }
}