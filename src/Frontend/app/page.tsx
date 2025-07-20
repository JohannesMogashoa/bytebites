import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";

import { Button } from "@/components/ui/button";
import FilterRecipes from "@/components/client/FilterRecipes";
import RecipeList from "components/client/RecipeList";
import { Suspense } from "react";
import { auth0 } from "@/lib/auth0";
import { getRecipies } from "@/server/create-recipe";

export default async function HomePage() {
  const recipes = await getRecipies();
  const session = await auth0.getSession();

  return (
    <main className="container mx-auto p-4">
      <div className="flex justify-between">
        <h1 className="mb-4 text-2xl font-bold">Byte Bites Recipes</h1>
        {session !== null ? (
          <div className="flex space-x-3">
            <Avatar>
              <h2>{session.user.name}</h2>
            </Avatar>

            <Button>
              <a href="/auth/logout" className="ml-4">
                Logout
              </a>
            </Button>
          </div>
        ) : (
          <div>
            <a href="/auth/login">Login</a>
          </div>
        )}
      </div>

      <FilterRecipes />

      <Suspense fallback={<div>Loading...</div>}>
        <RecipeList recipes={Promise.resolve(recipes)} />
      </Suspense>
    </main>
  );
}
