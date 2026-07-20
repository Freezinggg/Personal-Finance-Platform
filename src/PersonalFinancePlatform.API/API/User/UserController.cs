using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancePlatform.API.Contracts.User;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.User.RegisterUser;

namespace PersonalFinancePlatform.API.API.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Me(), ChangeDisplayName()
    }
}
