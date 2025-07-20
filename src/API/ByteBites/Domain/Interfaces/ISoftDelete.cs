namespace ByteBites.Domain.Interfaces;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    
    DateTimeOffset? DeleteAt { get; set; }
}