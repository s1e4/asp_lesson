namespace MinimalApi.Auth
{
    public interface IUserRepository
    {
        UserDto GetUser(UserDto userModel);
    }
}
