using DemoProject.API.SwaggerExamples;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.API.Controllers
{
    /// <summary>
    /// Holds Admin endpoints. Should be using authentication and authorization.
    /// </summary>
    public class AdminController : ApiControllerBase
    {
        public AdminController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("verification", Name = "Verification")]
        [SwaggerRequestExample(typeof(VerificationRequest), typeof(VerificationRequestExample))]
        [SwaggerResponse(200, "Successful response", typeof(VerificationResponseDTO))]
        [SwaggerResponseExample(200, typeof(VerificationResponseExample))]
        public async Task<IActionResult> GetVerification([FromQuery] VerificationRequest request)
        {
            var result = await SendToHandler(request);
            return SendResponse(result);
        }
    }
}
