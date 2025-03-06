using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Handlers;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Domain.Entities;
using DemoProject.Infrastructure.Services;
using Moq;
using Xunit;

namespace DemoProject.Tests.Handlers
{
    public class PremiumThirdPartyHandlerTests
    {
        private readonly Mock<IDataService> _dataServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFailureSimulationService> _failureSimulationServiceMock;
        private readonly PremiumThirdPartyHandler _handler;

        public PremiumThirdPartyHandlerTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _mapperMock = new Mock<IMapper>();
            _failureSimulationServiceMock = new Mock<IFailureSimulationService>();

            _handler = new PremiumThirdPartyHandler(_dataServiceMock.Object, _mapperMock.Object, _failureSimulationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnServiceUnavailable_WhenFailureSimulationTriggers()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new PremiumThirdPartyRequest(queryInfo);
            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(true);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<ServiceUnavailableErrorResponse>(result.Result);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedCompanies_WhenNoFailureOccurs()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new PremiumThirdPartyRequest(queryInfo);

            var companies = new List<PremiumServiceCompany>
            {
                new PremiumServiceCompany { CompanyIdentificationNumber = "company-1", CompanyName = "Company A" },
                new PremiumServiceCompany { CompanyIdentificationNumber = "company-2", CompanyName = "Company B" },
            };

            var mappedDtos = new List<PremiumThirdPartyResponseDTO>
            {
                new PremiumThirdPartyResponseDTO { CompanyIdentificationNumber = "company-1", CompanyName = "Company A" },
                new PremiumThirdPartyResponseDTO { CompanyIdentificationNumber = "company-2", CompanyName = "Company B" }
            };

            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(false);
            _dataServiceMock.Setup(d => d.GetPremiumCompaniesByIdAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(companies);

            _mapperMock.Setup(m => m.Map<PremiumThirdPartyResponseDTO>(It.IsAny<PremiumServiceCompany>()))
                .Returns((PremiumServiceCompany c) => mappedDtos.FirstOrDefault(d => d.CompanyIdentificationNumber == c.CompanyIdentificationNumber));

            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as PremiumThirdPartyResponse;

            Assert.NotNull(result);
            Assert.IsType<PremiumThirdPartyResponse>(result.Result);
            Assert.Equal(companies.Count, response.Total);
            Assert.Equal(mappedDtos.Count, response.Results.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResponse_WhenExceptionOccurs()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new PremiumThirdPartyRequest(queryInfo);
            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(false);

            _dataServiceMock.Setup(d => d.GetPremiumCompaniesByIdAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Result);
        }
    }
}
