using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Auth
{
    public class UserDto
    {

        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public UserDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public UserDto()
        {
        }
    }
}
