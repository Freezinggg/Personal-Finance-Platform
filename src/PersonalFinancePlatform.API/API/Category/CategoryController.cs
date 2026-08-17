using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.Category;
using PersonalFinancePlatform.API.Contracts.Transaction;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Category;
using PersonalFinancePlatform.Application.Handler.Category.CreateCategory;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using PersonalFinancePlatform.Application.Interfaces.Authentication;

namespace PersonalFinancePlatform.API.API.Category
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator, ICurrentUser currentUser) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUser _currentUser = currentUser;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var userId = _currentUser.UserId;

            var result = await _mediator.Send(new CreateCategoryCommand(userId, request.CategoryName));
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
    }
}
