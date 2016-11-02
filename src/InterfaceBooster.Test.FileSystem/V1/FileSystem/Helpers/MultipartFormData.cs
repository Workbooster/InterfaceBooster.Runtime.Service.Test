using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Runtime.Common.Interfaces.Model.Service.FileSystem;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace InterfaceBooster.Runtime.Service.Test.V1.FileSystem.Helpers
{
    public static class MultipartFileData
    {
        public static MultipartFormDataContent singleFile(FileMetaDataDto fileMetaData, FileTransferDataDto fileData)
        {
            MultipartFormDataContent content =
                new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));

            FileMultipartClientDto transferDto = new FileMultipartClientDto
            {
                MetaData = fileMetaData,
                Data = fileData
            };

            string createDtoJason = JsonConvert.SerializeObject(transferDto);
            content.Add(new StringContent(createDtoJason, Encoding.UTF8, "application/json"), "binaryfile");

            content.Add(new ByteArrayContent(fileData.Content), fileData.Name);

            return content;
        }
    }
}
