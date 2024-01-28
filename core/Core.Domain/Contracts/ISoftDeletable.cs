namespace Core.Domain.Contracts;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}
