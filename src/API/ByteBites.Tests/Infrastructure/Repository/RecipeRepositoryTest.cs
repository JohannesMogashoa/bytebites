using ByteBites.Domain;
using ByteBites.Infrastructure.Data;
using ByteBites.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit; // Use Xunit attributes

namespace ByteBites.Tests.Infrastructure.Repository;

// Implement IDisposable for teardown
public class RecipeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RecipeRepository _recipeRepository;

    public RecipeRepositoryTests()
    {
        // Use an in-memory database for testing
        // For XUnit, setup goes into the constructor
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _recipeRepository = new RecipeRepository(_dbContext);
    }

    // Teardown is handled by the Dispose method
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact] // XUnit's equivalent of [Test] for a single test case
    public async Task GetAllRecipes_ShouldReturnAllRecipes()
    {
        // Arrange
        var recipe1 = new Recipe
        {
            Id = Guid.NewGuid(),
            Title = "Recipe 1",
            IsDeleted = false
        };
        var recipe2 = new Recipe
        {
            Id = Guid.NewGuid(),
            Title = "Recipe 2",
            IsDeleted = false
        };
        var recipe3 = new Recipe
        {
            Id = Guid.NewGuid(),
            Title = "Recipe 3",
            IsDeleted = true
        }; // Soft-deleted recipe

        await _dbContext.Recipes.AddRangeAsync(recipe1, recipe2, recipe3);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _recipeRepository.GetAllRecipes();

        // Assert
        // XUnit uses Assert.Collection or individual asserts
        Assert.NotNull(result);
        Assert.Equal(3, result.Count()); // Should return all, even soft-deleted ones based on the current implementation
        Assert.Contains(recipe1, result);
        Assert.Contains(recipe2, result);
        Assert.Contains(recipe3, result);
    }

    [Fact]
    public async Task GetRecipeById_ShouldReturnRecipe_WhenRecipeExistsAndIsNotDeleted()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe
        {
            Id = recipeId,
            Title = "Test Recipe",
            IsDeleted = false
        };
        await _dbContext.Recipes.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _recipeRepository.GetRecipeById(recipeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recipeId, result.Id);
        Assert.Equal("Test Recipe", result.Title);
    }

    [Fact]
    public async Task GetRecipeById_ShouldReturnNull_WhenRecipeDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _recipeRepository.GetRecipeById(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetRecipeById_ShouldReturnRecipe_WhenRecipeExistsAndIsSoftDeleted()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipe = new Recipe
        {
            Id = recipeId,
            Title = "Soft Deleted Recipe",
            IsDeleted = true
        };
        await _dbContext.Recipes.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _recipeRepository.GetRecipeById(recipeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recipeId, result.Id);
        Assert.True(result.IsDeleted);
    }

    [Fact]
    public async Task AddRecipe_ShouldAddRecipeToDatabase()
    {
        // Arrange
        var newRecipe = new Recipe
        {
            Title = "New Recipe",
            Description = "Description",
            Ingredients = "Ingredients",
            Steps = "Steps",
            CookingTime = 30,
            DietaryTags = "Vegan"
        };

        // Act
        await _recipeRepository.AddRecipe(newRecipe);

        // Assert
        var addedRecipe = await _dbContext.Recipes.FindAsync(newRecipe.Id);
        Assert.NotNull(addedRecipe);
        Assert.Equal("New Recipe", addedRecipe.Title);
        Assert.NotEqual(default(DateTime), addedRecipe.CreatedAt);
        Assert.Equal("system", addedRecipe.CreatedBy);
        Assert.False(addedRecipe.IsDeleted);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldUpdateExistingRecipe()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var originalRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Original Title",
            Description = "Original Description",
            Ingredients = "Original Ingredients",
            Steps = "Original Steps",
            CookingTime = 10,
            DietaryTags = "Original Tags",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            CreatedBy = "system"
        };
        await _dbContext.Recipes.AddAsync(originalRecipe);
        await _dbContext.SaveChangesAsync();

        var updatedRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Updated Title",
            Description = "Updated Description",
            Ingredients = "Updated Ingredients",
            Steps = "Updated Steps",
            CookingTime = 60,
            DietaryTags = "Updated Tags"
        };

        // Act
        await _recipeRepository.UpdateRecipe(updatedRecipe);

        // Assert
        var result = await _dbContext.Recipes.FindAsync(recipeId);
        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal("Updated Ingredients", result.Ingredients);
        Assert.Equal("Updated Steps", result.Steps);
        Assert.Equal(60, result.CookingTime);
        Assert.Equal("Updated Tags", result.DietaryTags);
        Assert.NotNull(result.UpdatedAt);
        Assert.Equal("system", result.UpdatedBy);
        Assert.Equal(originalRecipe.CreatedAt, result.CreatedAt); // Should not change
    }

    [Fact]
    public async Task UpdateRecipe_ShouldThrowKeyNotFoundException_WhenRecipeDoesNotExist()
    {
        // Arrange
        var nonExistentRecipe = new Recipe
        {
            Id = Guid.NewGuid(),
            Title = "Non Existent"
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _recipeRepository.UpdateRecipe(nonExistentRecipe)
        );
    }


    [Fact]
    public async Task DeleteRecipe_ShouldRemoveRecipeFromDatabase()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var recipeToDelete = new Recipe
        {
            Id = recipeId,
            Title = "To Be Deleted",
            IsDeleted = false
        };
        await _dbContext.Recipes.AddAsync(recipeToDelete);
        await _dbContext.SaveChangesAsync();

        // Act
        await _recipeRepository.DeleteRecipe(recipeId);

        // Assert
        var result = await _dbContext.Recipes.FindAsync(recipeId);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteRecipe_ShouldThrowKeyNotFoundException_WhenRecipeDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _recipeRepository.DeleteRecipe(nonExistentId)
        );
    }
}