using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ST10390916Function
{
    public class UploadCustomer
    {
        private readonly ILogger<UploadCustomer> _logger;

        public UploadCustomer(ILogger<UploadCustomer> logger)
        {
            _logger = logger;
        }

        [Function("UploadCustomer")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string partitionKey = req.Query["partitionKey"];
            string rowKey = req.Query["rowKey"];
            string firstName = req.Query["firstName"];
            string lastName = req.Query["lastName"];
            string phoneNumber = req.Query["phoneNumber"];
            string email = req.Query["Email"];

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            TableClient tableClient = new TableClient("DefaultEndpointsProtocol=https;AccountName=st10390916storage;AccountKey=Q6z1hLCZQ/fOwujI8LODTtgJAqf9f3uDRL34MXsSSF6XHFO74qZjtJntWuE5xbgNYSVBPJkKAAMr+AStwFL5TQ==;EndpointSuffix=core.windows.net", "customers");
            tableClient.AddEntityAsync(new TableEntity
            {
                ["PartitionKey"] = partitionKey,
                ["RowKey"] = rowKey,
                ["FirstName"] = firstName,
                ["LastName"] = lastName,
                ["PhoneNumber"] = phoneNumber,
                ["Email"] = email
            });
            return new OkObjectResult("Data stored in Azure Table");
        }
    }
}
