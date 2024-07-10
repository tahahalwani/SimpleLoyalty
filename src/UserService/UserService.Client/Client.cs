using Application.Contracts.Shared;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
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
    public class Client : IClient
    {
        private readonly UserDbContext _userDbContext;

        public Client(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<APIResponse<IDto>> CreateUser(CreateUserDto createUserDto)
        {
            //ideally, we would have an automapper that would map between dtos and entities
            User user = new User()
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                CreatedDate = DateTime.UtcNow
            };

            UserValidator validator = new UserValidator();
            ValidationResult result = validator.Validate(user);

            if (result.IsValid)
            {
                await _userDbContext.Users.AddAsync(user);
                await _userDbContext.SaveChangesAsync();

                return new APIResponse<IDto>()
                {
                    IsSuccess = true
                };
            }
            else
            {
                return new APIResponse<IDto>()
                {
                    IsSuccess = false,
                    Message = result.ToString()
                };
            }
        }

        public async Task<APIResponse<List<UserDto>>> GetUsers()
        {
            //ideally, we would have an automapper that would map between dtos and entities

            var users = await _userDbContext.Users
                .Include(u => u.Earnings).ToListAsync();

            var result = users.Select(user => new UserDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TotalEarnings = user.Earnings.Select(earning => earning.EarnedPoints).Sum()
            }).ToList();

            return new APIResponse<List<UserDto>>()
            {
                IsSuccess = true,
                Result = result
            };
        }

        public async Task<APIResponse<IDto>> EarnPoints(EarnPointsDto earnPointsDto)
        {
            //ideally, we would have an automapper that would map between dtos and entities
            Earning earning = new Earning()
            {
                UserId = earnPointsDto.UserId,
                EarnedPoints = earnPointsDto.PointsEarned,
                CreatedDate = DateTime.UtcNow
            };

            EarningValidator validator = new EarningValidator();
            ValidationResult result = validator.Validate(earning);

            if (result.IsValid)
            {
                await _userDbContext.Earnings.AddAsync(earning);
                await _userDbContext.SaveChangesAsync();

                return new APIResponse<IDto>()
                {
                    IsSuccess = true
                };
            }
            else
            {
                return new APIResponse<IDto>()
                {
                    IsSuccess = false,
                    Message = result.ToString()
                };
            }
        }

        public async Task<APIResponse<int>> GetUserPoints(Guid Id)
        {
            //ideally, we would have an automapper that would map between dtos and entities

            var user = await _userDbContext.Users
                .Include(u => u.Earnings)
                .Where(u => u.Id == Id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new APIResponse<int>()
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Result = 0
                };
            }

            var points = user.Earnings.Select(earning => earning.EarnedPoints).Sum();

            return new APIResponse<int>()
            {
                IsSuccess = true,
                Result = points
            };
        }
    }
}
