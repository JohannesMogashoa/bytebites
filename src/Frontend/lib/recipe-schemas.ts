import { z } from "zod";

// Zod schema for CreateRecipeDTO
export const CreateRecipeSchema = z.object({
  title: z.string().min(3, "Title is required"),
  description: z.string(),
  ingredients: z.string().min(3, "Ingredients are required"),
  steps: z.string().min(3, "Steps are required"),
  cookingTime: z.coerce
    .number()
    .min(1, "Cooking time must be at least 1 minute"),
  dietaryTags: z.string().min(1, "Dietary tags are required"),
});

// Zod schema for UpdateRecipeDTO
export const UpdateRecipeSchema = z.object({
  id: z.string().uuid(),
  title: z.string(),
  description: z.string(),
  ingredients: z.string(),
  steps: z.string(),
  cookingTime: z.coerce.number(),
  dietaryTags: z.string(),
});

export const FilterRecipesSchema = z.object({
  title: z.string().optional(),
  dietaryTags: z.string().optional(),
  ingredients: z.string().optional(),
  cookingTime: z.coerce.number().optional(),
});

// TypeScript types derived from Zod schemas
export type CreateRecipeDTO = z.infer<typeof CreateRecipeSchema>;
export type UpdateRecipeDTO = z.infer<typeof UpdateRecipeSchema>;
export type FilterRecipesDTO = z.infer<typeof FilterRecipesSchema>;
