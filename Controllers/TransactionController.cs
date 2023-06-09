﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly List<TransactionDTO> _transactionRepository;

        public TransactionController(TransactionsService transactionsService, IOptions<FilePaths> filePaths, ILogger<TransactionController> logger)
        {
            _transactionsService = transactionsService;
            _filePaths = filePaths.Value;
            _logger = logger;
        }

        // This gets the "leaderboard", i.e. just the processors ranked by total fees

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var processors = await _transactionsService.GetAllProcessorsAsync(false);

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
            return Content(jsonResponse, "application/json");
        }

        // This gets the leaderboard and all transactions
        [HttpGet("AllResults")]
        public async Task<IActionResult> GetFullResults()
        {
            var processors = await _transactionsService.GetAllProcessorsAsync(false);

            var responses = processors.Select(p => new ProcessorResponse
            {
                Name = p.ProcessorName,
                TotalFee = p.TotalFee,
                Transactions = p.Transactions
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

        [HttpPost("void")]
        public async Task<IActionResult> Void()
        {
            await _transactionsService.ProcessTransactionsAsync(true);

            var processors = await _transactionsService.GetAllProcessorsAsync(true);

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
            return Content(jsonResponse, "application/json");
        }
    }
}

