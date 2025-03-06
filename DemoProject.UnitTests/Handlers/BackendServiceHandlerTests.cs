using AutoMapper;
using DemoProject.Application.DTOs;
using DemoProject.Application.Handlers;
using DemoProject.Application.Mediator;
using DemoProject.Application.Requests;
using DemoProject.Domain.Entities;
using DemoProject.Infrastructure.Services;
using MediatR;
using Moq;
using Xunit;

namespace DemoProject.Tests.Handlers
{
    public class BackendServiceHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IDataService> _dataServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BackendServiceHandler _handler;

        public BackendServiceHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _dataServiceMock = new Mock<IDataService>();
            _mapperMock = new Mock<IMapper>();

            _handler = new BackendServiceHandler(_mediatorMock.Object, _dataServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFreeServiceResponse_WhenFreeThirdPartyRequestSucceeds()
        {
            var guid = Guid.NewGuid();
            var request = new BackendServiceRequest(guid, "test-query");
            var queryInfo = new QueryInfo("test-query", true);

            var freeServiceResponse = new FreeThirdPartyResponse
            {
                Results = new List<FreeThirdPartyResponseDTO>
                {
                    new FreeThirdPartyResponseDTO { CIN = "company-1", Name = "Company A" }
                },
                Total = 1
            };

            var apiResponse = new ApiResponseResult(freeServiceResponse);

            _mediatorMock.Setup(m => m.Send(It.IsAny<FreeThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as BackendServiceResponse;

            Assert.NotNull(response);
            Assert.Equal("FREE", response.Source);
            Assert.Equal(guid, response.VerificationId);
            Assert.Equal("company-1", ((FreeThirdPartyResponseDTO)response.Result).CIN);

            // be sure that premium service was not called...
            _mediatorMock.Verify(m => m.Send(It.IsAny<PremiumThirdPartyRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldFallbackToPremiumService_WhenFreeThirdPartyRequestFails()
        {
            var guid = Guid.NewGuid();

            var request = new BackendServiceRequest(guid, "test-query");
            var queryInfo = new QueryInfo("test-query", true);

            var premiumServiceResponse = new PremiumThirdPartyResponse
            {
                Results = new List<PremiumThirdPartyResponseDTO>
                {
                    new PremiumThirdPartyResponseDTO { CompanyIdentificationNumber = "premium-1", CompanyName = "Premium Company A" }
                },
                Total = 1
            };

            var fallbackResponse = new ApiResponseResult(premiumServiceResponse);

            _mediatorMock.Setup(m => m.Send(It.IsAny<FreeThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponseResult()); // simulating failure...

            _mediatorMock.Setup(m => m.Send(It.IsAny<PremiumThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fallbackResponse);

            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as BackendServiceResponse;

            Assert.NotNull(response);
            Assert.Equal("PREMIUM", response.Source);
            Assert.Equal(guid, response.VerificationId);
            Assert.Equal("premium-1", ((PremiumThirdPartyResponseDTO)response.Result).CompanyIdentificationNumber);

            // be sure that premium service was called...
            _mediatorMock.Verify(m => m.Send(It.IsAny<PremiumThirdPartyRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnErrorMessage_WhenBothServicesFail()
        {
            var guid = Guid.NewGuid();

            var request = new BackendServiceRequest(guid, "test-query");

            _mediatorMock.Setup(m => m.Send(It.IsAny<FreeThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponseResult(null)); // free service fails

            _mediatorMock.Setup(m => m.Send(It.IsAny<PremiumThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponseResult(null)); // premium service fails

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as BackendServiceResponse;
            string message = response.Result.ToString();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(response.VerificationId, guid);
            Assert.True(message.Contains("Third-party services are unavailable."));
        }

        [Fact]
        public async Task Handle_ShouldStoreVerificationData_WhenResponseIsValid()
        {
            var guid = Guid.NewGuid();

            var request = new BackendServiceRequest(guid, "test-query");

            var freeServiceResponse = new FreeThirdPartyResponse
            {
                Results = new List<FreeThirdPartyResponseDTO>
                {
                    new FreeThirdPartyResponseDTO { CIN = "company-1", Name = "Company A" }
                },
                Total = 1
            };

            var apiResponse = new ApiResponseResult(freeServiceResponse);
            var backendResponse = new BackendServiceResponse { Query = "test-query", VerificationId = guid };
            var verificationDto = new VerificationResponseDTO();
            var verificationData = new VerificationData();

            _mediatorMock.Setup(m => m.Send(It.IsAny<FreeThirdPartyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            _mapperMock.Setup(m => m.Map<VerificationResponseDTO>(backendResponse)).Returns(verificationDto);
            _mapperMock.Setup(m => m.Map<VerificationData>(verificationDto)).Returns(verificationData);

            var result = await _handler.Handle(request, CancellationToken.None);

            // be sure that we will store verification data...
            _dataServiceMock.Verify(d => d.StoreVerificationDataAsync(It.IsAny<VerificationData>()), Times.Once);
        }
    }
}
