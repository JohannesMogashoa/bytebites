"use client";

import React from "react";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "components/ui/form";
import { Button } from "components/ui/button";
import { Input } from "components/ui/input";
import { useForm } from "react-hook-form";
import { type z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { FilterRecipesSchema } from "@/lib/recipe-schemas";
import { filterRecipes } from "@/server/create-recipe";
import { useRecipeStore } from "@/providers/recipes-store-provider";

import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerTrigger,
} from "@/components/ui/drawer";

const FilterRecipes = () => {
  const { setRecipes } = useRecipeStore((state) => state);

  const form = useForm<z.infer<typeof FilterRecipesSchema>>({
    resolver: zodResolver(FilterRecipesSchema),
    defaultValues: {
      title: undefined,
      ingredients: undefined,
      cookingTime: undefined,
      dietaryTags: undefined,
    },
  });

  async function onSubmit(values: z.infer<typeof FilterRecipesSchema>) {
    try {
      const data = await filterRecipes(values);
      setRecipes(data);
    } catch (error) {
      // Handle error (show toast, etc.)
      console.error(error);
    }
  }

  return (
    <Drawer>
      <DrawerTrigger>Open</DrawerTrigger>
      <DrawerContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
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
            <DrawerClose asChild>
              <Button variant="outline">Cancel</Button>
            </DrawerClose>
          </form>
        </Form>
      </DrawerContent>
    </Drawer>
  );
};

export default FilterRecipes;
