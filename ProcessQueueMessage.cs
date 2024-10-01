using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace ST10390916Functions
{
    public class ProcessQueueMessage
    {
        [Function("ProcessQueueMessage")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string queueName = req.Query["queueName"];
            string message = req.Query["message"];

            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
            {
                return new BadRequestObjectResult("Queue name and message must be provided.");
            }

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var queueServiceClient = new QueueServiceClient(connectionString);
            var queueClient = queueServiceClient.GetQueueClient(queueName);
            queueClient.CreateIfNotExistsAsync();
            queueClient.SendMessageAsync(message);

            return new OkObjectResult("Message added to queue");
        }
    }
}