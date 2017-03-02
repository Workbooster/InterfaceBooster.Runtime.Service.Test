using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using NUnit.Framework;
using InterfaceBooster.Runtime.Business.BasicAuth.Utilities;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Model;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Authentication
{
    [TestFixture]
    public class Authentication_Works
    {
        #region CONSTANTS

        string WORKINGPATH = @"C:\Dat\IB_Authentication";

        #endregion CONSTANTS

        #region MEMBERS

        private SecurityHandler _SecurityHandler;

        private BasicAuthentication _BasicAuthentication;

        private Stopwatch stopWatch = new Stopwatch();

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            Array.ForEach(Directory.GetFiles(WORKINGPATH), File.Delete);

            _SecurityHandler = new SecurityHandler();

            _SecurityHandler.Load(WORKINGPATH);

            _SecurityHandler.AddPermission("Administrator", new string[] { Methods.Get, Methods.Post, Methods.Put, Methods.Delete }, new string[] { "" });
            _SecurityHandler.AddPermission("TaskRunner", new string[] { Methods.Get, Methods.Post }, new string[] { "runtime", "task" });
            _SecurityHandler.AddPermission("TaskLogReader", new string[] { Methods.Get }, new string[] { "runtime", "task", "(GUID)", "log" });
            _SecurityHandler.AddPermission("TaskOthers", new string[] { Methods.Get, Methods.Put }, new string[] { "runtime", "task", "(*)", "others" });
            _SecurityHandler.AddPermission("TaskInteger", new string[] { Methods.Get, Methods.Delete }, new string[] { "runtime", "identity", "(int)"});
            _SecurityHandler.AddPermission("TaskGuid", new string[] { Methods.Put, Methods.Post }, new string[] { "runtime", "guid", "(GUID)" });

            _SecurityHandler.AddRole("Administrator", new string[] { "Administrator" });
            _SecurityHandler.AddRole("TaskUser", new string[] { "TaskRunner", "TaskLogReader", "TaskOthers", "TaskInteger", "TaskGuid" });

            _SecurityHandler.AddUser("ADMIN", "Administrator", "abcd$1234", new string[] { "Administrator" });
            _SecurityHandler.AddUser("GAU", "Giovanni", "Gina", new string[] { "TaskUser" });

            _BasicAuthentication = new BasicAuthentication(WORKINGPATH);
        }

        [Test]
        public void Administrator_Works()
        {
            string authHeader = _SecurityHandler.AuthHeder("Administrator", "abcd$1234");

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, ""));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, ""));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, ""));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, ""));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "filesystem/file"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "filesystem/file"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "filesystem/file"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "filesystem/file"));
        }

        [Test]
        public void Giovanni_Works()
        {
            string authHeader = _SecurityHandler.AuthHeder("Giovanni", "Gina");

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, ""));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/idc"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/idc"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/idc"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/idc"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/idc/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/idc/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/idc/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/idc/log"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/identity/17"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/identity/17"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/identity/17"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/identity/17"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/identity/rabarber"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/identity/rabarber"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/identity/rabarber"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/identity/rabarber"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/log"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/log"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/guid/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/guid/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/guid/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/guid/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/guid/17"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/guid/17"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/guid/17"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/guid/17"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/guid"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/guid"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/guid"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/guid"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/idc/others"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/idc/others"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/idc/others"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/idc/others"));

            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/others"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/others"));
            Assert.AreEqual(HttpStatusCode.OK, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/others"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task/{A5871AFF-FCDC-41C2-BD40-4446C002FEF6}/others"));
        }

        [Test]
        public void Pepe_Does_Not_Work()
        {
            string authHeader = _SecurityHandler.AuthHeder("Pepe", "Carmen");

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, ""));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, ""));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime"));

            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Get, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Post, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Put, "runtime/task"));
            Assert.AreEqual(HttpStatusCode.Unauthorized, _BasicAuthentication.KnownCredentials(authHeader, Methods.Delete, "runtime/task"));
        }

        #endregion PUBLIC METHODS
    }
}
