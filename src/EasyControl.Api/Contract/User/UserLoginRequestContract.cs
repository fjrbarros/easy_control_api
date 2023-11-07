namespace EasyControl.Api.Contract.User
{
    public class UserLoginRequestContract
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
