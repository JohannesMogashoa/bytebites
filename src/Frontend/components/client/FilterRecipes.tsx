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

import {
  Drawer,
  DrawerClose,
  DrawerContent,
  DrawerDescription,
  DrawerFooter,
  DrawerHeader,
  DrawerTitle,
} from "@/components/ui/drawer";
import { FilterIcon, FilterXIcon } from "lucide-react";

const FilterRecipes = ({
  handleSubmit,
  resetFilters,
}: {
  handleSubmit: (values: z.infer<typeof FilterRecipesSchema>) => void;
  resetFilters: () => void;
}) => {
  const [open, setOpen] = React.useState(false);
  const form = useForm<z.infer<typeof FilterRecipesSchema>>({
    resolver: zodResolver(FilterRecipesSchema),
    defaultValues: {
      title: undefined,
      ingredients: undefined,
      cookingTime: undefined,
      dietaryTags: undefined,
    },
  });

  const onSubmit = (values: z.infer<typeof FilterRecipesSchema>) => {
    handleSubmit(values);
    setOpen(false);
  };

  return (
    <Drawer direction="right" open={open} onOpenChange={setOpen}>
      <div className="my-5 flex justify-end space-x-3">
        <Button
          variant={"outline"}
          onClick={resetFilters}
          className="cursor-pointer"
        >
          <FilterXIcon className="cursor-pointer"></FilterXIcon>
        </Button>
        <Button
          variant={"outline"}
          className="cursor-pointer"
          onClick={() => setOpen(true)}
        >
          <FilterIcon className="cursor-pointer" />
        </Button>
      </div>

      <DrawerContent>
        <div className="mx-auto w-full max-w-sm p-4">
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
              <DrawerHeader>
                <DrawerTitle>Filter Recipes</DrawerTitle>
                <DrawerDescription>
                  Looking for something specific? Use the filters below to
                  narrow the results.
                </DrawerDescription>
              </DrawerHeader>
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
              <DrawerFooter>
                <div className="flex justify-end space-x-5">
                  <DrawerClose>Cancel</DrawerClose>
                  <Button type="submit">Filter</Button>
                </div>
              </DrawerFooter>
            </form>
          </Form>
        </div>
      </DrawerContent>
    </Drawer>
  );
};

export default FilterRecipes;
