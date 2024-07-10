using Application.Contracts.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using UserService.Application.Contracts.Requests;
using UserService.Client;
using UserService.Host.Controllers;

namespace UserService.Client.Tests
{
    public class UnitTest1
    {
        private readonly Mock<IClient> _client;
        private readonly Mock<ILogger<UserContoller>> _logger;
        private readonly Mock<IConnectionMultiplexer> _muxer;
        public UnitTest1()
        {
            _client = new Mock<IClient>();
            _logger = new Mock<ILogger<UserContoller>>();
            _muxer = new Mock<IConnectionMultiplexer>();
        }

        [Fact]
        public async Task EarnTest()
        {
            var pointsData = GetPointsData();
            var pointsResponses = GetPointsResponses();

            var userContoller = new UserContoller(_logger.Object, _client.Object, _muxer.Object);

            _client.Setup(x => x.EarnPoints(pointsData[0]))
                .ReturnsAsync(pointsResponses[0]);
            var productResult = await userContoller.Earn(pointsData[0]);
            Assert.NotNull(productResult);
            Assert.False(productResult.IsSuccess);

            _client.Setup(x => x.EarnPoints(pointsData[1]))
                .ReturnsAsync(pointsResponses[1]);
            productResult = await userContoller.Earn(pointsData[1]);
            Assert.NotNull(productResult);
            Assert.False(productResult.IsSuccess);

            _client.Setup(x => x.EarnPoints(pointsData[2]))
                .ReturnsAsync(pointsResponses[2]);
            productResult = await userContoller.Earn(pointsData[2]);
            Assert.NotNull(productResult);
            Assert.True(productResult.IsSuccess);
        }

        private List<EarnPointsDto> GetPointsData()
        {
            List<EarnPointsDto> pointsData = new List<EarnPointsDto>
            {
                new EarnPointsDto
                {
                    PointsEarned = 0,
                    UserId = new Guid(),
                },
                 new EarnPointsDto
                {
                    PointsEarned = 101,
                    UserId = new Guid(),
                },
                 new EarnPointsDto
                {
                    PointsEarned = 30,
                    UserId = new Guid(),
                },
            };
            return pointsData;
        }

        private List<APIResponse<IDto>> GetPointsResponses()
        {
            List<APIResponse<IDto>> pointsResponses = new List<APIResponse<IDto>>
            {
                new APIResponse<IDto>
                {
                    IsSuccess = false,
                },
                new APIResponse<IDto>
                {
                    IsSuccess = false,
                },
                 new APIResponse<IDto>
                {
                    IsSuccess = true,
                },
            };
            return pointsResponses;
        }
    }
}
