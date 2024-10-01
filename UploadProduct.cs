using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ST10390916Function
{
    public class UploadProduct
    {
        private readonly ILogger<UploadProduct> _logger;

        public UploadProduct(ILogger<UploadProduct> logger)
        {
            _logger = logger;
        }

        [Function("UploadProduct")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string partitionKey = req.Query["partitionKey"];
            string rowKey = req.Query["rowKey"];
            string productName = req.Query["productName"];
            string productCode = req.Query["productCode"];
            string price = req.Query["price"];
            string productWeight = req.Query["productWeight"];

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            TableClient tableClient = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10390916storage;" +
                "AccountKey=Q6z1hLCZQ/fOwujI8LODTtgJAqf9f3uDRL34MXsSSF6XHFO74qZjtJntWuE5xbgNYSVBPJkKAAMr+AStwFL5TQ==;" +
                "EndpointSuffix=core.windows.net", "products");
            tableClient.AddEntityAsync(new TableEntity
            {
                ["PartitionKey"] = partitionKey,
                ["RowKey"] = rowKey,
                ["ProductName"] = productName,
                ["ProductCode"] = productCode,
                ["Price"] = price,
                ["ProductWeight"] = productWeight
            });
            return new OkObjectResult("Data stored in Azure Table");
        }
    }
}
