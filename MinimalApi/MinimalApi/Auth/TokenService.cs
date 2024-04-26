using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApi.Auth
{

    public class TokenService : ITokenService
    {
        // Время жизни токена - 30 минут.
        private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);

        // Метод для построения JWT (JSON Web Token).
        public string BuildToken(string key, string issuer, UserDto user)
        {
            // Создание утверждений (claims) для токена. Утверждения - это пары "имя-значение",
            // которые содержат информацию о пользователе и другие метаданные.
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName), // Имя пользователя.
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) // Уникальный идентификатор.
        };

            // Создание ключа безопасности на основе секретного ключа.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            // Создание подписи для токена, используя алгоритм HMAC и ключ безопасности.
            var credentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature);

            // Описание токена - с указанием издателя, получателя, утверждений, 
            // времени истечения и подписи.
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);

            // Генерация токена и его сериализация в строку.
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
    //public class TokenService : ITokenService
    //{
    //    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);
    //    public string BuildToken(string key, string issuer, UserDto user)
    //    {
    //        var claims = new[]
    //        {
    //        new Claim(ClaimTypes.Name, user.UserName),
    //        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
    //    };

    //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    //        var credentials = new SigningCredentials(securityKey,
    //            SecurityAlgorithms.HmacSha256Signature);
    //        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
    //            expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
    //        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    //    }
    //}
}
