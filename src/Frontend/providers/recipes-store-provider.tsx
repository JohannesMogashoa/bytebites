"use client";

import { type ReactNode, createContext, useRef, useContext } from "react";
import { useStore } from "zustand";

import { type RecipeStore, createRecipeStore } from "@/stores/recipes-store";

export type RecipeStoreApi = ReturnType<typeof createRecipeStore>;

export const RecipeStoreContext = createContext<RecipeStoreApi | undefined>(
  undefined,
);

export interface RecipeStoreProviderProps {
  children: ReactNode;
}

export const RecipeStoreProvider = ({ children }: RecipeStoreProviderProps) => {
  const storeRef = useRef<RecipeStoreApi | null>(null);

  storeRef.current ??= createRecipeStore();

  return (
    <RecipeStoreContext.Provider value={storeRef.current}>
      {children}
    </RecipeStoreContext.Provider>
  );
};

export const useRecipeStore = <T,>(selector: (store: RecipeStore) => T): T => {
  const recipeStoreContext = useContext(RecipeStoreContext);

  if (!recipeStoreContext) {
    throw new Error(`useRecipeStore must be used within RecipeStoreProvider`);
  }

  return useStore(recipeStoreContext, selector);
};
