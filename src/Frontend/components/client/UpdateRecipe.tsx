"use client";

import { Dialog, DialogContent, DialogTitle } from "components/ui/dialog";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "components/ui/form";
import React, { useState } from "react";

import { Button } from "../ui/button";
import type { IRecipeDetail } from "@/lib/interfaces/IRecipeDetail";
import { Input } from "components/ui/input";
import { UpdateRecipeSchema } from "lib/recipe-schemas";
import { updateRecipe } from "server/create-recipe";
import { useForm } from "react-hook-form";
import { useRecipeStore } from "@/providers/recipes-store-provider";
import type z from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

export const UpdateRecipe = ({
  open,
  close,
  recipe,
}: {
  open: boolean;
  close: () => void;
  recipe: IRecipeDetail;
}) => {
  const form = useForm<z.infer<typeof UpdateRecipeSchema>>({
    resolver: zodResolver(UpdateRecipeSchema),
    defaultValues: {
      id: recipe.id,
      title: recipe.title,
      description: recipe.description,
      ingredients: recipe.ingredients,
      steps: recipe.steps,
      cookingTime: recipe.cookingTime,
      dietaryTags: recipe.dietaryTags,
    },
  });

  async function onSubmit(values: z.infer<typeof UpdateRecipeSchema>) {
    try {
      await updateRecipe(values);
      // Optionally close the dialog and reset the form
      onClose();
    } catch (error) {
      // Handle error (show toast, etc.)
      console.error(error);
    }
  }

  const onClose = () => {
    form.reset();
    close();
  };

  return (
    <Dialog open={open} onOpenChange={onClose}>
      <DialogContent aria-describedby={undefined}>
        <DialogTitle>Update Recipe</DialogTitle>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="id"
              render={({ field }) => (
                <FormItem>
                  <FormControl>
                    <Input type="hidden" {...field} />
                  </FormControl>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="title"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Title</FormLabel>
                  <FormControl>
                    <Input placeholder="Yummy" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description</FormLabel>
                  <FormControl>
                    <Input type="text" placeholder="Yummy yum yum" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="ingredients"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Ingredients</FormLabel>
                  <FormControl>
                    <Input placeholder="Yum bits..." {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="steps"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Steps</FormLabel>
                  <FormControl>
                    <Input placeholder="Yum 1..." {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="cookingTime"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Cooking Time</FormLabel>
                  <FormControl>
                    <Input type="number" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="dietaryTags"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Dietary Tags</FormLabel>
                  <FormControl>
                    <Input placeholder="Yummy" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button type="submit">Submit</Button>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};

export const UpdateRecipeButton = ({ recipe }: { recipe: IRecipeDetail }) => {
  const [open, setOpen] = useState<boolean>(false);

  const close = () => setOpen(false);

  const openDialog = () => {
    setOpen(true);
  };

  return (
    <>
      <Button variant="outline" onClick={openDialog}>
        Update Recipe
      </Button>
      <UpdateRecipe open={open} close={close} recipe={recipe} />
    </>
  );
};
