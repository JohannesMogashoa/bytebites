using ByteBites.API.Application.Common.Interfaces;
using ByteBites.API.Domain;
using ByteBites.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.API.Infrastructure.Repository;

public class RecipeRepository(ApplicationDbContext dbContext) : IRecipeRepository
{
    public async Task<IEnumerable<Recipe>> GetAllRecipes()
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Return all recipes, filtering out soft-deleted ones
        return await dbContext.Recipes.ToListAsync();
    }

    public async Task<Recipe?> GetRecipeById(Guid id)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Find the recipe by ID, returning null if not found or if soft-deleted
        return await dbContext.Recipes.FindAsync(id);
    }

    public async Task AddRecipe(Recipe recipe)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Set the creation properties
        recipe.Id = Guid.NewGuid();
        recipe.CreatedAt = DateTime.UtcNow;
        recipe.UpdatedAt = null;
        recipe.CreatedBy = "system"; // Replace with actual user context if available
        recipe.IsDeleted = false;

        // Add the recipe to the in-memory list
        dbContext.Add(recipe);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRecipe(Recipe recipe)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Find the existing recipe by ID
        var existingRecipe = await dbContext.Recipes.FindAsync(recipe.Id);
        if (existingRecipe == null)
        {
            throw new KeyNotFoundException("Recipe not found or has been deleted.");
        }

        // Update the properties
        existingRecipe.Title = recipe.Title;
        existingRecipe.Description = recipe.Description;
        existingRecipe.Ingredients = recipe.Ingredients;
        existingRecipe.Steps = recipe.Steps;
        existingRecipe.CookingTime = recipe.CookingTime;
        existingRecipe.DietaryTags = recipe.DietaryTags;
        existingRecipe.UpdatedAt = DateTime.UtcNow;
        existingRecipe.UpdatedBy = "system"; // Replace with actual user context if available
        
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteRecipe(Guid id)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Find the recipe by ID
        var recipe = await dbContext.Recipes.FindAsync(id);
        if (recipe == null)
        {
            throw new KeyNotFoundException("Recipe not found or has been deleted.");
        }

        // Soft delete the recipe
        recipe.IsDeleted = true;
        recipe.UpdatedAt = DateTime.UtcNow;
        recipe.UpdatedBy = "system"; // Replace with actual user context if available
        
        dbContext.Update(recipe);
        await dbContext.SaveChangesAsync();
    }
}