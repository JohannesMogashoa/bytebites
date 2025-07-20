"use server";

import type {
  CreateRecipeDTO,
  FilterRecipesDTO,
  UpdateRecipeDTO,
} from "lib/recipe-schemas";

import type { IRecipeDetail } from "../lib/interfaces/IRecipeDetail";
import type { IRecipeListItem } from "@/lib/interfaces/IRecipeListItem";
import { auth0 } from "@/lib/auth0";
import { env } from "env";

const baseUrl = `${env.BACKEND_API_URL}/api/recipes`;

export async function createRecipe(
  data: CreateRecipeDTO,
): Promise<IRecipeDetail> {
  const session = await auth0.getSession();

  if (session === null) {
    throw new Error("You are not authorized!");
  }

  const token = session.tokenSet.accessToken;

  const response = await fetch(baseUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
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
  const session = await auth0.getSession();

  if (session === null) {
    throw new Error("You are not authorized!");
  }

  const token = session.tokenSet.accessToken;
  const response = await fetch(`${baseUrl}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error("Failed to update recipe");
  }

  return response.ok; // Return true if the update was successful
}

export async function deleteRecipeById(id: string): Promise<boolean> {
  const session = await auth0.getSession();

  if (session === null) {
    throw new Error("You are not authorized!");
  }

  const token = session.tokenSet.accessToken;

  const response = await fetch(`${baseUrl}/${id}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    throw new Error("Failed to delete recipe");
  }

  return response.ok;
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

export async function filterRecipes(
  filters: FilterRecipesDTO,
): Promise<IRecipeListItem[]> {
  const session = await auth0.getSession();

  if (session === null) {
    throw new Error("You are not authorized!");
  }

  const token = session.tokenSet.accessToken;

  const response = await fetch(`${baseUrl}/filter`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(filters),
  });

  if (!response.ok) {
    throw new Error("Failed to filter recipes");
  }

  const data = (await response.json()) as IRecipeListItem[];

  console.log(data);

  return data;
}
