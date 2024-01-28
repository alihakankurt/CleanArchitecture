namespace Core.Domain.Contracts;

public interface ICreationAuditable
{
    public DateTime CreatedAt { get; set; }
}
