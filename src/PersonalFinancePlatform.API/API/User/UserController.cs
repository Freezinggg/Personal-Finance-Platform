using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.Application.Common;
using System.Security.Claims;

namespace PersonalFinancePlatform.API.API.User
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Me(), ChangeDisplayName()
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok();
        }
    }
}
