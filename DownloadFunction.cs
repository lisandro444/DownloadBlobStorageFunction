using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DownloadBlobFunction
{
    public static class DownloadFunction
    {
        [FunctionName("DownloadFile")]
        public static void Run([BlobTrigger("devcontainer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            StreamReader reader = new StreamReader(myBlob);

            CloudStorageAccount sourceAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudBlobClient sourceClient = sourceAccount.CreateCloudBlobClient();

            CloudBlobContainer sourceBlobContainer = sourceClient.GetContainerReference("devcontainer");

            var blob = sourceBlobContainer.GetBlockBlobReference(name);
            
            var localPath = @"C:\" + name;

            blob.DownloadToFileAsync(localPath, FileMode.Create);

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes \n Content of the file:{reader.ReadToEnd()} ");
        }
    }
}
