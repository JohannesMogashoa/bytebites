namespace ByteBites.Application.DTOs;

public record RecipeListItemDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public static class RecipeListItemDTOExtensions
{
    public static RecipeListItemDTO ToListItemDto(this Domain.Recipe recipe)
    {
        return new RecipeListItemDTO
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            CreatedBy = recipe.CreatedBy,
            CreatedAt = recipe.CreatedAt
        };
    }
    
    public static IEnumerable<RecipeListItemDTO> ToListItemDtos(this IEnumerable<Domain.Recipe> recipes)
    {
        return recipes.Select(r => r.ToListItemDto());
    }
}