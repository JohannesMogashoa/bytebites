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
			.WithDescription("Get all recipes")
			.WithTags("Recipes")
			.Produces<List<RecipeListItemDto>>(StatusCodes.Status200OK);
	}
}