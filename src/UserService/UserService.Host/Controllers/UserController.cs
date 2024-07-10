using Application.Contracts.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Diagnostics;
using UserService.Application.Contracts.Requests;
using UserService.Application.Contracts.Responses;
using UserService.Client;

namespace UserService.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserContoller : ControllerBase
    {
        private readonly ILogger<UserContoller> _logger;
        private readonly UserService.Client.IClient _client;
        private readonly IDatabase _redis; //redis can also be implemented at the Client level, whereby instead of querying the database, the local cache can be queried, and the value returned

        public UserContoller(ILogger<UserContoller> logger, UserService.Client.IClient client, IConnectionMultiplexer muxer)
        {
            _logger = logger;
            _client = client;
            _redis = muxer.GetDatabase();
        }

        /// <summary>
        /// Method to retrieve all users 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<APIResponse<List<UserDto>>> Get()
        {
            return await _client.GetUsers();
        }

        /// <summary>
        /// Method to create a user
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        public async Task<APIResponse<IDto>> Create(CreateUserDto createUserDto)
        {
            return await _client.CreateUser(createUserDto);
        }

        /// <summary>
        /// Method that increments a given user's points
        /// </summary>
        /// <param name="earnPointsDto"></param>
        /// <returns></returns>
        [HttpPost("Earn")]
        public async Task<APIResponse<IDto>> Earn(EarnPointsDto earnPointsDto)
        {
            //clear cached points for this user once they earn points
            var keyName = $"points:{earnPointsDto.UserId}";
            await _redis.KeyDeleteAsync(keyName);

            return await _client.EarnPoints(earnPointsDto);
        }

        /// <summary>
        /// Method that fetches a given user's points
        /// </summary>
        /// <param name="Id"></param>
        [HttpGet("GetUserPoints")]
        public async Task<APIResponse<int>> GetUserPoints(Guid Id)
        {
            int points = 0;
            var keyName = $"points:{Id}";
            //check if this user's points are cached
            var cachedPoints = await _redis.StringGetAsync(keyName);

            if (string.IsNullOrEmpty(cachedPoints)) //not cached
            {
                var apiResult = await _client.GetUserPoints(Id);

                if (apiResult.IsSuccess)
                {
                    points = apiResult.Result;
                }
                else
                {
                    return apiResult;
                }

                //cache the newly fetched points to last a day
                var setTask = _redis.StringSetAsync(keyName, points);
                var expireTask = _redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));
                await Task.WhenAll(setTask, expireTask);
            }
            else
            {
                points = Convert.ToInt32(cachedPoints);
            }

            return new APIResponse<int>()
            {
                Result = points,
                IsSuccess = true
            };
        }
    }
}
