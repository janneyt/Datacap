using Microsoft.AspNetCore.Mvc;
using Datacap.Services;
using Microsoft.Extensions.Options;
using Datacap.Models;
using Microsoft.Extensions.Logging;
using Datacap.Models.DTO_Models;
using Newtonsoft.Json;
using Castle.Core.Logging;
using System.Threading.Tasks;

namespace Datacap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionsService _transactionsService;
        private readonly FilePaths _filePaths;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(TransactionsService transactionsService, IOptions<FilePaths> filePaths, ILogger<TransactionController> logger)
        {
            _transactionsService = transactionsService;
            _filePaths = filePaths.Value;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Getting all transaction controller");

            // Call the AddAllTransactionsAsync method and get the transactions
            var transactions = await _transactionsService.AddAllTransactionsAsync();

            // Convert the transactions to your response model
            var responses = transactions.Select(t => new TransactionResponse
            {
                Type = t.TransactionType.TypeName,
                Rank = t.Rank,
                RefNo = t.TransactionID,
                Amount = t.Amount
            }).ToList();

            string jsonResponse = JsonConvert.SerializeObject(responses, Formatting.Indented);

            return Content(jsonResponse, "application/json");
        }

    }
}

