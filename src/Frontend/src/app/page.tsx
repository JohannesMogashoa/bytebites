import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import RecipeList from "@/components/client/RecipeList";
import { Suspense } from "react";
import { env } from "@/env";

export default async function HomePage() {
  const response = await fetch(`${env.BACKEND_API_URL}/api/recipes`);
  const recipes = (await response.json()) as IRecipeListItem[];

  return (
    <main className="container mx-auto p-4">
      <h1 className="mb-4 text-2xl font-bold">Recent Invoices</h1>

      <Suspense fallback={<div>Loading...</div>}>
        <RecipeList recipes={Promise.resolve(recipes)} />
      </Suspense>
    </main>
  );
}
