using ByteBites.API.Domain.Interfaces;

namespace ByteBites.API.Domain;

public class Recipe : IAuditable, ISoftDelete
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Ingredients { get; set; }
    public string Steps { get; set; }
    public decimal CookingTime { get; set; }
    public string DietaryTags { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}