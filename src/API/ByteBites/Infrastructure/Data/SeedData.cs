using ByteBites.Domain;
using Microsoft.EntityFrameworkCore;

namespace ByteBites.Infrastructure.Data;

public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>());
            
            // Look for any recipes.
            if (context.Recipes.Any())
            {
                return;   // DB has been seeded
            }

            context.Recipes.AddRange(
                new Recipe
                {
                    Title = "Classic Spaghetti Bolognese",
                    Ingredients = "Ground beef, diced tomatoes, onion, garlic, pasta, olive oil, basil, oregano, salt, pepper.",
                    Steps = "1. Brown beef. 2. Sauté onion and garlic. 3. Add tomatoes and seasonings, simmer. 4. Cook pasta. 5. Combine and serve.",
                    CookingTime = 45,
                    DietaryTags = "Non-Vegetarian, Italian"
                },
                new Recipe
                {
                    Title = "Vegetarian Chili",
                    Ingredients = "Kidney beans, black beans, corn, diced tomatoes, bell peppers, onion, chili powder, cumin, vegetable broth.",
                    Steps = "1. Sauté onion and bell peppers. 2. Add beans, corn, tomatoes, spices, and broth. 3. Simmer for 30 minutes.",
                    CookingTime = 50,
                    DietaryTags = "Vegetarian, Vegan, Gluten-Free"
                },
                new Recipe
                {
                    Title = "Simple Chicken Stir-Fry",
                    Ingredients = "Chicken breast, broccoli, carrots, bell peppers, soy sauce, ginger, garlic, rice, sesame oil.",
                    Steps = "1. Cook rice. 2. Stir-fry chicken. 3. Add vegetables, ginger, garlic. 4. Add soy sauce. 5. Serve over rice.",
                    CookingTime = 30,
                    DietaryTags = "Non-Vegetarian, Asian"
                }
            );
            context.SaveChanges();
        }
    }