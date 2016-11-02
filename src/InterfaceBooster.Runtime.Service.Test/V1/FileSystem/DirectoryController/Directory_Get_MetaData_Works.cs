﻿using System;
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

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.DirectoryController
{
    [TestFixture]
    public class Directory_Get_MetaData_Works
    {
        #region CONSTANTS

        const string
            RESTROOT = "filesystem/directory";

        #endregion CONSTANTS

        #region MEMBERS

        private HttpClient _Client;

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            _Client = ClientServices.SetupHttpClient(Config.serviceUrl);

            TestEnvironnement.GetOrDeleteDirectories();
        }

        [Test]
        public void Get_Directory_MetaData_From_Root_Works()
        {
            var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOT, ""));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            HttpResponseMessage response = clientTask.Result;

            var contentReadingTask = response.Content.ReadAsAsync<DirectoryMetaDataDto>();
            contentReadingTask.Wait();

            DirectoryMetaDataDto directoryMetaData = contentReadingTask.Result;

            Assert.AreEqual("", directoryMetaData.Name);
            Assert.AreEqual("/", directoryMetaData.Path);

            FileMetaDataDto fileMetaData = directoryMetaData.NestedFiles.FirstOrDefault(nfl => nfl.Name == Config.MyPicture);

            Assert.AreNotEqual(null, fileMetaData);

            directoryMetaData = directoryMetaData.NestedDirectories.FirstOrDefault(ndr => ndr.Name == Config.LevelOne);

            Assert.AreNotEqual(null, directoryMetaData);

            fileMetaData = directoryMetaData.NestedFiles.FirstOrDefault(nfl => nfl.Name == Config.YourPicture);

            Assert.AreNotEqual(null, fileMetaData);

            directoryMetaData = directoryMetaData.NestedDirectories.FirstOrDefault(ndr => ndr.Name == Config.LevelTwo);

            Assert.AreNotEqual(null, directoryMetaData);
        }

        [Test]
        public void Get_Directory_MetaData_From_LevelOne_Works()
        {
            var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOT, Config.LevelOne));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            HttpResponseMessage response = clientTask.Result;

            var contentReadingTask = response.Content.ReadAsAsync<DirectoryMetaDataDto>();
            contentReadingTask.Wait();

            DirectoryMetaDataDto directoryMetaData = contentReadingTask.Result;

            Assert.AreEqual(Config.LevelOne, directoryMetaData.Name);
            Assert.AreEqual(Path.Combine("/", Config.LevelOne), directoryMetaData.Path);

            FileMetaDataDto fileMetaData = directoryMetaData.NestedFiles.FirstOrDefault(nfl => nfl.Name == Config.YourPicture);

            Assert.AreNotEqual(null, fileMetaData);

            directoryMetaData = directoryMetaData.NestedDirectories.FirstOrDefault(ndr => ndr.Name == Config.LevelTwo);

            Assert.AreNotEqual(null, directoryMetaData);
        }

        [Test]
        public void Get_Directory_MetaData_From_LevelTwo_Works()
        {
            var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOT, String.Format("/{0}/{1}",  Config.LevelOne, Config.LevelTwo)));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            HttpResponseMessage response = clientTask.Result;

            var contentReadingTask = response.Content.ReadAsAsync<DirectoryMetaDataDto>();
            contentReadingTask.Wait();

            DirectoryMetaDataDto directoryMetaData = contentReadingTask.Result;

            Assert.AreEqual(Config.LevelTwo, directoryMetaData.Name);
            Assert.AreEqual(String.Format("/{0}/{1}",  Config.LevelOne, Config.LevelTwo), directoryMetaData.Path);
        }

        [Test]
        public void Get_Not_Existing_Directory_MetaData_Doesn_T_Work()
        {
            var clientTask = _Client.GetAsync(String.Format("{0}?path=/{1}", RESTROOT, "root"));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        [Test]
        public void Get_Directory_MetaData_Without_Path_Declaration_Doesn_T_Work()
        {
            var clientTask = _Client.GetAsync(RESTROOT);
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.InternalServerError, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS
    }
}