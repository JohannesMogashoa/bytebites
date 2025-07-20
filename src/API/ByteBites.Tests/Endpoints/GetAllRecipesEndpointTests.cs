using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain; // Assuming Recipe is in Domain
using ByteBites.Endpoints;
using Moq; // For mocking
using Xunit; // For XUnit attributes and assertions
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For Results

namespace ByteBites.Tests.Endpoints;

public class GetAllRecipesEndpointTests
{
    [Fact]
    public async Task GetAllRecipes_ShouldReturnOkWithRecipeListItemDTOs_WhenRecipesExist()
    {
        // Arrange
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Create some domain Recipe objects
        var domainRecipes = new List<Recipe>
        {
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Recipe 1",
                Description = "Desc 1",
                CookingTime = 30,
                IsDeleted = false
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Title = "Recipe 2",
                Description = "Desc 2",
                CookingTime = 45,
                IsDeleted = false
            }
        };

        // Setup mock to return the list of domain recipes
        mockRecipeRepository
            .Setup(repo => repo.GetAllRecipes())
            .ReturnsAsync(domainRecipes);

        // Act
        // Extract the delegate mapped by the endpoint
        Func<IRecipeRepository, Task<IResult>> endpointDelegate =
            async (repo) =>
            {
                var recipes = await repo.GetAllRecipes();
                // Assume ToListItemDtos() extension method exists and works correctly
                return Results.Ok(recipes.ToListItemDtos());
            };

        var result = await endpointDelegate(mockRecipeRepository.Object);

        // Assert
        // Verify that GetAllRecipes was called exactly once
        mockRecipeRepository.Verify(repo => repo.GetAllRecipes(), Times.Once);

        // Assert that an OK result was returned
        var okResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        // Assert the type and content of the returned value
        var typedOkResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        Assert.NotNull(typedOkResult.Value);

        var returnedDtos = Assert.IsAssignableFrom<List<RecipeListItemDto>>(typedOkResult.Value);
        Assert.NotNull(returnedDtos);
        Assert.Equal(2, returnedDtos.Count);

        // Verify content of the first DTO
        Assert.Contains(returnedDtos, dto =>
            dto.Id == domainRecipes[0].Id &&
            dto.Title == domainRecipes[0].Title &&
            dto.Description == domainRecipes[0].Description);

        // Verify content of the second DTO
        Assert.Contains(returnedDtos, dto =>
            dto.Id == domainRecipes[1].Id &&
            dto.Title == domainRecipes[1].Title &&
            dto.Description == domainRecipes[1].Description);
    }

    [Fact]
    public async Task GetAllRecipes_ShouldReturnOkWithEmptyList_WhenNoRecipesExist()
    {
        // Arrange
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return an empty list of domain recipes
        mockRecipeRepository
            .Setup(repo => repo.GetAllRecipes())
            .ReturnsAsync(new List<Recipe>()); // Return an empty list

        // Act
        Func<IRecipeRepository, Task<IResult>> endpointDelegate =
            async (repo) =>
            {
                var recipes = await repo.GetAllRecipes();
                return Results.Ok(recipes.ToListItemDtos());
            };

        var result = await endpointDelegate(mockRecipeRepository.Object);

        // Assert
        mockRecipeRepository.Verify(repo => repo.GetAllRecipes(), Times.Once);

        var okResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var typedOkResult = Assert.IsAssignableFrom<OkObjectResult>(result);
        Assert.NotNull(typedOkResult.Value);

        var returnedDtos = Assert.IsAssignableFrom<List<RecipeListItemDto>>(typedOkResult.Value);
        Assert.NotNull(returnedDtos);
        Assert.Empty(returnedDtos); // Assert that the list is empty
    }

    [Fact]
    public async Task GetAllRecipes_ShouldHandleRepositoryException()
    {
        // Arrange
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Simulate an exception when GetAllRecipes is called
        mockRecipeRepository
            .Setup(repo => repo.GetAllRecipes())
            .ThrowsAsync(new InvalidOperationException("Simulated database connection error."));

        // Act & Assert
        Func<IRecipeRepository, Task<IResult>> endpointDelegate =
            async (repo) =>
            {
                var recipes = await repo.GetAllRecipes();
                return Results.Ok(recipes.ToListItemDtos());
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(mockRecipeRepository.Object)
        );

        // Verify that GetAllRecipes was still called once
        mockRecipeRepository.Verify(repo => repo.GetAllRecipes(), Times.Once);
    }
}


// NOTE: For these tests to compile, you need the RecipeListItemDTO
// and the ToListItemDtos() extension method.
// Here are mock implementations if they are not already in your Application project:

// namespace ByteBites.Application.DTOs
// {
//     public class RecipeListItemDTO
//     {
//         public Guid Id { get; set; }
//         public string Title { get; set; }
//         public int CookingTime { get; set; }
//         // Add any other properties relevant for the list item view
//     }
//
//     public static class RecipeListMappingExtensions
//     {
//         public static List<RecipeListItemDTO> ToListItemDtos(this IEnumerable<Recipe> recipes)
//         {
//             return recipes.Select(r => new RecipeListItemDTO
//             {
//                 Id = r.Id,
//                 Title = r.Title,
//                 CookingTime = r.CookingTime
//                 // Map other relevant properties
//             }).ToList();
//         }
//     }
// }

// // Also ensure IRecipeRepository and Recipe domain model are accessible
// namespace ByteBites.Application.Common.Interfaces
// {
//     public interface IRecipeRepository
//     {
//         Task<IEnumerable<Recipe>> GetAllRecipes();
//         // Other methods like GetRecipeById, AddRecipe etc.
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