import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import { createStore } from "zustand/vanilla";

export type RecipeState = {
  recipes: IRecipeListItem[];
};

export type RecipeStoreActions = {
  setRecipes: (recipes: IRecipeListItem[]) => void;
};

export type RecipeStore = RecipeState & RecipeStoreActions;

export const defaultInitState: RecipeState = {
  recipes: [],
};

export const createRecipeStore = (
  initState: RecipeState = defaultInitState,
) => {
  return createStore<RecipeStore>()((set) => ({
    ...initState,
    setRecipes: (recipes: IRecipeListItem[]) => set({ recipes }),
  }));
};
