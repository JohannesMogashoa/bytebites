using ByteBites.API.Application.Common.Interfaces;
using ByteBites.API.Application.DTOs;

namespace ByteBites.API.Endpoints;

public class GetRecipeById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/recipes/{id:guid}", async (Guid id, IRecipeRepository recipeRepository) =>
            {
                var recipe = await recipeRepository.GetRecipeById(id);
                return recipe == null ? Results.NotFound() : Results.Ok(recipe.ToDto());
            })
            .WithTags("Recipes")
            .WithName("GetRecipeById");
    }
}