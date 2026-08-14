using Microsoft.AspNetCore.Http;
using PersonalFinancePlatform.Application.Interfaces.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Authentication
{
    public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public Guid UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                    throw new InvalidOperationException(
                        "Authenticated user ID is invalid.");

                return userId;
            }
        }
    }
}
