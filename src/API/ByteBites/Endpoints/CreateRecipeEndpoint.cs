using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;

namespace ByteBites.Endpoints;

public class CreateRecipeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/recipes", async (CreateRecipeDTO createRecipeDto, IRecipeRepository recipeRepository) =>
            {
                var newRecipe = createRecipeDto.ToDomainModel();
                await recipeRepository.AddRecipe(newRecipe);
                return Results.Created($"/api/recipes/{newRecipe.Id}", newRecipe.ToDto());
            })
            .WithTags("Recipes")
            .WithName("CreateRecipe")
            .RequireAuthorization();
    }
}