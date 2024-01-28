namespace Core.Application.Services;

public interface IHashService
{
    public void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public bool ValidatePasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}
