"use client";

import type {
  FilterRecipesDTO,
  FilterRecipesSchema,
} from "@/lib/recipe-schemas";
import { useEffect, useState } from "react";

import FilterRecipes from "./FilterRecipes";
import RecipeListItemComponent from "./RecipeListItemComponent";
import { useRecipes } from "@/hooks/useRecipes";
import type z from "zod";

export const RecipeListingPageComponent = () => {
  const {
    recipes,
    loading,
    error,
    filters, // These filters reflect the current Zustand state
    setRecipeFilters,
    resetRecipeFilters,
    fetchRecipes, // Expose for manual refetching (e.g., a "Refresh" button)
  } = useRecipes();

  const [localFilters, setLocalFilters] = useState<FilterRecipesDTO>(filters);

  useEffect(() => {
    setLocalFilters(filters);
  }, [filters]);

  const handleSubmitFilters = (values: z.infer<typeof FilterRecipesSchema>) => {
    setRecipeFilters(values); // Apply and sync to URL
  };

  if (loading && recipes.length === 0) {
    // Only show loading if no data is currently displayed
    return <div className="p-4 text-center">Loading recipes...</div>;
  }

  if (error) {
    return (
      <div className="p-4 text-center text-red-600">
        Error: {error}
        <button
          onClick={() => void fetchRecipes()}
          className="ml-4 rounded bg-blue-500 px-3 py-1 text-white hover:bg-blue-600"
        >
          Try Again
        </button>
      </div>
    );
  }

  return (
    <div>
      {loading && recipes.length > 0 && (
        <div className="my-4 text-center text-gray-500">
          Updating recipes...
        </div>
      )}

      {recipes.length === 0 && !loading && (
        <p className="py-10 text-center text-gray-500">
          No recipes found matching your criteria.
        </p>
      )}

      <FilterRecipes
        handleSubmit={handleSubmitFilters}
        resetFilters={resetRecipeFilters}
      />

      <div className="flex flex-col space-y-5">
        {recipes.map((recipe) => (
          <RecipeListItemComponent recipe={recipe} key={recipe.id} />
        ))}
      </div>
    </div>
  );
};
