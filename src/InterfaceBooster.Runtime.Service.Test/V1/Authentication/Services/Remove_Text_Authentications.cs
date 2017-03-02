using NUnit.Framework;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Model;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Services
{
    [TestFixture]
    public class NUTestModule
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

            bool c = _SecurityHandler.RemoveUser("Hugo");

            bool b = _SecurityHandler.RemoveRole("FileInformationReader");

            bool a = _SecurityHandler.RemovePermission("GetFileInformation");
        }

        #endregion PUBLIC METHODS
    }
}
