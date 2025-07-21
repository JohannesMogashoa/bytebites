namespace ByteBites.Application.DTOs;

public record RecipeDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Steps { get; set; } = string.Empty;
    public int CookingTime { get; set; }
    public string? DietaryTags { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public static class RecipeDtoExtensions
{
    public static RecipeDto ToDto(this Domain.Recipe recipe)
    {
        return new RecipeDto
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
            UpdatedBy = recipe.UpdatedBy,
            UserId = recipe.UserId,
        };
    }
    
    public static IEnumerable<RecipeDto> ToDtos(this IEnumerable<Domain.Recipe> recipes)
    {
        return recipes.Select(recipe => recipe.ToDto());
    }
}