using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace blob_consumer
{
    public static class blob_consumer
    {
        [FunctionName("blob_consumer")]
        public static async Task Run([BlobTrigger("container-name/%BLOB_SUB_PATH%{name}", Connection = "TEST_STORAGE_CONNECTION_STRING")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            string connectionString = Environment.GetEnvironmentVariable("TEST_STORAGE_CONNECTION_STRING");
            string blobSubPath = Environment.GetEnvironmentVariable("BLOB_SUB_PATH");
          
            var _blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
			var _container = _blobClient.GetContainerReference("container-name");
		 
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobSubPath+name);
            
            log.LogInformation($"Deleting blob {name}");
			// Delete the blob.
			await blockBlob.DeleteIfExistsAsync();
            
            
        }
    }
}
