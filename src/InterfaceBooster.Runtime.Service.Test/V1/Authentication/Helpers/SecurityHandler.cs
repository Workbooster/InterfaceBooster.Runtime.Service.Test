using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Runtime.Business.BasicAuth.Utilities;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers
{
    public class SecurityHandler
    {
        #region MEMBERS

        private Security _Security;

        #endregion MEMBERS

        #region PUBLIC METHODS

        #region SECURITY ADMINISTRATION

        public Security Load(string path)
        {
            _Security = new Security(path);

            return _Security;
        }

        public bool AddPermission(string name, string[] methods, string[] path)
        {
            return _Security.AddPermission(name, methods, path);
        }

        public bool RemovePermission(string name)
        {
            return _Security.RemovePermission(name);
        }

        public bool AddRole(string name, string[] permissions)
        {
            return _Security.AddRole(name, permissions);
        }

        public bool AddPermissionToRole(string name, string permissionName)
        {
            return _Security.AddPermissionToRole(name, permissionName);
        }

        public bool RemoveRole(string name)
        {
            return _Security.RemoveRole(name);
        }

        public bool AddUser(string alias, string userName, string password, string[] roles)
        {
            string userInformation = Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(String.Format("{0}:{1}", userName, password)));

            return _Security.AddUser(alias, userName, password, roles);
        }

        public bool AddRoleToUser(string alias, string role)
        {
            return _Security.AddRoleToUser(alias, role);
        }

        public bool RemoveUser(string alias)
        {
            return _Security.RemoveUser(alias);
        }

        #endregion SECURITY ADMINISTRATION

        #region HELPERS

        public string UserInformation(string userName, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", userName, password)));
        }

        public string AuthHeder(string userName, string password)
        {
            return String.Format("Basic {0}", UserInformation(userName, password));
        }

        #endregion HELPERS

        #endregion PUBLIC METHODS
    }
}
