using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Domain.Entities;
using DemoProject.Infrastructure.Services;
using MediatR;
using Polly;

namespace DemoProject.Application.Handlers
{
    public class BackendServiceHandler : IRequestHandler<BackendServiceRequest, ApiResponseResult>
    {
        private readonly IMediator _mediator;
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public BackendServiceHandler(IMediator mediator, IDataService dataService, IMapper mapper)
        {
            _mediator = mediator;
            _dataService = dataService;
            _mapper = mapper;
        }

        public async Task<ApiResponseResult> Handle(BackendServiceRequest request, CancellationToken cancellationToken)
        {
            // fetch only active companies from services...
            var queryInfo = new QueryInfo(request.Query, true);

            // here we will use Polly (a .NET resilience and transient-fault-handling library that allows
            // to define policies for retrying, circuit-breaking, timeouts...
            var policy = Policy<ApiResponseResult>
                .Handle<HttpRequestException>()                                     // handle network-related issues...
                .OrResult(r => r == null || !r.HasResults)                          // handle empty results...
                .OrResult(r => r.IsFailure)                                         // handle 503...
                .RetryAsync(0)                                                      // retry 0 times in our case and just fall back...
                .WrapAsync(
                    Policy<ApiResponseResult>
                        .Handle<HttpRequestException>()
                        .OrResult(r => r == null || !r.HasResults || r.IsFailure)
                        .FallbackAsync(async (c) =>
                        {
                            var fallbackResult = await _mediator.Send(new PremiumThirdPartyRequest(queryInfo), c);
                            return fallbackResult;
                        })
                );

            // execute the first request with the defined policy
            var result = await policy.ExecuteAsync(async () =>
            {
                var freeServiceResult = await _mediator.Send(new FreeThirdPartyRequest(queryInfo), cancellationToken);
                return freeServiceResult;
            });

            var formattedResponse = FormatResponse(result, request);
            await StoreVerificationData(formattedResponse);

            return new ApiResponseResult(formattedResponse);
        }

        /// <summary>
        /// Stores verification data into the DB
        /// </summary>
        private async Task StoreVerificationData(BackendServiceResponse response)
        {
            // in MappingProfile.cs we have defined mapping between those two,
            // with all requirements
            var verification = _mapper.Map<VerificationResponseDTO>(response);

            await _dataService.StoreVerificationDataAsync(_mapper.Map<VerificationData>(verification));
        }

        /// <summary>
        /// Converts API responses into BackendServiceResponse
        /// </summary>
        private BackendServiceResponse FormatResponse(ApiResponseResult result, BackendServiceRequest request)
        {
            var response = new BackendServiceResponse
            {
                Query = request.Query,
                VerificationId = request.VerificationId
            };

            if (result.Result is IApiDataResponse dataResponse)
            {
                var results = dataResponse.GetResults();

                if (results.Any())
                {
                    response.Result = results.First();
                    if (results.Count() > 1)
                    {
                        response.OtherResults = results.Skip(1).ToList();
                    }
                }
                else
                {
                    // bad approch, because we bring inconsistency in the response format.
                    // maybe I didn't understand requiremnts correctly,
                    // but it should be like: response.Error = new { Message = "..." }
                    response.Result = new { Message = "No active companies found." };
                }

                // determine source here
                if (result.Result is FreeThirdPartyResponse)
                {
                    response.Source = "FREE";
                }
                else if (result.Result is PremiumThirdPartyResponse)
                {
                    response.Source = "PREMIUM";
                }

                return response;

            } 
            else
            {
                // for any response that is not data, we will consider it as service error...
                response.Result = new { Message = "Third-party services are unavailable." };
                return response;
            }
        }
    }
}
