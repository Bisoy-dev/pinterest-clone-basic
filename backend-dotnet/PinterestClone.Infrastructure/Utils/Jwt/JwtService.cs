using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PinterestClone.Infrastructure.Utils.Jwt;


public class JwtService : IJwtService
{
    public string Generate(string userId, string email)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pinteres-super-secret-key")),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer : "PinterestCloneBasic",
            expires : DateTime.UtcNow.AddMinutes(60),
            claims : claims,
            signingCredentials : signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}