import { CreateRecipeButton } from "@/components/client/CreateRecipe";
import MyRecipeDetailsComponent from "@/components/client/MyRecipeDetailsComponent";
import React from "react";
import { auth0 } from "@/lib/auth0";
import { getUserRecipes } from "@/server/create-recipe";

const MyRecipesPage = async () => {
  const session = await auth0.getSession();

  if (!session) {
    throw new Error("You are not authorized!");
  }

  const recipes = await getUserRecipes(session.user.sub);

  return (
    <>
      <div className="flex w-full items-center justify-between">
        <h1 className="text-2xl font-extrabold">My Recipes</h1>
        <CreateRecipeButton />
      </div>
      <div className="mt-5">
        {recipes.length === 0 ? (
          <p className="text-muted-foreground">You have no recipes yet.</p>
        ) : (
          <MyRecipeDetailsComponent recipies={recipes} />
        )}
      </div>
    </>
  );
};

export default MyRecipesPage;
