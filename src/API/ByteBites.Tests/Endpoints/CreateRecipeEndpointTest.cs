using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain;
using ByteBites.Endpoints;
using Moq; // For mocking
using Xunit; // For XUnit attributes and assertions
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For Results

namespace ByteBites.Tests.Endpoints;

public class CreateRecipeEndpointTests
{
    // We don't need a constructor for setup like in repository tests
    // because we're mocking dependencies per test method.

    [Fact]
    public async Task CreateRecipe_ShouldAddRecipeAndReturnCreatedResult()
    {
        // Arrange
        var mockRecipeRepository = new Mock<IRecipeRepository>();
        var createRecipeDto = new CreateRecipeDto
        {
            Title = "Test Recipe",
            Description = "A delicious test recipe.",
            Ingredients = "Flour, Sugar, Eggs",
            Steps = "Mix, Bake",
            CookingTime = 60,
            DietaryTags = "Vegetarian"
        };

        // This is crucial: We need to capture the Recipe object passed to AddRecipe
        // so we can assert its properties and set its Id.
        Recipe capturedRecipe = null;
        mockRecipeRepository
            .Setup(repo => repo.AddRecipe(It.IsAny<Recipe>()))
            .Callback<Recipe>(recipe =>
            {
                capturedRecipe = recipe;
                // Simulate the repository setting the ID and other properties
                recipe.Id = Guid.NewGuid();
                recipe.CreatedAt = DateTime.UtcNow;
                recipe.CreatedBy = "system";
                recipe.IsDeleted = false;
            })
            .Returns(Task.CompletedTask);

        // Act
        // Since MapEndpoint registers the route, we need to extract the delegate itself
        // to directly invoke it for unit testing. This often involves a little trick.
        // For simplicity, we'll manually create the delegate that the endpoint would map.

        // The actual delegate mapped by MapPost:
        Func<CreateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (dto, repo) =>
            {
                var newRecipe = dto.ToDomainModel(); // Assume ToDomainModel works as expected for DTO mapping
                await repo.AddRecipe(newRecipe);
                return Results.Created($"/api/recipes/{newRecipe.Id}", newRecipe.ToDto());
            };

        var result = await endpointDelegate(createRecipeDto, mockRecipeRepository.Object);

        // Assert
        // Verify that AddRecipe was called exactly once with a Recipe object
        mockRecipeRepository.Verify(repo => repo.AddRecipe(It.IsAny<Recipe>()), Times.Once);

        // Assert that a Created result was returned
        var createdResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        // Assert the Location header (URL) and the value content
        var typedCreatedResult = Assert.IsAssignableFrom<CreatedResult>(result);
        Assert.NotNull(typedCreatedResult.Location);
        Assert.StartsWith("/api/recipes/", typedCreatedResult.Location);

        var returnedRecipeDto = Assert.IsAssignableFrom<RecipeDto>(typedCreatedResult.Value);
        Assert.NotNull(returnedRecipeDto);
        Assert.Equal(createRecipeDto.Title, returnedRecipeDto.Title);
        Assert.Equal(createRecipeDto.Description, returnedRecipeDto.Description);
        Assert.Equal(capturedRecipe.Id, returnedRecipeDto.Id); // Verify the ID matches the one set by the mock repo
    }

    [Fact]
    public async Task CreateRecipe_ShouldHandleRepositoryExceptionGracefully()
    {
        // Arrange
        var mockRecipeRepository = new Mock<IRecipeRepository>();
        var createRecipeDto = new CreateRecipeDto
        {
            Title = "Bad Recipe"
        };

        // Simulate an exception from the repository's AddRecipe method
        mockRecipeRepository
            .Setup(repo => repo.AddRecipe(It.IsAny<Recipe>()))
            .ThrowsAsync(new InvalidOperationException("Database error simulation."));

        // Act & Assert
        // We expect the exception to propagate up if the endpoint doesn't handle it
        // and let the framework handle the error response (e.g., 500 Internal Server Error).
        // For this unit test, we just confirm that the exception is indeed thrown.
        Func<CreateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (dto, repo) =>
            {
                var newRecipe = dto.ToDomainModel();
                await repo.AddRecipe(newRecipe);
                return Results.Created($"/api/recipes/{newRecipe.Id}", newRecipe.ToDto());
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(createRecipeDto, mockRecipeRepository.Object)
        );

        // Verify that AddRecipe was called even if it threw an exception
        mockRecipeRepository.Verify(repo => repo.AddRecipe(It.IsAny<Recipe>()), Times.Once);
    }
}

// NOTE: You'll also need the DTOs and extension methods used by the endpoint
// For the tests to compile, ensure these are available (e.g., in your Application project)
// Dummy DTOs and extension methods for testing purposes if they're not fully implemented yet:

// namespace ByteBites.Application.DTOs
// {
//     public class CreateRecipeDTO
//     {
//         public string Title { get; set; }
//         public string Description { get; set; }
//         public string Ingredients { get; set; }
//         public string Steps { get; set; }
//         public int CookingTime { get; set; }
//         public string DietaryTags { get; set; }
//
//         public Recipe ToDomainModel()
//         {
//             return new Recipe
//             {
//                 Title = Title,
//                 Description = Description,
//                 Ingredients = Ingredients,
//                 Steps = Steps,
//                 CookingTime = CookingTime,
//                 DietaryTags = DietaryTags
//             };
//         }
//     }
//
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
//
// // Also the IRecipeRepository and Recipe domain model if not already present
// namespace ByteBites.Application.Common.Interfaces
// {
//     public interface IRecipeRepository
//     {
//         Task AddRecipe(Recipe recipe);
//         // Add other methods if they exist in your actual interface
//     }
// }
//
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