namespace MinimalApi.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDto> _users;

        public UserRepository()
        {
            _users = new List<UserDto>
        {
            new UserDto("John", "123"),
            new UserDto("Monica", "123"),
            new UserDto("Nancy", "123")
        };
        }

        public UserDto GetUser(UserDto userModel) =>
            _users.FirstOrDefault(u =>
                string.Equals(u.UserName, userModel.UserName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u.Password, userModel.Password)) ??
                throw new Exception("User not found");
    }
}
