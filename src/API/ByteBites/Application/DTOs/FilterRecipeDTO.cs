namespace ByteBites.Application.DTOs;

public class FilterRecipeDto
{
    public string? Title { get; set; }
    public string? DietaryTags { get; set; }
    public string? Ingredients { get; set; }
    public decimal? CookingTime { get; set; }
}