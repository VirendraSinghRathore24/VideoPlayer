using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoPlayerServer2
{
    public class StorageUploader
    {
        private static readonly string connectionString = "AccountEndpoint=https://appdb1.documents.azure.com:443/;AccountKey=FcGyp30wKyj4IbrrqDYu7Kq8bMKHw8j5mJYbhFoUGz2MFJcaiW6bBl9zQ7NhlD3UWzX0Kyuzc1R07cJBaZfA9A==;";
        private static readonly string dbName = "appdb1";
        private static readonly string containerName = "fileDetails";
        private static readonly string partitionKey = "/fileName";
        CosmosClient client = new CosmosClient(connectionString);

        public string Upload(string name, string size, string path)
        {
            try
            {
                client.CreateDatabaseIfNotExistsAsync(dbName).GetAwaiter().GetResult();
                Database database = client.GetDatabase(dbName);
                database.CreateContainerIfNotExistsAsync(containerName, partitionKey).GetAwaiter().GetResult();

                FileDetails fileDetails = new FileDetails()
                {
                    id = name,

                    fileName = name,
                    fileSize = size,
                    fileUploadedPath = path
                };

                Container container = client.GetContainer(dbName, containerName);
                container.CreateItemAsync<FileDetails>(fileDetails, new PartitionKey(fileDetails.fileName)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return "success";
            }

        public async System.Threading.Tasks.Task GetDetailsFromCosmosDBAsync()
        {
            Container container = client.GetContainer(dbName, containerName);

            var query = container.GetItemLinqQueryable<IDictionary<string, object>>()
                .Select(f => f["fileUploadedPath"]);
            var iterator = query.ToFeedIterator();
            while (iterator.HasMoreResults)
            {
                foreach (var document in await iterator.ReadNextAsync())
                {
                    Console.WriteLine(document.ToString());
                }
            }
        }
    }
    public class FileDetails
    {
        public string id { get; set; }
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string fileUploadedPath { get; set; }
    }
}