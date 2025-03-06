using DemoProject.API.SwaggerExamples;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DemoProject.DemoProject.API.Controllers
{
    /// <summary>
    /// Holds our endpoints for the companies.
    /// </summary>
    public class CompaniesController : ApiControllerBase
    {
        public CompaniesController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("free-third-party", Name = "FreeThirdParty")]
        public async Task<IActionResult> GetFreeThirdParty([FromQuery] string query)
        {
            var request = new FreeThirdPartyRequest(new QueryInfo(query));
            var result = await SendToHandler(request);

            return SendResponse(result);
        }

        [HttpGet("premium-third-party", Name = "PremiumThirdParty")]
        public async Task<IActionResult> GetPremiumThirdParty([FromQuery] string query)
        {
            var request = new PremiumThirdPartyRequest(new QueryInfo(query));
            var result = await SendToHandler(request);

            return SendResponse(result);
        }

        [HttpGet("backend-service", Name = "BackendService")]
        [SwaggerRequestExample(typeof(BackendServiceRequest), typeof(BackendServiceRequestExample))]
        [SwaggerResponse(200, "Successful response", typeof(BackendServiceResponse))]
        [SwaggerResponseExample(200, typeof(BackendServiceResponseExample))]
        public async Task<IActionResult> GetBackendService([FromQuery] BackendServiceRequest request)
        {
            var result = await SendToHandler(request);

            return SendResponse(result);
        }
    }
}
