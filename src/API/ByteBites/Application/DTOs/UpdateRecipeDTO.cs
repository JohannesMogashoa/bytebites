namespace ByteBites.Application.DTOs;

public record UpdateRecipeDTO
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Ingredients { get; set; }
    public required string Steps { get; set; }
    public decimal CookingTime { get; set; }
    public required string DietaryTags { get; set; }
}

public static class UpdateRecipeDTOExtensions
{
    public static Domain.Recipe ToDomainModel(this UpdateRecipeDTO updateRecipeDto)
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