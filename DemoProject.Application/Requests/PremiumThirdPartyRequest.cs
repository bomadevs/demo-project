using DemoProject.Application.Mediator;
using DemoProject.Domain.Entities;
using MediatR;

namespace DemoProject.Application.Requests
{
    /// <summary>
    /// Request to get premium third-party data.
    /// </summary>
    public class PremiumThirdPartyRequest : IRequest<ApiResponseResult>
    {
        public QueryInfo QueryInfo { get; set; }

        public PremiumThirdPartyRequest(QueryInfo queryInfo)
        {
            this.QueryInfo = queryInfo;
        }
    }
}
