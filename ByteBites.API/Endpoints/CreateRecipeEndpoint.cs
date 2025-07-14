using ByteBites.API.Application.Common.Interfaces;
using ByteBites.API.Application.DTOs;

namespace ByteBites.API.Endpoints;

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
            .WithName("CreateRecipe");
    }
}