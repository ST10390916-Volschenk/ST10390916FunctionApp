using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ST10390916Function;
using System.Data;

namespace ST10390916Functions
{
    public class UploadFile
    {
        private readonly ILogger<UploadFile> _logger;

        public UploadFile(ILogger<UploadFile> logger)
        {
            _logger = logger;
        }

        [Function("UploadFile")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string shareName = req.Query["shareName"];
            string fileName = req.Query["fileName"];

            if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
            {
                return new BadRequestObjectResult("Share name and file name must be provided.");
            }

            try
            {
                var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                var shareServiceClient = new ShareServiceClient(connectionString);
                var shareClient = shareServiceClient.GetShareClient(shareName);
                await shareClient.CreateIfNotExistsAsync();
                var directoryClient = shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient(fileName);

                using var stream = req.Form.Files[0].OpenReadStream();
                if (stream == null || !stream.CanSeek || stream.Length == 0)
                {
                    return new BadRequestObjectResult("The stream is either null, non-seekable, or empty.");
                }
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);

                return new OkObjectResult("File uploaded to Azure Files");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}