using DWDW_API.Core.Entities;
using DWDW_API.Providers;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DWDW_API.Providers
{
    public class JwtTokenProvider
    {
        private ExtensionSettings extensionSettings;

        public JwtTokenProvider(ExtensionSettings extensionSettings)
        {
            this.extensionSettings = extensionSettings;
        }
        public string CreateUserAccessToken(User user)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(extensionSettings.AppSettings.SecretKey);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name ,user.UserName),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim(ClaimTypes.System, "asdasd")
                };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(extensionSettings.AppSettings.TokenExpireTime),
                Issuer = extensionSettings.AppSettings.Issuer,
                Audience = extensionSettings.AppSettings.Audience,
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public string CreateDeviceAccessToken(Device device)
        //{

        //}
    }
}
