namespace ByteBites.API.Application.DTOs;

public record CreateRecipeDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string Steps { get; set; } = string.Empty;
    public decimal CookingTime { get; set; }
    public string DietaryTags { get; set; } = string.Empty;
}

public static class CreateRecipeDTOExtensions
{
    public static Domain.Recipe ToDomainModel(this CreateRecipeDTO createRecipeDto)
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