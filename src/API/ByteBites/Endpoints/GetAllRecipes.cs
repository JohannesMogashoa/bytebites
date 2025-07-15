using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;

namespace ByteBites.Endpoints;

public class GetAllRecipes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/recipes", async (IRecipeRepository recipeRepository) =>
            {
                var recipes = await recipeRepository.GetAllRecipes();
                return Results.Ok(recipes.ToListItemDtos());
            })
            .WithTags("Recipes");
    }
}