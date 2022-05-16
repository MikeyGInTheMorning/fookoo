using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;
using System.Collections.Generic;

namespace Fookoo.Web
{
    public static class GetSayingsFunction
    {
        [FunctionName("GetSayingFunction")]
        public static async Task<IActionResult> GetSaying(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["hash"];

        
            await GetAzureServices();
            await UploadToBlob();
            var responseMessage = "\n\n" + await ListBlobs();
            await DownloadFromBlob();
            return new OkObjectResult(responseMessage);
        }


        static string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        static string containerName = "sayings1";
        static string blobName = "data.json";
        static BlobContainerClient containerClient;
        static BlobServiceClient blobServiceClient;
        static BlobClient blobClient;

        private static async Task GetAzureServices()
        {
            // Create a BlobServiceClient object which will be used to create a container client
            blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            if (!await containerClient.ExistsAsync())
                containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            // Get a reference to a blob
            blobClient = containerClient.GetBlobClient(blobName);
        }

        private static async Task<string> ListBlobs()
        {
            var retVal = string.Empty;
            retVal += "Listing blobs...\n";

            // List all blobs in the container
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                retVal += "\t" + blobItem.Name;
            }

            Console.Write(retVal);
            return retVal;
        }

        private static async Task UploadToBlob()
        {
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(myDataObject)))
            {
                await blobClient.UploadAsync(ms, overwrite: true);
            }
        }

        private static async Task DownloadFromBlob()
        {
            Console.WriteLine("\nDownloading blob to\n\t{0}\n", blobName);

            // Download the blob's contents and save it to a file
            using (var stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;//resetting stream's position to 0
                var serializer = new JsonSerializer();

                using var sr = new StreamReader(stream);
                using var jsonTextReader = new JsonTextReader(sr);
                var result = serializer.Deserialize<List<Saying>>(jsonTextReader);

                Console.WriteLine("\nDownloading blob to\n\t{0}\n", result);
            }
        }

        public class Root
        {
            [JsonProperty("sayings")]
            List<Saying> Sayings { get; set; }
        }

        public class Saying
        {
            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("sentence")]
            public string Sentence { get; set; }
        }



        private static string myDataObject = @"
{
  ""sayings"": [
    {
      ""hash"": ""1"",
      ""saying"": ""I Love You.""
    },
    {
      ""hash"": ""2"",
      ""saying"": ""I Love You 2.""
    }
  ]
}
            ";
    }
}

