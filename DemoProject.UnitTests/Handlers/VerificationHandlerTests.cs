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
    public class VerificationHandlerTests
    {
        private readonly Mock<IDataService> _dataServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly VerificationHandler _handler;

        public VerificationHandlerTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _mapperMock = new Mock<IMapper>();
            _handler = new VerificationHandler(_dataServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedVerificationData_WhenDataExists()
        {
            var guid = Guid.NewGuid();
            var query = "test";

            var request = new VerificationRequest(guid);
            var verificationEntity = new VerificationData { VerificationId = guid, QueryText = query };
            var mappedDto = new VerificationResponseDTO { VerificationId = guid, QueryText = query };

            _dataServiceMock.Setup(d => d.GetVerificationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(verificationEntity);

            _mapperMock.Setup(m => m.Map<VerificationResponseDTO>(It.IsAny<VerificationData>()))
                .Returns(mappedDto);

            var result = await _handler.Handle(request, CancellationToken.None);
            var response = result.Result as VerificationResponseDTO;

            Assert.NotNull(result);
            Assert.NotNull(response);
            Assert.IsType<VerificationResponseDTO>(result.Result);
            Assert.Equal(guid, response.VerificationId);
            Assert.Equal(query, response.QueryText);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFoundError_WhenDataDoesNotExist()
        {
            var guid = Guid.NewGuid();
            var query = "test";

            var request = new VerificationRequest(guid);

            _dataServiceMock.Setup(d => d.GetVerificationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((VerificationData)null); // simulating no data...

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<NotFoundErrorResponse>(result.Result);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyResponse_WhenExceptionOccurs()
        {
            var guid = Guid.NewGuid();
            var query = "test";

            var request = new VerificationRequest(guid);

            _dataServiceMock.Setup(d => d.GetVerificationByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception(""));

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Result);
        }
    }
}
