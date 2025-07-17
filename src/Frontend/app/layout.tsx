"use client";

import "@/styles/globals.css";
import { Auth0Provider } from "@auth0/nextjs-auth0";
import { type Metadata } from "next";
import { Geist } from "next/font/google";

// export const metadata: Metadata = {
//   title: "Byte Bites",
//   description: "Your one stop shop for all the best recipe bites",
//   icons: [{ rel: "icon", url: "/favicon.ico" }],
// };

const geist = Geist({
  subsets: ["latin"],
  variable: "--font-geist-sans",
});

export default function RootLayout({
  children,
}: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="en" className={`${geist.variable}`}>
      <body>
        <Auth0Provider>{children}</Auth0Provider>
      </body>
    </html>
  );
}
