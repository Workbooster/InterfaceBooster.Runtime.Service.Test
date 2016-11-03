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
    public class Directory_Delete_Works
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

            TestEnvironnement.DeleteDerictories();
        }

        [Test]
        public void Delete_Directory_In_Root_Works()
        {
            var clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, Config.PremierNiveau));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        [Test]
        public void Delete_A_Not_Empty_Directory_In_Root_Works()
        {
            var clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, Config.LivelloUno));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        [Test]
        public void Delete_A_SubDirectory_Works()
        {
            var clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, String.Format("{0}/{1}", Config.ErsteStufe, Config.ZweiteStufe)));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        [Test]
        public void Delete_A_Wrong_Addressed_SubDirectory_Doesn_t_Work()
        {
            var clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, Config.DeuxiemeNiveau));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.NotFound, clientTask.Result.StatusCode);
        }

        [Test]
        public void Delete_Another_Directory_In_Root_Works()
        {
            var clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, Config.ErsteStufe));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);

            clientTask = _Client.DeleteAsync(String.Format("{0}?path=/{1}", ENDPOINT, Config.PremierNiveau));
            clientTask.Wait();

            Assert.AreEqual(HttpStatusCode.OK, clientTask.Result.StatusCode);
        }

        #endregion PUBLIC METHODS
    }
}
