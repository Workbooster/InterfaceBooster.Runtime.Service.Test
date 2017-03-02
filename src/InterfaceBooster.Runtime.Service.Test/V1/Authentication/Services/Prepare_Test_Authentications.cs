using NUnit.Framework;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Model;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Services
{
    [TestFixture]
    public class Prepare_Test_Authentications
    {
        #region MEMBERS

        private SecurityHandler _SecurityHandler;

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
        }

        [Test]
        public void Nike()
        {
            _SecurityHandler = new SecurityHandler();

            _SecurityHandler.Load(@"C:\.NET Projects\InterfaceBooster\InterfaceBooster.Server\InterfaceBooster.Runtime.Service.ConsoleApp\bin\Debug");

            _SecurityHandler.AddPermission("GetFileInformation", new string[] { Methods.Get }, new string[] { "filesystem", "file" });

            _SecurityHandler.AddRole("FileInformationReader", new string[] { "GetFileInformation" });

            _SecurityHandler.AddUser("Hugo", "Hugo", "Rabarber", new string[] { "FileInformationReader" });
        }

        #endregion PUBLIC METHODS
    }
}
