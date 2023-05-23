using Microsoft.AspNetCore.Mvc;
using Datacap.Services;
using Microsoft.Extensions.Options;
using Datacap.Models;
using Microsoft.Extensions.Logging;

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

        // GET: api/<TransactionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Getting all transaction controller");
            return new string[] { "value1", "value2" };
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
