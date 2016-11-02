using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using InterfaceBooster.Runtime.Common.Interfaces.Model.Service.FileSystem;
using InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers;
using NUnit.Framework;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.FileController
{
    [TestFixture]
    public class NUTestModule
    {
        #region CONSTANTS

        const string
            RESTROOT = "filesystem/file";

        #endregion CONSTANTS

        #region MEMBERS

        private HttpClient _Client;

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl);

            TestEnvironnement.GetOrDeleteFiles();
        }

        [Test]
        public void Delete_File_From_Root_Works()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            Assert.IsTrue(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));

            var clientTask = _Client.DeleteAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            Assert.IsFalse(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));
        }

        [Test]
        public void Delete_Not_Existing_File_From_Root_Doesn_T_Work()
        {
            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.DeleteAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        [Test]
        public void Delete_File_From_LevelTwo_Works()
        {
            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}/{1}/{2}", Config.LevelOne, Config.LevelTwo, fileName);

            Assert.IsTrue(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));

            var clientTask = _Client.DeleteAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            Assert.IsFalse(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));
        }

        [Test]
        public void Delete_Not_Existing_File_From_LevelTwo_Doesn_T_Work()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}/{1}/{2}", Config.LevelOne, Config.LevelTwo, fileName);

            var clientTask = _Client.DeleteAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS
    }
}
