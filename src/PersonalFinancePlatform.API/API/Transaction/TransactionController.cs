using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Transaction;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Handler.Transaction.DeleteTransaction;
using PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using PersonalFinancePlatform.Application.Handler.Transaction.UpdateTransaction;
using PersonalFinancePlatform.Application.Interfaces.Authentication;
using System.Security.Claims;

namespace PersonalFinancePlatform.API.API.Transaction
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpPost]
        public async Task<IActionResult> Record([FromBody] RecordTransactionRequest request)
        {
            var userId = _currentUser.UserId;

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


        [HttpPut("{transactionId}")]
        public async Task<IActionResult> Update(Guid transactionId, [FromBody] UpdateTransactionRequest request)
        {
            var userId = _currentUser.UserId;

            var result = await _mediator.Send(new UpdateTransactionCommand(userId, transactionId, request.Amount, request.Description, request.TransactionType, request.TransactionAt));
            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<UpdateTransactionResult>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse.Fail(result.ErrorMessage)),

                _ => StatusCode(500, ApiResponse.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> Delete(Guid transactionId)
        {
            var userId = _currentUser.UserId;

            var result = await _mediator.Send(new DeleteTransactionCommand(userId, transactionId));
            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<bool>.Ok(result.Data)),
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
            var userId = _currentUser.UserId;

            var result = await _mediator.Send(
                new GetTransactionHistoryQuery(
                    userId,
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
