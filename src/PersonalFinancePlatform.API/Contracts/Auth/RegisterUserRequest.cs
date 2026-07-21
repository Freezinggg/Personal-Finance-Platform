namespace PersonalFinancePlatform.API.Contracts.Auth
{
    public class RegisterUserRequest
    {
        public string DisplayName { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
