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
            _logger.LogInformation("Getting all processors controller");
            var processors = await _transactionsService.GetAllProcessorsAsync();

            var responses = processors.Select(p => new ProcessorResponse
            {
                Name = p.ProcessorName,
                TotalFee = p.TotalFee,
            }).OrderByDescending(r => r.TotalFee).ThenBy(r => r.Name).ToList();

            // Rank processors based on total fee
            int rank = 1;
            for (int i = 0; i < responses.Count(); i++)
            {
                responses[i].Rank = rank++;
            }

            string jsonResponse = JsonConvert.SerializeObject(responses, Formatting.Indented);
            Console.WriteLine($"jsonResponse {jsonResponse}");
            return Content(jsonResponse, "application/json");
        }
    }
}

