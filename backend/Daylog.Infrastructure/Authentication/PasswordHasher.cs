using Daylog.Application.Abstractions.Authentication;
using System.Security.Cryptography;

namespace Daylog.Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int _saltSize = 16; // 128 bit
    private const int _hashSize = 32;  // 256 bit
    private const int _iterations = 100_000; // Number of iterations for PBKDF2

    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _hashSize);
        var hashBytes = new byte[_saltSize + _hashSize];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, _saltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, _saltSize, _hashSize);
        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string password, string hashedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);
        var salt = new byte[_saltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, _saltSize);
        var storedHash = new byte[_hashSize];
        Buffer.BlockCopy(hashBytes, _saltSize, storedHash, 0, _hashSize);
        var derivedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithm, _hashSize);
        return CryptographicOperations.FixedTimeEquals(derivedHash, storedHash);
    }
}
