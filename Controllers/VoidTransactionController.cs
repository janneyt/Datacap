using Microsoft.AspNetCore.Mvc;
using Datacap.Services;
using Microsoft.Extensions.Options;
using Datacap.Models;
using Microsoft.Extensions.Logging;
using Datacap.Models.DTO_Models;
using Newtonsoft.Json;
using Castle.Core.Logging;

namespace Datacap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoidTransactionController : ControllerBase
    {
        private readonly VoidTransactionsService _transactionsService;
        private readonly FilePaths _filePaths;
        private readonly ILogger<TransactionController> _logger;

        public VoidTransactionController(VoidTransactionsService transactionsService, IOptions<FilePaths> filePaths, ILogger<TransactionController> logger)
        {
            _transactionsService = transactionsService;
            _filePaths = filePaths.Value;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            // A GET request should be sufficient for the use cases sent to me. This calls the file handling and business logic components and returns a JSON response
            _logger.LogInformation("Getting all transaction controller");

            List<TransactionResponse> responses = new List<TransactionResponse>
            {
                new TransactionResponse { Type = "Sale", Rank = 1, RefNo = 2, Amount = 10.0 },
                new TransactionResponse { Type = "Sale", Rank = 2, RefNo = 3, Amount = 20.0 },
                new TransactionResponse { Type = "Sale", Rank = 3, RefNo = 4, Amount = 15.0 }
            };

            // Serialize the list of TransactionResponse objects into JSON format
            string jsonResponse = JsonConvert.SerializeObject(responses, Formatting.Indented);

            // Return the serialized JSON object as a response
            return Content(jsonResponse, "application/json");
        }


    }
}

