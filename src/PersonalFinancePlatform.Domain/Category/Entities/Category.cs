using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.Category.Entities
{
    public sealed class Category
    {
        public Guid Id { get; }
        public Guid OwnerId { get; }
        public string CategoryName { get; private set; }
        public DateTime CreatedAt { get; }
        public bool IsSystem { get; } = false;

        public Category(Guid ownerId, string categoryName, DateTime createdAt, bool isSystem = false)
        {
            if (ownerId == Guid.Empty)
                throw new InvariantViolationException("Category must have Owner.");

            Id = Guid.NewGuid();
            OwnerId = ownerId;
            CategoryName = ValidateAndNormalizeName(categoryName);
            CreatedAt = createdAt;
            IsSystem = isSystem;
        }

        private string ValidateAndNormalizeName(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new InvariantViolationException("Category Name cannot be empty.");

            var normalizedCategoryName = categoryName.Trim();
            if (normalizedCategoryName.Length < 2 || normalizedCategoryName.Length > 100)
                throw new InvariantViolationException("Category Name have to be 2-100 characters.");

            return normalizedCategoryName;
        }

        public void Rename(string newCategoryName){
            if (IsSystem)
                throw new InvariantViolationException("This category cannot be renamed.");

            CategoryName = ValidateAndNormalizeName(newCategoryName);
        } 
    }
}
