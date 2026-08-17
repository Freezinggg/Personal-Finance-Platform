namespace PersonalFinancePlatform.API.Contracts.Category
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; }
        public CreateCategoryRequest(string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
