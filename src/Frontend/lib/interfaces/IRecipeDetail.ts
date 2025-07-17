export interface IRecipeDetail {
  id: string;
  title: string;
  description: string;
  ingredients: string;
  steps: string;
  cookingTime: number;
  dietaryTags: string;
  createdAt: string;
  updatedAt: string | null;
  createdBy: string;
  updatedBy: string | null;
}
