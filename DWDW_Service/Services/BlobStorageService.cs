using DWDW_Service.BlobConfig;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace DWDW_Service.Services
{
    public class BlobStorageService
    {
        string accessKey = "DefaultEndpointsProtocol=https;AccountName=dwx2storage;AccountKey=JnSKmfGqHO2uDWeytSUyqOsLCUiIgW5GflgccSwcbBvIIKD29PotPC2AUPU1DnW0hkT7+F7FQoAP2WN9jXVjQg==;EndpointSuffix=core.windows.net";

        public string UploadFileToBlob(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                var _task = Task.Run(() => this.UploadFileToBlobAsync(strFileName, fileData, fileMimeType));
                _task.Wait();
                string fileUrl = _task.Result;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }

        private async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string fileMimeType)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                string strContainerName = "dwimages";
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
                string fileName = this.GenerateFileName(strFileName);

                if (fileName != null && fileData != null)
                {
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                    cloudBlockBlob.Properties.ContentType = fileMimeType;
                    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                    return cloudBlockBlob.Uri.AbsoluteUri;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
