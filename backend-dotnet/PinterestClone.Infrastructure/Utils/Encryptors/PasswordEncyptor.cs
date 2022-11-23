using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
namespace PinterestClone.Infrastructure.Utils.Encryptors;

public class PasswordEncryptor : IEncryptor
{
    public HashAndSalt OnEncrypt(string password)
    {
        using var hmac = new HMACSHA512();
        var hashed = new HashAndSalt(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), 
                hmac.Key);
        return hashed;
        
    }

    public bool Verify(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}