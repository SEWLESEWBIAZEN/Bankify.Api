﻿using Bankify.Application.Common.DTOs.Auth.Request;
using Bankify.Application.Common.DTOs.Auth.Response;
using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bankify.Application.Features.Commands.Auth
{
    public class UserLogin:IRequest<OperationalResult<LoginResponse>>
    {
        public LoginRequest LoginRequest { get; set; }
    }

    internal class UserLoginCommandHandler : IRequestHandler<UserLogin, OperationalResult<LoginResponse>> 
    {
        private readonly IRepositoryBase<BUser> _users;
        private readonly INetworkService _networkService;
        private readonly IConfiguration _configuration;

        public UserLoginCommandHandler(IRepositoryBase<BUser> users, INetworkService networkService, IConfiguration configuration)
        {
            _users = users;
            _networkService = networkService;
            _configuration = configuration;
        }

        public async Task<OperationalResult<LoginResponse>> Handle(UserLogin command, CancellationToken cancellationToken)
        {
            var result=new OperationalResult<LoginResponse>();
            var request=command.LoginRequest;
            try 
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }

                var user = await _users.FirstOrDefaultAsync(u => u.Email == request.Email && u.RecordStatus != RecordStatus.Deleted);
                if(user is null)
                {
                    result.AddError(ErrorCode.NotFound, "User doesn't exist");
                    return result;
                }
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)){
                    result.AddError(ErrorCode.ValidationError, "Invalid Password");
                    return result;
                }
                var accessToken = GenerateToken(user);

                var loginResult = new LoginResponse
                {
                    Success = true,
                    Token = accessToken
                };
                result.Payload= loginResult;
                result.Message = "Logged In Successfully";

            }
            catch (Exception ex) 
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }

            return result;
        }

        public string GenerateToken(BUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName +" " +user.LastName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "Admin")


            };
            var token = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddSeconds(20),
                signingCredentials: credintials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
