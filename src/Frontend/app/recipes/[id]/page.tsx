import { Card, CardContent, CardHeader } from "@/components/ui/card";

import { Clock } from "lucide-react";
import React from "react";
import { cleanUpInstructions } from "@/lib/utils";
import { getRecipe } from "@/server/create-recipe";

const RecipeDetailPage = async ({
  params,
}: {
  params: Promise<{ id: string }>;
}) => {
  const { id } = await params;
  const recipe = await getRecipe(id);

  return (
    <main>
      <h1 className="text-2xl font-extrabold">{recipe.title}</h1>
      <p className="text-muted-foreground">{recipe.description}</p>

      <div className="mt-5 flex w-full space-x-5">
        <Card>
          <CardHeader className="text-lg font-semibold">
            <h3>Ingredients</h3>
          </CardHeader>
          <CardContent>
            <ul className="list-disc pl-5">
              {recipe.ingredients.split(",").map((ingredient, index) => (
                <li key={index}>{ingredient}</li>
              ))}
            </ul>
          </CardContent>
        </Card>
        <Card>
          <CardHeader className="text-lg font-semibold">
            <h2>Instructions</h2>
          </CardHeader>
          <CardContent>
            <ul className="list-decimal pl-5">
              {cleanUpInstructions(recipe.steps).map((instruction, index) => (
                <li key={index}>{instruction.trim()}</li>
              ))}
            </ul>
          </CardContent>
        </Card>
      </div>

      <div className="mt-5 mb-5 flex space-x-3">
        <Clock />
        <p>{recipe.cookingTime} minutes</p>
      </div>

      <div>
        {recipe.dietaryTags.split(",").map((tag, index) => (
          <span
            key={index}
            className="mr-2 inline-block rounded bg-blue-100 px-2 py-1 text-sm text-blue-800"
          >
            {tag.trim()}
          </span>
        ))}
      </div>
    </main>
  );
};

export default RecipeDetailPage;
