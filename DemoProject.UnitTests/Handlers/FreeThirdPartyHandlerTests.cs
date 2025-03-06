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
    public class FreeThirdPartyHandlerTests
    {
        private readonly Mock<IDataService> _dataServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFailureSimulationService> _failureSimulationServiceMock;
        private readonly FreeThirdPartyHandler _handler;

        public FreeThirdPartyHandlerTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _mapperMock = new Mock<IMapper>();
            _failureSimulationServiceMock = new Mock<IFailureSimulationService>();

            _handler = new FreeThirdPartyHandler(_dataServiceMock.Object, _mapperMock.Object, _failureSimulationServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnServiceUnavailable_WhenFailureSimulationTriggers()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new FreeThirdPartyRequest(queryInfo);
            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(true);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<ServiceUnavailableErrorResponse>(result.Result);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedCompanies_WhenNoFailureOccurs()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new FreeThirdPartyRequest(queryInfo);

            var companies = new List<FreeServiceCompany>
            {
                new FreeServiceCompany { CIN = "company-1", Name = "Company A" },
                new FreeServiceCompany { CIN = "company-2", Name = "Company B" },
            };

            var mappedDtos = new List<FreeThirdPartyResponseDTO>
            {
                new FreeThirdPartyResponseDTO { CIN = "company-1", Name = "Company A" },
                new FreeThirdPartyResponseDTO { CIN = "company-2", Name = "Company B" }
            };

            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(false);
            _dataServiceMock.Setup(d => d.GetFreeCompaniesByIdAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(companies);

            _mapperMock.Setup(m => m.Map<FreeThirdPartyResponseDTO>(It.IsAny<FreeServiceCompany>()))
                .Returns((FreeServiceCompany c) => mappedDtos.FirstOrDefault(d => d.CIN == c.CIN));

            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as FreeThirdPartyResponse;

            Assert.NotNull(result);
            Assert.IsType<FreeThirdPartyResponse>(result.Result);
            Assert.Equal(companies.Count, response.Total);
            Assert.Equal(mappedDtos.Count, response.Results.Count());
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResponse_WhenExceptionOccurs()
        {
            var queryInfo = new QueryInfo("test", true);
            var request = new FreeThirdPartyRequest(queryInfo);
            _failureSimulationServiceMock.Setup(s => s.ShouldFail(It.IsAny<int>())).Returns(false);

            _dataServiceMock.Setup(d => d.GetFreeCompaniesByIdAsync(It.IsAny<string>(), It.IsAny<bool>()))
                .ThrowsAsync(new Exception("Database error"));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Result);
        }
    }
}
