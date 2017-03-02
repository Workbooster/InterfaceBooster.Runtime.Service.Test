using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NUnit.Framework;
using InterfaceBooster.Runtime.Business.BasicAuth.Utilities;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Helpers;
using InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers;
using InterfaceBooster.Runtime.Service.Test.V1.FileSystem;
using InterfaceBooster.Runtime.Service.Test.V1.Authentication.Model;

namespace InterfaceBooster.Runtime.Service.Test.V1.Authentication.Services
{
    [TestFixture]
    class FileSystem_FileControler_Works
    {
        #region MEMBERS

        private HttpClient _Client;

        #endregion MEMBERS

        [SetUp]
        public void SetupTest()
        {
            TestEnvironnement.GetOrDeleteFiles();
        }

        [Test]
        public void Get_File_MetaData_From_Root_With_Legal_Authorisation_Does_Not_Work()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl, true, "Hugo", "Rabarber");

            string endpoint = "filesystem/file";

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", endpoint, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_File_MetaData_From_Root_With_Wrong_Password_Does_Not_Work()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl, true, "Hugo", "Pflaume");

            string endpoint = "filesystem/file";

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", endpoint, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Unauthorized, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_File_MetaData_From_Root_With_Wrong_Username_Does_Not_Work()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl, true, "Kevin", "Rabarber");

            string endpoint = "filesystem/file";

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", endpoint, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Unauthorized, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_File_MetaData_From_Root_With_Illegal_endpoint_Does_Not_Work()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl, true, "Hugo", "Rabarber");

            string endpoint = "filesystem/file";

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.DeleteAsync(String.Format("{0}?path={1}", endpoint, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Unauthorized, clientTask.Result.StatusCode);
        }
    }
}
