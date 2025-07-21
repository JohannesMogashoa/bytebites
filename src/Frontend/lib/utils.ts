import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const cleanUpInstructions = (instructions: string): string[] => {
  return instructions
    .split(/(?:\d+\.\s)/) // Split by numbers followed by a period and space
    .filter((step) => step.trim() !== "") // Remove empty strings
    .map((step) => step.trim()); // Trim whitespace from each step
};
