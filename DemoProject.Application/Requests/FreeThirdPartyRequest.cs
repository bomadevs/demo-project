using DemoProject.Application.Mediator;
using DemoProject.Domain.Entities;
using MediatR;

namespace DemoProject.Application.Requests
{
    /// <summary>
    /// Request to get data from a free third-party service, will be handled by FreeThirdPartyHandler, by
    /// using mediator.
    /// </summary>
    public class FreeThirdPartyRequest : IRequest<ApiResponseResult>
    {
        public QueryInfo QueryInfo;

        public FreeThirdPartyRequest(QueryInfo queryInfo)
        {
            this.QueryInfo = queryInfo;
        }
    }
}
