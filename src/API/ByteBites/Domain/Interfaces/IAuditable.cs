namespace ByteBites.Domain.Interfaces;

public interface IAuditable
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string UserId { get; set; } // Optional: if you want to track the user who created/updated the entity
}