import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import { createStore } from "zustand";

export type RecipeState = {
  recipes: IRecipeListItem[];
  loading: boolean;
  error: string | null;
  filters: object;
};

export type RecipeStoreActions = {
  setRecipes: (recipes: IRecipeListItem[]) => void;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  setFilters: (filters: object) => void;
  resetFilters: () => void;
};

export type RecipeStore = RecipeState & RecipeStoreActions;

export const defaultInitState: RecipeState = {
  recipes: [],
  loading: false,
  error: null,
  filters: {},
};

export const createRecipeStore = (
  initState: RecipeState = defaultInitState,
) => {
  return createStore<RecipeStore>()((set) => ({
    ...initState,
    setRecipes: (recipes: IRecipeListItem[]) => set({ recipes }),
    setLoading: (loading: boolean) => set({ loading }),
    setError: (error: string | null) => set({ error }),
    setFilters: (filters: object) => set({ filters }),
    resetFilters: () => set({ filters: {} }),
  }));
};
