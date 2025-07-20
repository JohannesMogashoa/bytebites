using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;

namespace ByteBites.Endpoints;

public class UpdateRecipe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/recipes/{id:guid}", async (Guid id, UpdateRecipeDto updateRecipeDto, IRecipeRepository recipeRepository) =>
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
            .WithName("UpdateRecipe")
            .RequireAuthorization();
    }
}