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
using System.Linq;

namespace Fookoo.Web
{
    public static class GetSayingsFunction
    {
        [FunctionName("GetSaying")]
        public static async Task<IActionResult> GetSaying(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = null)] HttpRequest req,
            ILogger log)
        {
            var hash = req.Query["hash"];
            var isHashValid = !string.IsNullOrEmpty(hash);

            await GetAzureServices();
            var sayings = await DownloadFromBlob();
            var isSayingsValid = sayings is not null || sayings.Count != 0;

            if (!isSayingsValid || !isHashValid)
                return new OkObjectResult(new Saying());

            var saying =  sayings.FirstOrDefault(a => a.Hash.ToLower() == hash.ToString().ToLower()) ?? new Saying();

            return new OkObjectResult(saying);
        }

        public class AddSayingRequest
        {
            [JsonProperty("sentence")]
            public string Sentence { get; set; }
        }

        [FunctionName("AddSaying")]
        public static async Task<IActionResult> AddSaying(
    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
    ILogger log)
        {
            await GetAzureServices();
            var sayings = await DownloadFromBlob();

            if (sayings is not null && sayings.Count == 10000)
                return new BadRequestObjectResult("Exceeeded custom syaing cap, try again tomorrow!");


            var content = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<AddSayingRequest>(content);

            var newId = Guid.NewGuid().ToString();
            sayings.Add(new()
            {
                Hash = newId,
                Sentence = body.Sentence
            });

            await UploadToBlob(sayings);

            var responseMessage = $"Saved Saying! Your Hash is {newId}";
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

        private static async Task UploadToBlob(object uploadObject)
        {
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);
            var data = JsonConvert.SerializeObject(uploadObject);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                await blobClient.UploadAsync(ms, overwrite: true);
            }
        }

        private static async Task<List<Saying>> DownloadFromBlob()
        {
            Console.WriteLine("\nDownloading blob to\n\t{0}\n", blobName);

            // Download the blob's contents and save it to a file
            using var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;//resetting stream's position to 0
            var serializer = new JsonSerializer();

            using var sr = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(sr);

            var result = serializer.Deserialize<List<Saying>>(jsonTextReader) ?? new();

            Console.WriteLine("\nDownloading blob to\n\t{0}\n", result);

            return result;
        }

        [JsonObject]
        public class Saying
        {
            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("sentence")]
            public string Sentence { get; set; }
        }

        private static List<Saying> testObj = new List<Saying>()
        {
            new()
            {
                Hash ="1",
                Sentence = "I Love You."
            },
            new()
            {
                Hash ="2",
                Sentence = "I Love You 2."
            }
        };
    }
}

