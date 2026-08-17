using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Category.Entities;
using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Category.CreateCategory
{
    public class CreateCategoryHandler(
        ICategoryRepository categoryRepository
        ) : IRequestHandler<CreateCategoryCommand, Result<Guid>>
    {
        private readonly ICategoryRepository _categoryRepo = categoryRepository;
        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = new Domain.Category.Entities.Category(request.UserId, request.CategoryName, DateTime.UtcNow);
                var existsCategory = await _categoryRepo.ExistsByNameAsync(request.UserId, category.CategoryName, cancellationToken);
                if(existsCategory)
                    return Result<Guid>.Fail("Category with same name already exist");

                await _categoryRepo.AddAsync(category, cancellationToken);
                return Result<Guid>.Success(category.Id);
            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT
                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<Guid>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<Guid>.Fail(ex.Message);
                    default:
                        return Result<Guid>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                return Result<Guid>.Error("An unexpected error occurred..");
            }
        }
    }
}
