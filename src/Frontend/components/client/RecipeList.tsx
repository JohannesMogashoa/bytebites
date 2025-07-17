"use client";

import { Card, CardContent, CardHeader } from "../ui/card";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "components/ui/table";
import { use, useState } from "react";

import { Button } from "components/ui/button";
import CreateRecipe from "./CreateRecipe";
import type { IRecipeListItem } from "lib/interfaces/IRecipeListItem";
import UpdateRecipe from "./UpdateRecipe";
import moment from "moment";

type RecipeListProps = {
  recipes: Promise<IRecipeListItem[]>;
};

const RecipeList = ({ recipes }: RecipeListProps) => {
  const [open, setOpen] = useState(false);
  const [openEdit, setEditOpen] = useState(false);
  const [recipe, setRecipe] = useState<IRecipeListItem | null>(null);
  const allRecipes = use(recipes);

  const clearRecipe = () => {
    setRecipe(null);
    setEditOpen(false);
  };

  const deleteRecipe = async (id: string) => {
    try {
      await deleteRecipe(id);
      // Optionally, you can refresh the recipe list or show a success message
    } catch (error) {
      console.error("Failed to delete recipe:", error);
    }
  };

  return (
    <Card>
      <CardHeader className="flex items-center justify-end">
        <Button onClick={() => setOpen(true)} className="mb-4">
          Create Recipe
        </Button>
      </CardHeader>
      <CardContent>
        <Table>
          <TableCaption>A list of your recent invoices.</TableCaption>
          <TableHeader>
            <TableRow>
              <TableHead className="w-[100px]">Title</TableHead>
              <TableHead>Decription</TableHead>
              <TableHead>Created By</TableHead>
              <TableHead className="text-right">Created At</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {allRecipes.map((recipe) => (
              <TableRow key={recipe.id}>
                <TableCell className="font-medium">{recipe.title}</TableCell>
                <TableCell>{recipe.description}</TableCell>
                <TableCell>{recipe.createdBy}</TableCell>
                <TableCell className="text-right">
                  {moment(recipe.createdAt).fromNow()}
                </TableCell>
                <TableCell className="text-right">
                  <Button
                    variant="destructive"
                    size="sm"
                    onClick={() => deleteRecipe(recipe.id)}
                  >
                    Delete
                  </Button>
                </TableCell>
                <TableCell className="text-right">
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => {
                      setRecipe(recipe);
                      setEditOpen(true);
                    }}
                  >
                    Edit
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>

      <CreateRecipe open={open} setOpen={setOpen} />
      {recipe && (
        <UpdateRecipe open={openEdit} close={clearRecipe} id={recipe.id} />
      )}
    </Card>
  );
};

export default RecipeList;
