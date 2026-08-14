using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Transaction;
using PersonalFinancePlatform.API.Contracts.Wallet;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using PersonalFinancePlatform.Application.Handler.Wallet.CreateWallet;
using PersonalFinancePlatform.Application.Handler.Wallet.DeleteWallet;
using PersonalFinancePlatform.Application.Handler.Wallet.UpdateWallet;
using PersonalFinancePlatform.Application.Interfaces.Authentication;
using System.Security.Claims;

namespace PersonalFinancePlatform.API.API.Wallet
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request)
        {
            var userId = _currentUser.UserId;

            var result = await _mediator.Send(new CreateWalletCommand(userId, request.WalletName));
            return result.Status switch
            {
                ResultStatus.Success => Ok(ApiResponse<Guid>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse.Fail(result.ErrorMessage)),

                _ => StatusCode(500, ApiResponse.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }


        [HttpPut("{walletId}")]
        public async Task<IActionResult> Update(Guid walletId, [FromBody] UpdateWalletRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var result = await _mediator.Send(new UpdateWalletCommand(userId, walletId, request.WalletName));
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

        [HttpDelete("{walletId}")]
        public async Task<IActionResult> Delete(Guid walletId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

            var result = await _mediator.Send(new DeleteWalletCommand(walletId, userId));
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
    }
}
