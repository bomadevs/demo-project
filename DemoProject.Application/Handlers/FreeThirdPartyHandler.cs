using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Infrastructure.Services;
using MediatR;

namespace DemoProject.Application.Handlers
{
    public class FreeThirdPartyHandler : IRequestHandler<FreeThirdPartyRequest, ApiResponseResult>
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;
        private readonly IFailureSimulationService _failureSimulationService;

        // 40% of the time, this endpoint should simulate a 503...
        private const int FailureRate = 40;

        public FreeThirdPartyHandler(IDataService dataService, IMapper mapper, IFailureSimulationService failureSimulationService)
        {
            _dataService = dataService;
            _mapper = mapper;
            _failureSimulationService = failureSimulationService;
        }

        public async Task<ApiResponseResult> Handle(FreeThirdPartyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // simulate failure based on custom failure rate
                if (_failureSimulationService.ShouldFail(FailureRate))
                {
                    return new ApiResponseResult(new ServiceUnavailableErrorResponse(""));
                }

                // query free companies using the service...
                var companies = await _dataService.GetFreeCompaniesByIdAsync(request.QueryInfo.Query, request.QueryInfo.IsActive);

                // map to DTO response format...
                var response = new FreeThirdPartyResponse
                {
                    Results = companies.Select(c => _mapper.Map<FreeThirdPartyResponseDTO>(c)),
                    Total = companies.Count
                };

                return new ApiResponseResult(response);
            }
            catch (Exception ex)
            {
                return new ApiResponseResult();
            }
        }
    }
}
