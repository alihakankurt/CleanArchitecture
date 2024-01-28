namespace Core.Application.Models;

public sealed record TokenModel(string Token, DateTime CreatedAt, DateTime ExpiresAt)
{
}
