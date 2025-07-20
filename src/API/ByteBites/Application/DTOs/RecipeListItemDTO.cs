namespace ByteBites.Application.DTOs;

public record RecipeListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    
    public string? DietaryTags { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public static class RecipeListItemDtoExtensions
{
    public static RecipeListItemDto ToListItemDto(this Domain.Recipe recipe)
    {
        return new RecipeListItemDto
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            CreatedBy = recipe.CreatedBy,
            CreatedAt = recipe.CreatedAt,
            UserId = recipe.UserId,
            DietaryTags = recipe.DietaryTags,
        };
    }
    
    public static IEnumerable<RecipeListItemDto> ToListItemDtos(this IEnumerable<Domain.Recipe> recipes)
    {
        return recipes.Select(r => r.ToListItemDto());
    }
}