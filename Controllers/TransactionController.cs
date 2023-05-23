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
        public IActionResult Get()
        {
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


        // GET api/<TransactionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TransactionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TransactionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TransactionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
