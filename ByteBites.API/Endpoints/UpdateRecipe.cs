using ByteBites.API.Application.Common.Interfaces;
using ByteBites.API.Application.DTOs;

namespace ByteBites.API.Endpoints;

public class UpdateRecipe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/recipes/{id:guid}", async (Guid id, UpdateRecipeDTO updateRecipeDto, IRecipeRepository recipeRepository) =>
            {
                if(id == Guid.Empty){
                    return Results.BadRequest("Invalid recipe ID.");
                }
                
                if(id != updateRecipeDto.Id){
                    return Results.BadRequest("ID in the URL does not match the ID in the request body.");
                }
                
                var existingRecipe = await recipeRepository.GetRecipeById(id);
                if (existingRecipe == null)
                {
                    return Results.NotFound();
                }

                await recipeRepository.UpdateRecipe(updateRecipeDto.ToDomainModel());
                return Results.Ok();
            })
            .WithTags("Recipes")
            .WithName("UpdateRecipe");
    }
}