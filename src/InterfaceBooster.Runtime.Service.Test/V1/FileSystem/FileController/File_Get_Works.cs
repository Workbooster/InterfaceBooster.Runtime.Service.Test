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
    public class File_Get_Works
    {
        #region CONSTANTS

        const string
            RESTROOT = "filesystem/file/download",
            RESTROOTMETADATA = "filesystem/file";

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
        public void Get_File_Data_From_Root_Works()
        {
            string fileName = Config.MyPicture;
            string filePath = String.Format("/{0}", fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            byte[] fileData = clientTask.Result.Content.ReadAsByteArrayAsync().Result;


            // Write received data

            string fileResultPath = String.Format(@"{0}\{1}", Config.LocalResultPath, fileName);
            File.WriteAllBytes(fileResultPath, fileData);


            //Compare

            FileInfo fileInfoResult = new FileInfo(fileResultPath);
            FileInfo fileInfoSource = new FileInfo(Path.Combine(Config.LocalFiles, Config.MyPicture));

            Assert.AreEqual(fileInfoSource.Length, fileInfoResult.Length);
       }

        [Test]
        public void Get_NOT_EXISTING_File_Data_From_Root_Doesn_T_Work()
        {
            string fileName = "anyPicture";

            var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOT, fileName));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_File_Data_From_LevelOne_Works()
        {
            string fileName = Config.YourPicture;
            string filePath = String.Format("/{0}/{1}", Config.LevelOne, fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            byte[] fileData = clientTask.Result.Content.ReadAsByteArrayAsync().Result;


            // Write received data

            string fileResultPath = String.Format(@"{0}\{1}", Config.LocalResultPath, fileName);
            File.WriteAllBytes(fileResultPath, fileData);


            //Compare

            FileInfo fileInfoResult = new FileInfo(fileResultPath);
            FileInfo fileInfoSource = new FileInfo(Path.Combine(Config.LocalFiles, Config.MyPicture));

            Assert.AreEqual(fileInfoSource.Length, fileInfoResult.Length);
        }

        [Test]
        public void Get_NOT_EXISTING_File_Data_From_LevelOne_Doesn_T_Work()
        {
            string fileName = "anyPicture";
            string filePath = String.Format("/{0}/{1}", Config.LevelOne, fileName);

            var clientTask = _Client.GetAsync(String.Format("{0}?path={1}", RESTROOT, filePath));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS

        #region INTERNAL METHODS

        //private FileMetaDataDto FileMetaData(string fileSystemFilePath)
        //{
        //    //var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOTMETADATA, fileName));
        //    //clientTask.Wait();

        //    //HttpResponseMessage response = clientTask.Result;

        //    //var contentReadingTask = response.Content.ReadAsAsync<FileMetaDataDto>();
        //    //contentReadingTask.Wait();

        //    FileInfo fileInfo = new FileInfo(Path.Combine(Config.FileSystemPath, fileSystemFilePath.Replace("/", @"\")));




        //    return null;
        //    //contentReadingTask.Result;
        //}
        #endregion INTERNAL METHODS


    }
}
