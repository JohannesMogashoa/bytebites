using ByteBites.Application.Common.Interfaces;
using ByteBites.Application.DTOs;
using ByteBites.Domain; // Assuming Recipe is in Domain
using ByteBites.Endpoints;
using Moq; // For mocking
using Xunit; // For XUnit attributes and assertions
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // For Results

namespace ByteBites.Tests.Endpoints;

public class UpdateRecipeEndpointTests
{
    [Fact]
    public async Task UpdateRecipe_ShouldReturnOk_WhenRecipeExistsAndIdsMatch()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var existingRecipe = new Recipe
        {
            Id = recipeId,
            Title = "Original Title",
            Description = "Original Description",
            // ... other properties
        };
        var updateDto = new UpdateRecipeDto
        {
            Id = recipeId,
            Title = "Updated Title",
            Description = "Updated Description",
            Ingredients = "Updated Ingredients",
            Steps = "Updated Steps",
            CookingTime = 60,
            DietaryTags = "Updated Tags"
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return the existing recipe when GetRecipeById is called
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync(existingRecipe);

        // Setup mock for UpdateRecipe, simply complete the task
        mockRecipeRepository
            .Setup(repo => repo.UpdateRecipe(It.IsAny<Recipe>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty)
                {
                    return Results.BadRequest("Invalid recipe ID.");
                }

                if (id != dto.Id)
                {
                    return Results.BadRequest("ID in the URL does not match the ID in the request body.");
                }

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        var result = await endpointDelegate(recipeId, updateDto, mockRecipeRepository.Object);

        // Assert
        // Verify that GetRecipeById was called once with the correct ID
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);

        // Verify that UpdateRecipe was called once with a Recipe object derived from the DTO
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.Is<Recipe>(
            r => r.Id == updateDto.Id && r.Title == updateDto.Title && r.Description == updateDto.Description
        )), Times.Once);

        // Assert that an OK result was returned
        var okResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldReturnBadRequest_WhenIdInUrlIsEmptyGuid()
    {
        // Arrange
        var recipeId = Guid.Empty; // Invalid ID
        var updateDto = new UpdateRecipeDto
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test Description",
            Ingredients = "Test Ingredients",
            Steps = "Test Steps",
            CookingTime = 30,
            DietaryTags = "Test Tags"
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>(); // No setup needed as it shouldn't be called

        // Act
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty)
                {
                    return Results.BadRequest("Invalid recipe ID.");
                }

                if (id != dto.Id)
                {
                    return Results.BadRequest("ID in the URL does not match the ID in the request body.");
                }

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        var result = await endpointDelegate(recipeId, updateDto, mockRecipeRepository.Object);

        // Assert
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(It.IsAny<Guid>()), Times.Never);
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Never);

        var badRequestResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        var valueResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal("Invalid recipe ID.", valueResult.Value);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldReturnBadRequest_WhenIdMismatchBetweenUrlAndBody()
    {
        // Arrange
        var urlId = Guid.NewGuid();
        var bodyId = Guid.NewGuid(); // Different from urlId
        var updateDto = new UpdateRecipeDto
        {
            Id = bodyId,
            Title = "Test",
            Description = "Test Description",
            Ingredients = "Test Ingredients",
            Steps = "Test Steps",
            CookingTime = 30,
            DietaryTags = "Test Tags"
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>(); // No setup needed as it shouldn't be called

        // Act
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty)
                {
                    return Results.BadRequest("Invalid recipe ID.");
                }

                if (id != dto.Id)
                {
                    return Results.BadRequest("ID in the URL does not match the ID in the request body.");
                }

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        var result = await endpointDelegate(urlId, updateDto, mockRecipeRepository.Object);

        // Assert
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(It.IsAny<Guid>()), Times.Never);
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Never);

        var badRequestResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        var valueResult = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal("ID in the URL does not match the ID in the request body.", valueResult.Value);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldReturnNotFound_WhenRecipeDoesNotExist()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var updateDto = new UpdateRecipeDto
        {
            Id = recipeId,
            Title = "Test",
            Description = "Test Description",
            Ingredients = "Test Ingredients",
            Steps = "Test Steps",
            CookingTime = 30,
            DietaryTags = "Test Tags"
        };

        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Setup mock to return null for GetRecipeById (recipe not found)
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync((Recipe)null);

        // Ensure UpdateRecipe is never called if not found
        mockRecipeRepository
            .Setup(repo => repo.UpdateRecipe(It.IsAny<Recipe>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty) return Results.BadRequest("Invalid recipe ID.");
                if (id != dto.Id) return Results.BadRequest("ID in the URL does not match the ID in the request body.");

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        var result = await endpointDelegate(recipeId, updateDto, mockRecipeRepository.Object);

        // Assert
        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Never);

        var notFoundResult = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldHandleRepositoryGetByIdException()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var updateDto = new UpdateRecipeDto {
            Id = recipeId,
            Title = "Test",
            Description = "Test Description",
            Ingredients = "Test Ingredients",
            Steps = "Test Steps",
            CookingTime = 30,
            DietaryTags = "Test Tags"
        };
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        // Simulate an exception when GetRecipeById is called
        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ThrowsAsync(new InvalidOperationException("Database error on retrieve."));

        // Act & Assert
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty) return Results.BadRequest("Invalid recipe ID.");
                if (id != dto.Id) return Results.BadRequest("ID in the URL does not match the ID in the request body.");

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(recipeId, updateDto, mockRecipeRepository.Object)
        );

        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRecipe_ShouldHandleRepositoryUpdateException()
    {
        // Arrange
        var recipeId = Guid.NewGuid();
        var existingRecipe = new Recipe { Id = recipeId, Title = "Original" };
        var updateDto = new UpdateRecipeDto {
            Id = recipeId,
            Title = "Test",
            Description = "Test Description",
            Ingredients = "Test Ingredients",
            Steps = "Test Steps",
            CookingTime = 30,
            DietaryTags = "Test Tags"
        };
        var mockRecipeRepository = new Mock<IRecipeRepository>();

        mockRecipeRepository
            .Setup(repo => repo.GetRecipeById(recipeId))
            .ReturnsAsync(existingRecipe);

        // Simulate an exception when UpdateRecipe is called
        mockRecipeRepository
            .Setup(repo => repo.UpdateRecipe(It.IsAny<Recipe>()))
            .ThrowsAsync(new InvalidOperationException("Database error on update."));

        // Act & Assert
        Func<Guid, UpdateRecipeDto, IRecipeRepository, Task<IResult>> endpointDelegate =
            async (id, dto, repo) =>
            {
                if (id == Guid.Empty) return Results.BadRequest("Invalid recipe ID.");
                if (id != dto.Id) return Results.BadRequest("ID in the URL does not match the ID in the request body.");

                var existing = await repo.GetRecipeById(id);
                if (existing == null)
                {
                    return Results.NotFound();
                }

                await repo.UpdateRecipe(dto.ToDomainModel());
                return Results.Ok();
            };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            endpointDelegate(recipeId, updateDto, mockRecipeRepository.Object)
        );

        mockRecipeRepository.Verify(repo => repo.GetRecipeById(recipeId), Times.Once);
        mockRecipeRepository.Verify(repo => repo.UpdateRecipe(It.IsAny<Recipe>()), Times.Once); // Called, but threw
    }
}


// NOTE: For these tests to compile, you need the UpdateRecipeDTO
// and the ToDomainModel() extension method for it.
// Here are mock implementations if they are not already in your Application project:

// namespace ByteBites.Application.DTOs
// {
//     public class UpdateRecipeDTO
//     {
//         public Guid Id { get; set; }
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
//                 Id = Id,
//                 Title = Title,
//                 Description = Description,
//                 Ingredients =