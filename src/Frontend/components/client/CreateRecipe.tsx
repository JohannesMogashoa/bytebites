"use client";

import { Dialog, DialogContent, DialogTitle } from "components/ui/dialog";
import { type Dispatch, type SetStateAction } from "react";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "components/ui/form";

import { Button } from "../ui/button";
import { CreateRecipeSchema } from "lib/recipe-schemas";
import { createRecipe } from "server/create-recipe";
import { Input } from "components/ui/input";
import { useForm } from "react-hook-form";
import { type z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

const CreateRecipe = ({
  open,
  setOpen,
}: {
  open: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
}) => {
  const form = useForm<z.infer<typeof CreateRecipeSchema>>({
    resolver: zodResolver(CreateRecipeSchema),
    defaultValues: {
      title: "",
      description: "",
      ingredients: "",
      steps: "",
      cookingTime: 0,
      dietaryTags: "",
    },
  });

  const onClose = () => {
    form.reset();
    setOpen(false);
  };

  // 2. Define a submit handler.
  async function onSubmit(values: z.infer<typeof CreateRecipeSchema>) {
    try {
      console.log("Submitting recipe:", values);
      await createRecipe(values);
      // Optionally close the dialog and reset the form
      onClose();
    } catch (error) {
      // Handle error (show toast, etc.)
      console.error(error);
    }
  }

  return (
    <div>
      <Dialog open={open} onOpenChange={onClose}>
        <DialogContent aria-describedby={undefined}>
          <DialogTitle>Create A Recipe</DialogTitle>
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
                name="description"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Description</FormLabel>
                    <FormControl>
                      <Input
                        type="text"
                        placeholder="Yummy yum yum"
                        {...field}
                      />
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
    </div>
  );
};

export default CreateRecipe;
