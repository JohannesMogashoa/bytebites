"use server";

import type { CreateRecipeDTO, UpdateRecipeDTO } from "lib/recipe-schemas";

import type { IRecipeDetail } from "../lib/interfaces/IRecipeDetail";
import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import { env } from "env";

const baseUrl = `${env.BACKEND_API_URL}/api/recipes`;

export async function createRecipe(
  data: CreateRecipeDTO,
): Promise<IRecipeDetail> {
  const response = await fetch(baseUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      //   Authorization: `Bearer ${token.token}`,
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error("Failed to create recipe");
  }

  return (await response.json()) as IRecipeDetail;
}

export async function updateRecipe(
  id: string,
  data: UpdateRecipeDTO,
): Promise<boolean> {
  const response = await fetch(`${baseUrl}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error("Failed to update recipe");
  }

  return response.ok; // Return true if the update was successful
}

export async function deleteRecipe(id: string): Promise<void> {
  const response = await fetch(`${baseUrl}/${id}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("Failed to delete recipe");
  }
}

export async function getRecipies(): Promise<IRecipeListItem[]> {
  const response = await fetch(baseUrl);

  if (!response.ok) {
    throw new Error("Failed to fetch recipes");
  }

  return (await response.json()) as IRecipeListItem[];
}

export async function getRecipe(id: string): Promise<IRecipeDetail> {
  const response = await fetch(`${baseUrl}/${id}`);

  if (!response.ok) {
    throw new Error("Failed to fetch recipe");
  }

  return (await response.json()) as IRecipeDetail;
}
