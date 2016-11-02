using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using InterfaceBooster.Runtime.Common.Interfaces.Model.Service.FileSystem;
using Helpers = InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers;
using NUnit.Framework;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.FileController
{
    [TestFixture]
    public class File_Post_Works
    {
        #region CONSTANTS

        const string
            RESTROOT = "filesystem/file";

        #endregion CONSTANTS

        #region MEMBERS

        private HttpClient _Client;

        private FileMetaDataDto _FileMetaData;
        private FileTransferDataDto _FileTransferredData;

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            _Client = Helpers.ClientServices.SetupHttpClient(Config.serviceUrl);

            Helpers.TestEnvironnement.PostFiles();
        }

        [Test]
        public void Post_File_To_Root_With_Same_Name_Works()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            SetUpFileMetaData(filePath, fileName, fileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            Assert.IsTrue(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));
        }

        [Test]
        public void Post_File_To_Root_With_Other_Name_Works()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}", fileName);

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            Assert.IsTrue(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));
        }

        [Test]
        public void Post_File_To_LevelOne_With_Other_Name_Works()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}/{1}", Config.LevelOne, fileName);

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            Assert.IsTrue(File.Exists(
                Path.Combine(Config.FileSystemPath, filePath.TrimStart('/').Replace("/", @"\"))));
        }

        [Test]
        public void Post_File_To_LevelTwo_With_Same_Name_Works()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}/{1}/{2}", Config.LevelOne, Config.LevelTwo, fileName);

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        [Test]
        public void Post_File_To_Not_Existing_LevelThree_With_Same_Name_Doesn_T_Work()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}/{1}/{2}/{3}", Config.LevelOne, Config.LevelTwo, "LevelThree", fileName);

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotAcceptable, clientTask.Result.StatusCode);
        }

        [Test]
        public void Post_File_To_Root_With_Inconsitent_Data_Doesn_T_Work()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            _FileTransferredData.Name = Config.YourPicture;

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.InternalServerError, clientTask.Result.StatusCode);
        }

        [Test]
        public void Post_File_To_Root_With_Illegal_Name_Doesn_T_Work()
        {
            string sourceFileName = Config.MyPicture;

            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", "my:Picture");

            SetUpFileMetaData(filePath, fileName, sourceFileName);

            MultipartFormDataContent content =
                Helpers.MultipartFileData.singleFile(_FileMetaData, _FileTransferredData);

            var clientTask = _Client.PostAsync(RESTROOT, content);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.InternalServerError, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS

        #region INTERNAL METHODS

        private void SetUpFileMetaData(string postFilePath, string postFileName, string sourceFileName)
        {
            string sourceFilePath = String.Format(@"{0}\{1}", Config.LocalFiles, sourceFileName);


            // File MetaData

            FileInfo fileInfo = new FileInfo(sourceFilePath);

            _FileMetaData = new FileMetaDataDto
            {
                Name = postFileName,
                Path = postFilePath,
                Size = Convert.ToInt32(fileInfo.Length),
                DateOfCreation = fileInfo.CreationTime,
                DateOfLastChange = fileInfo.LastWriteTime
            };


            // File content

            byte[] fileBytes = File.ReadAllBytes(sourceFilePath);

            _FileTransferredData = new FileTransferDataDto
            {
                Name = postFileName,
                Content = fileBytes
            };
        }

        #endregion INTERNAL METHODS

    }
}
