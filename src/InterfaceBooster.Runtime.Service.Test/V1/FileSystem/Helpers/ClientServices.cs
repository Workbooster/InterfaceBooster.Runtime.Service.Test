using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using InterfaceBooster.Runtime.Common.Interfaces.Model.Service.FileSystem;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers
{
    public static class ClientServices
    {

        public static HttpClient SetupHttpClient(
            string baseUri, bool basicAutentication = false, string userName = "", string password = "")
        {
            HttpClient client;

            if (!basicAutentication)
            {
                client = new HttpClient();
            }
            else
            {
                var authValue =
                    new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(String.Format("{0}:{1}", userName, password))));

                HttpClientHandler handler = new HttpClientHandler();

                client = new HttpClient(handler)
                {
                    DefaultRequestHeaders = { Authorization = authValue }
                    //Set some other client defaults like timeout / BaseAddress
                };

                //------------------------------------------------------------------------

                //HttpClientHandler handler = new HttpClientHandler
                //{
                //    Credentials = new
                //        System.Net.NetworkCredential(userName, password)
                //};
                //client = new HttpClient(handler);
            }

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static FileMetaDataDto FileMetaData(string fileSystemFilePath)
        {
            fileSystemFilePath = fileSystemFilePath.TrimStart(new char[] { '/' }).Replace("/", @"\");

            return FillFileMetaData(
                Path.Combine(Config.FileSystemPath, fileSystemFilePath));
        }

        public static string SystemIoPath(string fileSystemPath)
        {
            return
                Path.Combine(Config.FileSystemPath, fileSystemPath.TrimStart('/').Replace("/", @"\"));
        }


        #region INTERNAL METHODS

        private static FileMetaDataDto FillFileMetaData(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            return
                new FileMetaDataDto
                {
                    Name = fileInfo.Name,
                    Path = FileSystemPath(fileInfo.FullName),
                    Size = fileInfo.Length,
                    DateOfCreation = fileInfo.CreationTime,
                    DateOfLastChange = fileInfo.LastWriteTime
                };
        }

        public static string FileSystemPath(string SystemIoPath)
        {
            int rootPos = SystemIoPath.IndexOf(Config.FileSystemDirectory) + Config.FileSystemDirectory.Length;

            return ToFileSystemPath(SystemIoPath.Substring(rootPos));
        }

        public static string ToFileSystemPath(string interfaceBoosterPath)
        {
            return
                interfaceBoosterPath.Replace(@"\", "/");
        }

        #endregion INTERNAL METHODS


    }
}
