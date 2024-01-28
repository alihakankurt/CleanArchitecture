using System.Security.Cryptography;
using System.Text;
using Core.Application.Services;

namespace Core.Infrastructure.Services;

public sealed class HashService : IHashService
{
    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        passwordSalt = hmac.Key;
    }

    public bool ValidatePasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
}
