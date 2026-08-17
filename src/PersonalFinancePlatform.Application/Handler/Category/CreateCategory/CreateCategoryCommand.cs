using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Category.CreateCategory
{
    public sealed class CreateCategoryCommand : IRequest<Result<Guid>>
    {
        public Guid UserId { get; }
        public string CategoryName { get; }

        public CreateCategoryCommand(Guid userId, string categoryName)
        {
            UserId = userId;
            CategoryName = categoryName;
        }
    }
}
