using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Infrastructure.Services;
using MediatR;

namespace DemoProject.Application.Handlers
{
    public class VerificationHandler : IRequestHandler<VerificationRequest, ApiResponseResult>
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public VerificationHandler(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        public async Task<ApiResponseResult> Handle(VerificationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var verificationData = await _dataService.GetVerificationByIdAsync(request.VerificationId);
                if (verificationData != null)
                {
                    return new ApiResponseResult(_mapper.Map<VerificationResponseDTO>(verificationData));
                }
                return new ApiResponseResult(new NotFoundErrorResponse("Verification data not found."));
            }
            catch (Exception ex)
            {
                return new ApiResponseResult();
            }
        }
    }
}
