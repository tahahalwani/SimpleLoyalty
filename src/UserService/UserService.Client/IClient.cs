using Application.Contracts.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Contracts.Requests;
using UserService.Application.Contracts.Responses;
using UserService.Domain;
using UserService.Infrastructure;

namespace UserService.Client
{
    public interface IClient
    {
        Task<APIResponse<IDto>> CreateUser(CreateUserDto createUserDto);

        Task<APIResponse<List<UserDto>>> GetUsers();

        Task<APIResponse<IDto>> EarnPoints(EarnPointsDto earnPointsDto);

        Task<APIResponse<int>> GetUserPoints(Guid Id);
    }
}
