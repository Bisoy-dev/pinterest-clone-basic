using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace PinterestClone.Infrastructure.Utils.Jwt;


public class JwtService : IJwtService
{
    private readonly IConfiguration configuration;

    public JwtService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public string Generate(string userId, string email)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer : configuration["Jwt:Issuer"],
            expires : DateTime.UtcNow.AddMinutes(60),
            claims : claims,
            signingCredentials : signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}