using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Application.Mediator
{
    /// <summary>
    /// Base class for all API controllers, that will share common behavior.
    /// </summary>
    [ApiController]
    [ProducesResponseType(typeof(NotFoundErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ServiceUnavailableErrorResponse), StatusCodes.Status503ServiceUnavailable)]
    public class ApiControllerBase : ControllerBase
    {
        // mediator pattern. decouple request handling logic...
        protected readonly ISender Mediator;

        public ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected async Task<ApiResponseResult> SendToHandler(IRequest<ApiResponseResult> request,
            CancellationToken cancellationToken = default)
        {
            return await this.Mediator.Send(request, cancellationToken);
        }

        protected IActionResult SendResponse(ApiResponseResult result) =>
            SendResponse<IApiResponse>(result, Ok);

        protected IActionResult SendResponse<T>(ApiResponseResult result, Func<T, ActionResult> onSuccess)
            where T : IApiResponse =>
            result.Result switch
            {
                NotFoundErrorResponse r => NotFound(r),
                ServiceUnavailableErrorResponse r => StatusCode(StatusCodes.Status503ServiceUnavailable, r),
                null => NoContent(),
                T r => onSuccess(r),
                _ => throw new NotImplementedException($"Unhandled response type: {typeof(T)}")
            };
    }
}
