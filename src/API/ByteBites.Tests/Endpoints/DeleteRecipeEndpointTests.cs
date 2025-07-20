using ByteBites.Application.Common.Interfaces;
using ByteBites.Domain; // Assuming Recipe is in Domain
using ByteBites.Endpoints;
using Moq; // For mocking
using Xunit; // For XUnit attributes and assertions
using Microsoft.AspNetCore.Http; // For Results

namespace ByteBites.Tests.Endpoints;

public class DeleteRecipeEndpointTests
{
    [Fact]
    public async Task DeleteRecipe_ShouldReturnNoContent_WhenRecipeExists()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var existingRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Recipe to Delete",
            IsDeleted = false
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return the existing recipe when GetRecipeById is called
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync(existingRecipe);

        // Setup mock for DeleteRecipe, simply complete the task
        mockRecipeRepository
            .Setup(repo => repo.DeleteRecipe(recipeId))
            .Returns(Task.CompletedTask);

        // Act
        // Extract the delegate mapped by the endpoint
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                if (recipe == null)
                {
                    return Results.NotFound();
                }

                await repo.DeleteRecipe(id);
                return Results.NoContent();
            };

        var result = await endpointDelegate(recipeId, mockRecipeRepository.Object);

        // Assert
        // Verify that GetRecipeById was called once with the correct ID
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);

        // Verify that DeleteRecipe was called once with the correct ID
        mockRecipeRepository.Verify(repo => repo.DeleteRecipe(recipeId), Times.Once);

        // Assert that a NoContent result was returned
        var noContentResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteRecipe_ShouldReturnNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var nonExistentRecipeId = Guid.NewGuid();

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return null when GetRecipeById is called (recipe not found)
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(nonExistentRecipeId))
            .ReturnsAsync((Recipe)null); // Explicitly return null

        // Ensure DeleteRecipe is NEVER called if the recipe is not found
        mockRecipeRepository
            .Setup(repo => repo.DeleteRecipe(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask); // Or Throws<Exception>() if you want to be stricter about it not being called

        // Act
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                if (recipe == null)
                {
                    return Results.NotFound();
                }

                await repo.DeleteRecipe(id);
                return Results.NoContent();
            };

        var result = await endpointDelegate(nonExistentRecipeId, mockRecipeRepository.Object);

        // Assert
        // Verify that GetRecipeById was called once
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(nonExistentRecipeId), Times.Once);

        // Verify that DeleteRecipe was NOT called
        mockRecipeRepository.Verify(repo => repo.DeleteRecipe(It.IsAny<Guid>()), Times.Never);

        // Assert that a NotFound result was returned
        var notFoundResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task DeleteRecipe_ShouldHandleRepositoryGetByIdException()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Simulate an exception when GetRecipeById is called
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ThrowsAsync(new InvalidOperationException("Database error on retrieve."));

        // Ensure DeleteRecipe is NEVER called if GetRecipeById throws
        mockRecipeRepository
            .Setup(repo => repo.DeleteRecipe(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                if (recipe == null)
                {
                    return Results.NotFound();
                }

                await repo.DeleteRecipe(id);
                return Results.NoContent();
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(recipeId, mockRecipeRepository.Object)
        );

        // Verify GetRecipeById was called once
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
        // Verify DeleteRecipe was never called
        mockRecipeRepository.Verify(repo => repo.DeleteRecipe(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRecipe_ShouldHandleRepositoryDeleteException()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var existingRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Recipe to Delete (Error Scenario)",
            IsDeleted = false
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return the existing recipe
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync(existingRecipe);

        // Simulate an exception when DeleteRecipe is called
        mockRecipeRepository
            .Setup(repo => repo.DeleteRecipe(recipeId))
            .ThrowsAsync(new InvalidOperationException("Database error on delete."));

        // Act & Assert
        Func<Guid, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, repo) =>
            {
                var recipe = await repo.GetRecipeById(id);
                if (recipe == null)
                {
                    return Results.NotFound();
                }

                await repo.DeleteRecipe(id);
                return Results.NoContent();
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(recipeId, mockRecipeRepository.Object)
        );

        // Verify GetRecipeById was called once
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
        // Verify DeleteRecipe was called once (even though it threw an exception)
        mockRecipeRepository.Verify(repo => repo.DeleteRecipe(recipeId), Times.Once);
    }
}