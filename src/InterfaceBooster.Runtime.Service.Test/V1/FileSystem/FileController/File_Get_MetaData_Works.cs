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
    public class File_Get_MetaData_Works
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
            _Client = ClientServices.SetupHttpClient(Config.serviceUrl);

            TestEnvironnement.GetOrDeleteFiles();
        }

        [Test]
        public void Get_File_MetaData_From_Root_Works()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            HttpResponseMessage response = clientTask.Result;

            var contentReadingTask = response.Content.ReadAsAsync<FileMetaDataDto>();
            contentReadingTask.Wait();

            FileMetaDataDto fileMetaData = contentReadingTask.Result;

            Assert.AreEqual(fileName, fileMetaData.Name);
            Assert.AreEqual(filePath, fileMetaData.Path);

            FileInfo fileInfo = new FileInfo(Path.Combine(Config.LocalFiles, Config.MyPicture));

            Assert.AreEqual(fileInfo.Length, fileMetaData.Size);
        }

        [Test]
        public void Get_Not_Existing_File_MetaData_From_Root_Doesn_T_Work()
        {
            string fileName = "anyPicture";
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_File_MetaData_From_LevelOne_Works()
        {
            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}/{1}", Config.LevelOne, fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            HttpResponseMessage response = clientTask.Result;

            var contentReadingTask = response.Content.ReadAsAsync<FileMetaDataDto>();
            contentReadingTask.Wait();

            FileMetaDataDto fileMetaData = contentReadingTask.Result;

            Assert.AreEqual(fileName, fileMetaData.Name);
            Assert.AreEqual(filePath, fileMetaData.Path);

            FileInfo fileInfo = new FileInfo(Path.Combine(Config.LocalFiles, Config.MyPicture));

            Assert.AreEqual(fileInfo.Length, fileMetaData.Size);
        }

        [Test]
        public void Get_Not_Existing_File_MetaData_From_LevelOne_Doesn_T_Work()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}/{1}", Config.LevelOne,fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, Config.LevelOne, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS
    }
}
