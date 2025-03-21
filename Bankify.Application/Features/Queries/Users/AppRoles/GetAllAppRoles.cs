﻿using Bankify.Application.Common.Helpers;
using Bankify.Application.Repository;
using Bankify.Application.Services;
using Bankify.Domain.Models.Shared;
using Bankify.Domain.Models.Users;
using MediatR;

namespace Bankify.Application.Features.Queries.Users.AppRoles
{
    public class GetAllAppRoles : IRequest<OperationalResult<List<AppRole>>>
    {
        public bool ForDropdown { get; set; }
    }

    internal class GetAllAppRolesQueryHandler : IRequestHandler<GetAllAppRoles, OperationalResult<List<AppRole>>>
    {
        private readonly IRepositoryBase<AppRole> _AppRoles;
        private readonly INetworkService _networkService;

        public GetAllAppRolesQueryHandler(IRepositoryBase<AppRole> AppRoles, INetworkService networkService)
        {
            _AppRoles = AppRoles;
            _networkService = networkService;
        }

        public async Task<OperationalResult<List<AppRole>>> Handle(GetAllAppRoles query, CancellationToken cancellationToken)
        {
            var result = new OperationalResult<List<AppRole>>();
            try
            {
                var dbReachable = await _networkService.IsConnected();
                if (!dbReachable)
                {
                    result.AddError(ErrorCode.NetworkError, "Network Error (Unable to reach database)");
                    return result;
                }

                var AppRoles = new List<AppRole>();
                if (!query.ForDropdown) 
                {
                    AppRoles = await _AppRoles.WhereAsync(rc => rc.RecordStatus != RecordStatus.Deleted, "RoleClaims.AppClaim");
                }
                else
                {
                    AppRoles = await _AppRoles.WhereAsync(rc => rc.Id != 0 && rc.RecordStatus != RecordStatus.Deleted);

                }
                  
                if (AppRoles.Count == 0)
                {
                    result.AddError(ErrorCode.NotFound, "No Role Found");
                    return result;
                }
                result.Payload = AppRoles;

            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ServerError, ex.Message);
            }
            return result;
        }
    }
}
