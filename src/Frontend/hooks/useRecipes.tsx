"use client";

import Error from "next/error";
import { useCallback, useEffect, useRef } from "react";
import { filterRecipes } from "@/server/create-recipe";
import { useRecipeStore } from "@/providers/recipes-store-provider";
import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import { type FilterRecipesDTO } from "@/lib/recipe-schemas";

interface UseRecipesHook {
  recipes: IRecipeListItem[];
  loading: boolean;
  error: string | null;
  filters: FilterRecipesDTO;
  fetchRecipes: (params?: FilterRecipesDTO) => Promise<void>;
  setRecipeFilters: (filters: FilterRecipesDTO) => void;
  resetRecipeFilters: () => void;
}

export const useRecipes = (): UseRecipesHook => {
  const {
    recipes,
    loading,
    error,
    filters,
    setRecipes,
    setLoading,
    setError,
    setFilters,
    resetFilters,
  } = useRecipeStore((state) => state);

  const initialFetchDone = useRef(false);

  // Function to fetch recipes based on filters
  const fetchRecipes = useCallback(
    async (params?: FilterRecipesDTO) => {
      setLoading(true);
      setError(null); // Clear previous errors

      // Merge new params with existing filters from Zustand
      // This allows partial updates to filters while keeping others
      const currentFilters = params ? { ...filters, ...params } : filters;

      try {
        const fetchedRecipes = await filterRecipes(currentFilters);
        setRecipes(fetchedRecipes);
      } catch (err) {
        if (err instanceof Error) {
          setError(err.message);
        } else {
          setError("An unknown error occurred.");
        }
        console.error("Failed to filter recipes:", err);
      } finally {
        setLoading(false);
      }
    },
    [filters, setRecipes, setLoading, setError],
  );

  useEffect(() => {
    // Only fetch if not done initially, or if filters have changed
    if (
      !initialFetchDone.current ||
      JSON.stringify(filters) !== JSON.stringify(useRecipeStore)
    ) {
      void fetchRecipes(); // Fetch with current filters in Zustand
      initialFetchDone.current = true;
    }
  }, [filters, fetchRecipes]); // Depend only on filters and fetchRecipes

  // Function to update filters in Zustand
  const setRecipeFilters = useCallback(
    (newFilters: FilterRecipesDTO) => {
      setFilters(newFilters);
      // The `useEffect` above will react to the `filters` change and trigger `fetchRecipes`
    },
    [setFilters],
  );

  // Function to reset filters in Zustand
  const resetRecipeFilters = useCallback(() => {
    resetFilters(); // Clears filters in Zustand
    // The `useEffect` will react to the `filters` change (now empty) and trigger a re-fetch
  }, [resetFilters]);

  return {
    recipes,
    loading,
    error,
    filters,
    fetchRecipes,
    setRecipeFilters,
    resetRecipeFilters,
  };
};
