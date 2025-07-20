"use client";

import "@/styles/globals.css";

import { Auth0Provider } from "@auth0/nextjs-auth0";
import { RecipeStoreProvider } from "@/providers/recipes-store-provider";

export default function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="en">
      <body>
        <Auth0Provider>
          <RecipeStoreProvider>{children}</RecipeStoreProvider>
        </Auth0Provider>
      </body>
    </html>
  );
}
