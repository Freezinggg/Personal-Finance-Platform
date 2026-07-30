using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Transaction;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;

namespace PersonalFinancePlatform.API.API.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("record")]
        public async Task<IActionResult> Record([FromBody] RecordTransactionRequest request)
        {
            var result = await _mediator.Send(
                new RecordTransactionCommand(request.WalletId, request.Amount, request.Description, request.TransactionType, request.TransactionAt)
                );
            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<RecordTransactionResult>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse<RecordTransactionResult>.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse<RecordTransactionResult>.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse<RecordTransactionResult>.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse<RecordTransactionResult>.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse<RecordTransactionResult>.Fail(result.ErrorMessage)),

                _ => StatusCode(500, ApiResponse<RecordTransactionResult>.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }
    }
}
