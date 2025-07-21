"use client";

import { Card, CardContent, CardHeader } from "../ui/card";

import { Button } from "../ui/button";
import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import Link from "next/link";
import React from "react";
import moment from "moment";

const RecipeListItemComponent = ({ recipe }: { recipe: IRecipeListItem }) => {
  return (
    <Card>
      <CardHeader className="flex items-center justify-between">
        <div className="text-lg font-semibold">{recipe.title}</div>
        <div className="text-muted-foreground text-sm">{recipe.createdBy}</div>
      </CardHeader>
      <CardContent>
        <div className="text-muted-foreground text-sm">
          {recipe.description}
        </div>
        <div className="flex items-center justify-between">
          <div className="text-muted-foreground mt-2 text-xs">
            {moment(recipe.createdAt).fromNow()}
          </div>
          <Button>
            <Link href={`/recipes/${recipe.id}`} className="text-white">
              View Recipe
            </Link>
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default RecipeListItemComponent;
