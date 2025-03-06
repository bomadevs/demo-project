using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Infrastructure.Services;
using MediatR;

namespace DemoProject.Application.Handlers
{
    public class PremiumThirdPartyHandler : IRequestHandler<PremiumThirdPartyRequest, ApiResponseResult>
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;
        private readonly IFailureSimulationService _failureSimulationService;

        // 10% of the time, this endpoint should simulate a 503...
        private const int FailureRate = 10;

        public PremiumThirdPartyHandler(IDataService dataService, IMapper mapper, IFailureSimulationService failureSimulationService)
        {
            _dataService = dataService;
            _mapper = mapper;
            _failureSimulationService = failureSimulationService;
        }

        public async Task<ApiResponseResult> Handle(PremiumThirdPartyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // simulate failure based on custom failure rate
                if (_failureSimulationService.ShouldFail(FailureRate))
                {
                    return new ApiResponseResult(new ServiceUnavailableErrorResponse(""));
                }

                // query premium companies using the service...
                var companies = await _dataService.GetPremiumCompaniesByIdAsync(request.QueryInfo.Query, request.QueryInfo.IsActive);

                // map to DTO response format...
                var response = new PremiumThirdPartyResponse
                {
                    Results = companies.Select(c => _mapper.Map<PremiumThirdPartyResponseDTO>(c)),
                    Total = companies.Count
                };

                return new ApiResponseResult(response);
            }
            catch
            {
                return new ApiResponseResult();
            }
        }
    }
}
