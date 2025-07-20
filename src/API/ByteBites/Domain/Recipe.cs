using System.ComponentModel.DataAnnotations;
using ByteBites.Domain.Interfaces;

namespace ByteBites.Domain;

public class Recipe : IAuditable, ISoftDelete
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Ingredients { get; set; } = string.Empty;
    [Required]
    public string Steps { get; set; }  = string.Empty;
    [Range(1, 1440, ErrorMessage = "Cooking time must be between 1 minute and 24 hours.")]
    public int CookingTime { get; set; }
    public string? DietaryTags { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    
    // ISoftDelete property
    public bool IsDeleted { get; set; } = false;
    public DateTimeOffset? DeleteAt { get; set; }
}