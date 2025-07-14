namespace ByteBites.API.Application.DTOs;

public record RecipeDTO
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
}

public static class RecipeDTOExtensions
{
    public static RecipeDTO ToDto(this Domain.Recipe recipe)
    {
        return new RecipeDTO
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            Ingredients = recipe.Ingredients,
            Steps = recipe.Steps,
            CookingTime = recipe.CookingTime,
            DietaryTags = recipe.DietaryTags,
            CreatedAt = recipe.CreatedAt,
            UpdatedAt = recipe.UpdatedAt,
            CreatedBy = recipe.CreatedBy,
            UpdatedBy = recipe.UpdatedBy
        };
    }
}