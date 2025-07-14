using ByteBites.API.Application.Common.Interfaces;

namespace ByteBites.API.Endpoints;

public class DeleteRecipe : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/recipes/{id:guid}", async (Guid id, IRecipeRepository recipeRepository) =>
            {
                var recipe = await recipeRepository.GetRecipeById(id);
                if (recipe == null)
                {
                    return Results.NotFound();
                }

                await recipeRepository.DeleteRecipe(id);
                return Results.NoContent();
            })
            .WithTags("Recipes")
            .WithName("DeleteRecipe");
    }
}