using Datacap.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Datacap.Middleware;

namespace Datacap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/Error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorMessage = context?.Error.Message ?? "An unexpected error occurred.";
            var statusCode = context?.Error is HttpException httpException
                ? httpException.StatusCode
                : StatusCodes.Status500InternalServerError;

            var result = new ErrorModel
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };

            return StatusCode(statusCode, result);
        }
    }
}

