namespace EasyControl.Api.Contract.User
{
    public class UserLoginResponseContract : UserCreateResponseContract
    {
        public string Token { get; set; } = string.Empty;
    }
}
