using System.ComponentModel.DataAnnotations;

namespace ByteBites.Application.DTOs;

public record UpdateRecipeDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(250, ErrorMessage = "Description cannot exceed 200 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingredients are required.")]
    public string Ingredients { get; set; } = string.Empty;

    [Required(ErrorMessage = "Steps are required.")]
    public string Steps { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Cooking time must be between 1 minute and 24 hours.")]
    public int CookingTime { get; set; }

    public string? DietaryTags { get; set; }
}

public static class UpdateRecipeDtoExtensions
{
    public static Domain.Recipe ToDomainModel(this UpdateRecipeDto updateRecipeDto)
    {
        return new Domain.Recipe
        {
            Id = updateRecipeDto.Id,
            Title = updateRecipeDto.Title,
            Description = updateRecipeDto.Description,
            Ingredients = updateRecipeDto.Ingredients,
            Steps = updateRecipeDto.Steps,
            CookingTime = updateRecipeDto.CookingTime,
            DietaryTags = updateRecipeDto.DietaryTags
        };
    }
}