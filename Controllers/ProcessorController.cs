using Datacap.Models.DTO_Models;
using Datacap.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ProcessorController : ControllerBase
{
    private readonly ProcessorService _processorService;

    public ProcessorController(ProcessorService processorService)
    {
        _processorService = processorService;
    }

    [HttpPost]
    public IActionResult AddProcessor([FromBody] FeeRuleDTO feeRuleDto)
    {
        _processorService.AddProcessor(feeRuleDto);
        return Ok();
    }
}

