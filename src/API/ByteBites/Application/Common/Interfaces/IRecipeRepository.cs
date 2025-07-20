using ByteBites.Application.DTOs;
using ByteBites.Domain;

namespace ByteBites.Application.Common.Interfaces;

public interface IRecipeRepository
{
    Task<IEnumerable<Recipe>> GetAllRecipes();
    Task<Recipe?> GetRecipeById(Guid id);
    Task AddRecipe(Recipe recipe);
    Task UpdateRecipe(Recipe recipe);
    Task DeleteRecipe(Guid id);
    Task<IEnumerable<Recipe>> FilterRecipes(FilterRecipeDto filter);
}