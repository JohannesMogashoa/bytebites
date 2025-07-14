using ByteBites.API.Application.Common.Interfaces;
using ByteBites.API.Application.DTOs;

namespace ByteBites.API.Endpoints;

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