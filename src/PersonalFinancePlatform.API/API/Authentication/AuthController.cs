using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Auth;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;

namespace PersonalFinancePlatform.API.API.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //Register, Login, Logout, etc.

        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request.DisplayName, request.Email, request.Password));
            return result.Status switch
            {
                ResultStatus.Success => CreatedAtAction($"/api/user/{result.Data!.UserId}", ApiResponse<RegisterUserResult>.Ok(result.Data)),
                ResultStatus.Invalid => BadRequest(ApiResponse<RegisterUserResult>.Fail(result.ErrorMessage)),
                ResultStatus.Fail => Conflict(ApiResponse<RegisterUserResult>.Fail(result.ErrorMessage)),
                ResultStatus.Error => StatusCode(500, ApiResponse<RegisterUserResult>.Fail(result.ErrorMessage)),
                ResultStatus.NotFound => NotFound(ApiResponse<RegisterUserResult>.Fail(result.ErrorMessage)),
                ResultStatus.ServiceUnavailable => StatusCode(503, ApiResponse<RegisterUserResult>.Fail(result.ErrorMessage)),

                _ => StatusCode(500, ApiResponse<RegisterUserResult>.Fail("Unhandled result status")) //default value if ResultStatus is its new or default
            };
        }
    }
}
