using Bankify.Application.Common.DTOs.ActionLogs.Response;
using Bankify.Application.Features.Queries.Actionlogs;
using Microsoft.AspNetCore.Mvc;

namespace Bankify.Api.Controllers.V1._0.Logs
{
    public class ActionLogsController : BaseController
    {
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllActionLogs();
            var result = await _mediator.Send(query);
            var logList=_mapper.Map<List<ActionLogDetail>>(result.Payload);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(logList);
        }
    }
}
