"use client";

import { Card, CardContent, CardHeader } from "../ui/card";

import { Button } from "../ui/button";
import type { IRecipeDetail } from "@/lib/interfaces/IRecipeDetail";
import React from "react";
import { UpdateRecipeButton } from "./UpdateRecipe";
import { deleteRecipeById } from "@/server/create-recipe";
import moment from "moment";

const MyRecipeDetailsComponent = ({
  recipies,
}: {
  recipies: IRecipeDetail[];
}) => {
  const deleteRecipe = async (recipeId: string) => {
    await deleteRecipeById(recipeId);
  };

  return (
    <>
      {recipies.map((recipe) => (
        <Card key={recipe.id} className="mb-5">
          <CardHeader className="flex items-center justify-between">
            <div className="text-lg font-semibold">{recipe.title}</div>
            <div className="text-muted-foreground text-sm">
              {recipe.createdBy}
            </div>
          </CardHeader>
          <CardContent>
            <div className="text-muted-foreground text-sm">
              {recipe.description}
            </div>
            <div className="flex items-center justify-between">
              <div className="text-muted-foreground mt-2 text-xs">
                {moment(recipe.createdAt).fromNow()}
              </div>
              <div className="space-x-3">
                <Button onClick={() => deleteRecipe(recipe.id)}>Delete</Button>
                <UpdateRecipeButton recipe={recipe} />
              </div>
            </div>
          </CardContent>
        </Card>
      ))}
    </>
  );
};

export default MyRecipeDetailsComponent;
