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
        //public static void GetDirectory(FileTransferDataDto fileContents)
        //{
        //    Get<FileTransferDataDto>("filesystem/directory", fileContents);
        //}

        //public static FileTransferDataDto PostFile(FileTransferDataDto fileContents)
        //{
        //    return Post<FileTransferDataDto>("filesystem/file", fileContents);
        //}

        //private static TContent Get<TContent>(string endpoint, TContent content)
        //{
        //    using (var client = SetupHttpClient())
        //    {
        //        //var clientTask = client.GetAsJsonAsync("filesystem/directory", content);
        //        var clientTask = client.GetAsync(endpoint);
        //        clientTask.Wait();

        //        HttpResponseMessage response = clientTask.Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            try
        //            {
        //                var contentReadingTask = response.Content.ReadAsAsync<TContent>();
        //                contentReadingTask.Wait();
        //                return contentReadingTask.Result;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(String.Format("Error while reading the response from GET request to {0}/{1}. Message: {2}", client.BaseAddress, endpoint, ex.Message), ex);
        //            }
        //        }
        //        throw new Exception(String.Format("Error response from server while running GET request to {0}/{1}. Reason: {2}", client.BaseAddress, endpoint, response.ReasonPhrase));
        //    }
        //}

        //private static TContent Post<TContent>(string endpoint, TContent content)
        //{
        //    using (var client = SetupHttpClient())
        //    {
        //        var clientTask = client.PostAsJsonAsync(endpoint, content);
        //        clientTask.Wait();

        //        HttpResponseMessage response = clientTask.Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            try
        //            {
        //                var contentReadingTask = response.Content.ReadAsAsync<TContent>();
        //                contentReadingTask.Wait();
        //                return contentReadingTask.Result;
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(String.Format("Error while reading the response from GET request to {0}/{1}. Message: {2}", client.BaseAddress, endpoint, ex.Message), ex);
        //            }
        //        }
        //        throw new Exception(String.Format("Error response from server while running GET request to {0}/{1}. Reason: {2}", client.BaseAddress, endpoint, response.ReasonPhrase));
        //    }
        //}

        public static HttpClient SetupHttpClient(string baseUri)
        {
            var client = new HttpClient();

            // string baseUri = "http://localhost:63110";

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static FileMetaDataDto FileMetaData(string fileSystemFilePath)
        {
            fileSystemFilePath = fileSystemFilePath.TrimStart(new char[] {'/'}).Replace("/", @"\");

            return FillFileMetaData(
                Path.Combine(Config.FileSystemPath, fileSystemFilePath));
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
                    Size = Convert.ToInt32(fileInfo.Length),
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
