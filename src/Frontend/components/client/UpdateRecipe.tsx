import { UpdateRecipeSchema } from "lib/recipe-schemas";
import { getRecipe, updateRecipe } from "server/create-recipe";
import { zodResolver } from "@hookform/resolvers/zod";
import React, {
  useEffect,
  useState,
  type Dispatch,
  type SetStateAction,
} from "react";
import { useForm } from "react-hook-form";
import type z from "zod";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "components/ui/form";
import { Dialog, DialogContent, DialogTitle } from "components/ui/dialog";
import { Input } from "components/ui/input";
import { Button } from "../ui/button";
import SkeletonLoader from "./SkeletonLoader";

const UpdateRecipe = ({
  open,
  close,
  id,
}: {
  open: boolean;
  close: () => void;
  id: string;
}) => {
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getRecipe(id);

        form.reset({
          id: id,
          title: data.title,
          description: data.description,
          ingredients: data.ingredients,
          steps: data.steps,
          cookingTime: data.cookingTime ?? 0,
          dietaryTags: data.dietaryTags ?? "",
        });
        setIsLoading(false);
      } catch (err) {
        //setError(err);
      }
    };

    fetchData();
  }, [id]);

  const form = useForm<z.infer<typeof UpdateRecipeSchema>>({
    resolver: zodResolver(UpdateRecipeSchema),
  });

  const onClose = () => {
    form.reset();
    close();
    setIsLoading(true);
  };

  async function onSubmit(values: z.infer<typeof UpdateRecipeSchema>) {
    try {
      console.log("Submitting recipe:", values);
      await updateRecipe(id, values);
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
          <DialogTitle>Update Recipe</DialogTitle>
          {isLoading ? (
            <SkeletonLoader />
          ) : (
            <Form {...form}>
              <form
                onSubmit={form.handleSubmit(onSubmit)}
                className="space-y-8"
              >
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
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
};

export default UpdateRecipe;
