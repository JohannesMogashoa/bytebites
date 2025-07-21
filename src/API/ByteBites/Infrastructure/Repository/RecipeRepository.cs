using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain;
using ByteBites.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.Infrastructure.Repository;

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
        
        dbContext.Remove(recipe);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Recipe>> FilterRecipes(FilterRecipeDto filter)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Build the query based on the filter criteria
        var query = dbContext.Recipes.AsNoTracking().AsEnumerable();

        if (!string.IsNullOrEmpty(filter.Title))
        {
            query = query.Where(r => r.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filter.DietaryTags))
        {
            query = query.Where(r => r.DietaryTags.Contains(filter.DietaryTags, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(filter.Ingredients))
        {
            query = query.Where(r => r.Ingredients.Contains(filter.Ingredients, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.CookingTime.HasValue)
        {
            query = query.Where(r => r.CookingTime <= filter.CookingTime.Value);
        }

        return query;
    }

    public async Task<IEnumerable<Recipe>> GetUserRecipes(string userId)
    {
        // Simulate asynchronous operation
        await Task.Delay(100);
        
        // Return recipes created by the specified user, filtering out soft-deleted ones
        return await dbContext.Recipes
            .Where(r => r.CreatedBy == userId)
            .ToListAsync();
    }
}