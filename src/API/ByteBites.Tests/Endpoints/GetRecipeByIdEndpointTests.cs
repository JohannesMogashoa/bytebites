using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain; // Assuming Recipe is in Domain
using ByteBites.Endpoints;
using Moq; // For mocking
using Xunit; // For XUnit attributes and assertions
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For Results

namespace ByteBites.Tests.Endpoints;

public class GetRecipeByIdEndpointTests
{
    [Fact]
    public async Task GetRecipeById_ShouldReturnOkWithRecipeDTO_WhenRecipeExists()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var domainRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Delicious Cake",
            Description = "A sweet treat.",
            Ingredients = "Sugar, Flour, Eggs",
            Steps = "Mix and bake.",
            CookingTime = 45,
            DietaryTags = "Dessert",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "TestUser",
            IsDeleted = false
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return the domain recipe when GetRecipeById is called with the specific ID
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync(domainRecipe);

        // Act
        // Extract the delegate mapped by the endpoint
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                // Assume ToDto() extension method exists and works correctly
                return recipe == null ? Results.NotFound() : Results.Ok(recipe.ToDto());
            };

        var result = await endpointDelegate(recipeId, mockRecipeRepository.Object);

        // Assert
        // Verify that GetRecipeById was called exactly once with the correct ID
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);

        // Assert that an OK result was returned
        var okResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        // Assert the type and content of the returned value
        var typedOkResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        Assert.NotNull(typedOkResult.Value);

        var returnedDto = Assert.IsAssignableFrom<RecipeDto>(typedOkResult.Value);
        Assert.NotNull(returnedDto);
        Assert.Equal(domainRecipe.Id, returnedDto.Id);
        Assert.Equal(domainRecipe.Title, returnedDto.Title);
        Assert.Equal(domainRecipe.Description, returnedDto.Description);
        Assert.Equal(domainRecipe.CookingTime, returnedDto.CookingTime);
        // Add more assertions for other properties if needed
    }

    [Fact]
    public async Task GetRecipeById_ShouldReturnNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var nonExistentRecipeId = Guid.NewGuid();

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return null when GetRecipeById is called (recipe not found)
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(nonExistentRecipeId))
            .ReturnsAsync((Recipe)null); // Explicitly return null

        // Act
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                return recipe == null ? Results.NotFound() : Results.Ok(recipe.ToDto());
            };

        var result = await endpointDelegate(nonExistentRecipeId, mockRecipeRepository.Object);

        // Assert
        // Verify that GetRecipeById was called once
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(nonExistentRecipeId), Times.Once);

        // Assert that a NotFound result was returned
        var notFoundResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetRecipeById_ShouldHandleRepositoryException()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Simulate an exception when GetRecipeById is called
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ThrowsAsync(new InvalidOperationException("Simulated database error on retrieve."));

        // Act & Assert
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                return recipe == null ? Results.NotFound() : Results.Ok(recipe.ToDto());
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(recipeId, mockRecipeRepository.Object)
        );

        // Verify that GetRecipeById was still called once
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
    }
}

// NOTE: For these tests to compile, you need the RecipeDTO
// and the ToDto() extension method.
// Here are mock implementations if they are not already in your Application project:

// namespace ByteBites.Application.DTOs
// {
//     public class RecipeDTO
//     {
//         public Guid Id { get; set; }
//         public string Title { get; set; }
//         public string Description { get; set; }
//         public string Ingredients { get; set; }
//         public string Steps { get; set; }
//         public int CookingTime { get; set; }
//         public string DietaryTags { get; set; }
//         public DateTime CreatedAt { get; set; }
//         public string CreatedBy { get; set; }
//         public DateTime? UpdatedAt { get; set; }
//         public string UpdatedBy { get; set; }
//         public bool IsDeleted { get; set; }
//     }
//
//     public static class RecipeMappingExtensions
//     {
//         public static RecipeDTO ToDto(this Recipe recipe)
//         {
//             if (recipe == null) return null; // Defensive check for mapping
//             return new RecipeDTO
//             {
//                 Id = recipe.Id,
//                 Title = recipe.Title,
//                 Description = recipe.Description,
//                 Ingredients = recipe.Ingredients,
//                 Steps = recipe.Steps,
//                 CookingTime = recipe.CookingTime,
//                 DietaryTags = recipe.DietaryTags,
//                 CreatedAt = recipe.CreatedAt,
//                 CreatedBy = recipe.CreatedBy,
//                 UpdatedAt = recipe.UpdatedAt,
//                 UpdatedBy = recipe.UpdatedBy,
//                 IsDeleted = recipe.IsDeleted
//             };
//         }
//     }
// }

// // Also ensure IRecipeRepository and Recipe domain model are accessible
// namespace ByteBites.Application.Common.Interfaces
// {
//     public interface IRecipeRepository
//     {
//         Task<Recipe?> GetRecipeById(Guid id);
//         // Other methods like GetAllRecipes, AddRecipe etc.
//     }
// }

// namespace ByteBites.Domain
// {
//     public class Recipe
//     {
//         public Guid Id { get; set; }
//         public string Title { get; set; }
//         public string Description { get; set; }
//         public string Ingredients { get; set; }
//         public string Steps { get; set; }
//         public int CookingTime { get; set; }
//         public string DietaryTags { get; set; }
//         public DateTime CreatedAt { get; set; }
//         public string CreatedBy { get; set; }
//         public DateTime? UpdatedAt { get; set; }
//         public string UpdatedBy { get; set; }
//         public bool IsDeleted { get; set; }
//     }
// }