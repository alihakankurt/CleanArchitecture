namespace Core.Domain.Contracts;

public interface IUpdateAuditable
{
    public DateTime UpdatedAt { get; set; }
}
