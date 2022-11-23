namespace PinterestClone.Infrastructure.Utils.Encryptors;

public interface IEncryptor
{
    HashAndSalt OnEncrypt(string password);
    bool Verify(string password, byte[]hash, byte[]salt);
}

public record HashAndSalt(byte[]Hash, byte[]Salt);