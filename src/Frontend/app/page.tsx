import RecipeList from "components/client/RecipeList";
import { Suspense } from "react";
import { auth0 } from "@/lib/auth0";
import { getRecipies } from "@/server/create-recipe";

export default async function HomePage() {
  const recipes = await getRecipies();
  const session = await auth0.getSession();

  return (
    <main className="container mx-auto p-4">
      <h1 className="mb-4 text-2xl font-bold">Recent Invoices</h1>
      {session !== null ? (
        <div>
          <h2>{session.user.name}</h2>
          <a href="/auth/logout">Logout</a>
        </div>
      ) : (
        <div>
          <a href="/auth/login">Login</a>
        </div>
      )}

      <Suspense fallback={<div>Loading...</div>}>
        <RecipeList recipes={Promise.resolve(recipes)} />
      </Suspense>
    </main>
  );
}
