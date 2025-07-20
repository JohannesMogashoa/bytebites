using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace ByteBites.Application.DTOs;

public record CreateRecipeDto
{
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

public static class CreateRecipeDtoExtensions
{
    public static Domain.Recipe ToDomainModel(this CreateRecipeDto createRecipeDto)
    {
        return new Domain.Recipe
        {
            Title = createRecipeDto.Title,
            Description = createRecipeDto.Description,
            Ingredients = createRecipeDto.Ingredients,
            Steps = createRecipeDto.Steps,
            CookingTime = createRecipeDto.CookingTime,
            DietaryTags = createRecipeDto.DietaryTags,
        };
    }
}

public class CreateRecipeDtoValidator : AbstractValidator<CreateRecipeDto>
{
    public CreateRecipeDtoValidator()
    {
        RuleFor(r => r.Title).NotNull().NotEmpty();
        RuleFor(r => r.Description).NotNull().NotEmpty();
        RuleFor(r => r.CookingTime).GreaterThan(0);
    }
}