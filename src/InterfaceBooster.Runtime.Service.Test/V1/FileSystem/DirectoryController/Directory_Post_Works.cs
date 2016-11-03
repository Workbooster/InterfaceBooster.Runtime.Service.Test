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

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.DirectoryController
{
    [TestFixture]
    public class Directory_Post_Works
    {
        #region CONSTANTS

        const string
            ENDPOINT = "filesystem/directory";

        #endregion CONSTANTS

        #region MEMBERS

        private HttpClient _Client;

        #endregion MEMBERS

        #region PUBLIC METHODS

        [SetUp]
        public void SetupTest()
        {
            _Client = ClientServices.SetupHttpClient(Config.ServiceUrl);

            TestEnvironnement.PostDirectories();
        }

        [Test]
        public void Post_Directory_To_Root_Works()
        {
            string path = Config.PremierNiveau;

            var clientTask = _Client.PostAsJsonAsync(ENDPOINT, FillMetaData(path));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Created, clientTask.Result.StatusCode);

            Assert.IsTrue(Directory.Exists(ClientServices.SystemIoPath(path)));
        }

        [Test]
        public void Post_Directory_To_Yet_Existing_Premiere_Niveau_Works()
        {
            string path = String.Format("{0}/{1}", Config.PremierNiveau, Config.DeuxiemeNiveau);

            var clientTask = _Client.PostAsJsonAsync(ENDPOINT, FillMetaData(path));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Created, clientTask.Result.StatusCode);

            Assert.IsTrue(Directory.Exists(ClientServices.SystemIoPath(path)));
        }

        [Test]
        public void Post_Yet_Existing_Directory_Deuxieme_Niveau_Works()
        {
            string path = String.Format("{0}/{1}", Config.PremierNiveau, Config.DeuxiemeNiveau);

            var clientTask = _Client.PostAsJsonAsync(ENDPOINT, FillMetaData(path));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Created, clientTask.Result.StatusCode);

            Assert.IsTrue(Directory.Exists(ClientServices.SystemIoPath(path)));

            clientTask = _Client.PostAsJsonAsync(ENDPOINT, FillMetaData(path));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Accepted, clientTask.Result.StatusCode);
        }

        [Test]
        public void Post_Not_Yet_Existing_Sub_Directory_Livello_Livello_Due_Works()
        {
            string path = String.Format("{0}/{1}", Config.LivelloUno, Config.LivelloDue);

            var clientTask = _Client.PostAsJsonAsync(ENDPOINT, FillMetaData(path));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.Created, clientTask.Result.StatusCode);

            Assert.IsTrue(Directory.Exists(ClientServices.SystemIoPath(path)));
        }

        [Test]
        public void Post_Directory_Without_Path_Declaration_Doesn_T_Work()
        {
            var clientTask = _Client.PostAsJsonAsync(ENDPOINT, new DirectoryMetaDataDto());
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.InternalServerError, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS

        #region INTERNAL METHODS

        private DirectoryMetaDataDto FillMetaData(string path)
        {
            return new DirectoryMetaDataDto
            {
                Name = path.Substring(path.LastIndexOf("/") + 1),
                Path = path
            };
        }

        #endregion INTERNAL METHODS

    }
}
