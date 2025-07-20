import RecipeList from "components/client/RecipeList";
import { Suspense } from "react";
import { getRecipies } from "@/server/create-recipe";

export default async function HomePage() {
  const recipes = await getRecipies();

  return (
    <main className="container mx-auto p-4">
      <h1 className="mb-4 text-2xl font-bold">Recent Invoices</h1>

      <Suspense fallback={<div>Loading...</div>}>
        <RecipeList recipes={Promise.resolve(recipes)} />
      </Suspense>
    </main>
  );
}
