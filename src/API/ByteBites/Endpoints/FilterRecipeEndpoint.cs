using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;

namespace ByteBites.Endpoints;

public class FilterRecipeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/recipes/filter", async (FilterRecipeDto filter, IRecipeRepository recipeRepository) =>
            {
                var recipes = await recipeRepository.FilterRecipes(filter);

                return Results.Ok(recipes);
            })
            .WithTags("Recipes")
            .WithName("FilterRecipes");
    }
}