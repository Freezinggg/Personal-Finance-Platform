using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Transaction;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using System.Security.Claims;

namespace PersonalFinancePlatform.API.API.Transaction
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Record([FromBody] RecordTransactionRequest request)
        {
            var result = await _mediator.Send(new RecordTransactionCommand(request.WalletId, request.Amount, request.Description, request.TransactionType, request.TransactionAt));
            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<RecordTransactionResult>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse.Fail(result.ErrorMessage)),

                _ => StatusCode(500, ApiResponse.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionHistory([FromQuery] GetTransactionHistoryRequest request)
        {
            //Hardcode userId, will remove later if have jwt/authorization
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(
                new GetTransactionHistoryQuery(
                    Guid.Parse(userIdClaim),
                    request.WalletId,
                    request.TransactionType,
                    request.Page,
                    request.PageSize)
                );

            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<PagedResult<GetTransactionHistoryResult>>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse.Fail(result.ErrorMessage)),
                        
                _ => StatusCode(500, ApiResponse.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }
    }
}
