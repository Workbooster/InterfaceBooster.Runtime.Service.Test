using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Definitions
{
    [TestFixture]
    public class Security_Administration_Works
    {
        #region MEMBERS

        private SecurityHandler _SecurityHandler = new SecurityHandler();

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            _SecurityHandler.Load(@"C:\Dat\IB_Authentication");
        }

        [Test]
        public void Add_And_Remove_A_Permission_Works()
        {
            Assert.IsTrue(_SecurityHandler.AddPermission("testPermission", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" }));

            Assert.IsTrue(_SecurityHandler.RemovePermission("testPermission"));
        }

        [Test]
        public void Add_Or_Remove_A_Permission_Twice_Does_Not_Work()
        {
            Assert.IsTrue(_SecurityHandler.AddPermission("testPermission", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" }));
            Assert.IsFalse(_SecurityHandler.AddPermission("testPermission", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" }));

            Assert.IsTrue(_SecurityHandler.RemovePermission("testPermission"));
            Assert.IsFalse(_SecurityHandler.RemovePermission("testPermission"));
        }

        [Test]
        public void Add_And_Remove_A_Role_Works()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });

            Assert.IsTrue(_SecurityHandler.AddRole("testRole", new string[] { "testPermission1", "testPermission2" }));

            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));

            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Add_Permmission_To_Role_Works()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });

            Assert.IsTrue(_SecurityHandler.AddRole("testRole", new string[] { "testPermission1", "testPermission2" }));
            Assert.IsTrue(_SecurityHandler.AddPermission("testPermission", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" }));

            Assert.IsTrue(_SecurityHandler.AddPermissionToRole("testRole", "testPermission"));

            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));
            Assert.IsTrue(_SecurityHandler.RemovePermission("testPermission"));

            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Add_Or_Remove_A_Role_Twice_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });

            Assert.IsTrue(_SecurityHandler.AddRole("testRole", new string[] { "testPermission1", "testPermission2" }));
            Assert.IsFalse(_SecurityHandler.AddRole("testRole", new string[] { "TaskRunner", "TaskOthers" }));

            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));
            Assert.IsFalse(_SecurityHandler.RemoveRole("testRole"));

            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Add_And_Remove_A_User_Works()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddRole("testRole1", new string[] { "testPermission1", "testPermission2" });

            Assert.IsTrue(_SecurityHandler.AddUser("testUser", "text", "test", new string[] { "testRole1" }));

            Assert.IsTrue(_SecurityHandler.RemoveUser("testUser"));

            _SecurityHandler.RemoveRole("testRole1");
            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Add_Role_To_User_Works()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddRole("testRole1", new string[] { "testPermission1", "testPermission2" });

            Assert.IsTrue(_SecurityHandler.AddUser("testUser", "text", "test", new string[] { "testRole1" }));
            Assert.IsTrue(_SecurityHandler.AddRole("testRole", new string[] { "testPermission1", "testPermission2" }));

            Assert.IsTrue(_SecurityHandler.AddRoleToUser("testUser", "testRole"));

            Assert.IsTrue(_SecurityHandler.RemoveUser("testUser"));
            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));

            _SecurityHandler.RemoveRole("testRole1");
            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }


        [Test]
        public void Add_Or_Remove_A_User_Twice_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddRole("testRole1", new string[] { "testPermission1", "testPermission2" });

            Assert.IsTrue(_SecurityHandler.AddUser("testUser", "text", "test", new string[] { "testRole1" }));
            Assert.IsFalse(_SecurityHandler.AddUser("testUser", "text", "test", new string[] { "testRole1" }));

            Assert.IsTrue(_SecurityHandler.RemoveUser("testUser"));
            Assert.IsFalse(_SecurityHandler.RemoveUser("testUser"));

            _SecurityHandler.RemoveRole("testRole1");
            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Add_A_Role_Referencing_A_Unknown_Permission_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });

            Assert.IsFalse(_SecurityHandler.AddRole("testRole", new string[] { "TestPermission" }));

            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Remove_A_Permission_Referenced_In_A_Role_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddRole("testRole", new string[] { "testPermission" });

            Assert.IsFalse(_SecurityHandler.RemovePermission("testPermission"));

            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));

            Assert.IsTrue(_SecurityHandler.RemovePermission("testPermission"));
        }

        [Test]
        public void Add_A_User_Referencing_A_Unknown_Role_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddRole("testRole1", new string[] { "testPermission1", "testPermission2" });

            Assert.IsFalse(_SecurityHandler.AddUser("testUser", "text", "test", new string[] { "TaskRunner" }));

            _SecurityHandler.RemoveRole("testRole1");
            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        [Test]
        public void Remove_A_Role_Referenced_In_A_User_Does_Not_Work()
        {
            _SecurityHandler.AddPermission("testPermission1", new string[] { "GET", "POST" }, new string[] { "fielsystem", "file" });
            _SecurityHandler.AddPermission("testPermission2", new string[] { "PUT", "DELETE" }, new string[] { "fielsystem", "file" });

            _SecurityHandler.AddRole("testRole", new string[] { "testPermission1", "testPermission2" });
            _SecurityHandler.AddUser("testUser", "text", "test", new string[] { "testRole" });

            Assert.IsFalse(_SecurityHandler.RemoveRole("testRole"));

            Assert.IsTrue(_SecurityHandler.RemoveUser("testUser"));

            Assert.IsTrue(_SecurityHandler.RemoveRole("testRole"));

            _SecurityHandler.RemovePermission("testPermission1");
            _SecurityHandler.RemovePermission("testPermission2");
        }

        #endregion PUBLIC METHODS
    }
}
