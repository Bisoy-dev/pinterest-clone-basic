using System;
namespace PinterestClone.Infrastructure.Utils.Jwt;

public interface IJwtService
{
    string Generate(string userId, string email);
}