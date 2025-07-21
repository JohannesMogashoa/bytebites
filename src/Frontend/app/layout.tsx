"use client";

import "@/styles/globals.css";

import { Auth0Provider } from "@auth0/nextjs-auth0";
import NavBarComponent from "@/components/client/NavBarComponent";
import { RecipeStoreProvider } from "@/providers/recipes-store-provider";
import { useUser } from "@auth0/nextjs-auth0";

export default function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  const { user } = useUser();
  return (
    <html lang="en">
      <body className="min-h-screen w-screen">
        <Auth0Provider>
          <RecipeStoreProvider>
            <main className="container mx-auto p-4">
              <NavBarComponent user={user} />
              {children}
            </main>
          </RecipeStoreProvider>
        </Auth0Provider>
      </body>
    </html>
  );
}
