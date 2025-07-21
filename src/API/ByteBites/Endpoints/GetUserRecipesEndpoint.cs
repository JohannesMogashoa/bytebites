using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain;

namespace ByteBites.Endpoints;

public class GetUserRecipesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/recipes/user/{userId}", async (string userId, IRecipeRepository recipeRepository) =>
            {
                var recipes = await recipeRepository.GetUserRecipes(userId);
                return Results.Ok(recipes.ToDtos());
            })
            .WithName("GetUserRecipes")
            .WithSummary("Retrieves all recipes created by a specific user.")
            .Produces<IEnumerable<RecipeDto>>(StatusCodes.Status200OK);
    }
}