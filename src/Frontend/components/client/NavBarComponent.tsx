"use client";

import Link from "next/link";
import React from "react";
import type { User } from "@auth0/nextjs-auth0/types";
import UserMenuComponent from "./UserMenuComponent";

const NavBarComponent = ({ user }: { user: User | null | undefined }) => {
  return (
    <div className="mb-10 flex items-center justify-between">
      <h1>
        <Link href="/" className="text-2xl font-bold">
          Byte Bites
        </Link>
      </h1>
      <UserMenuComponent user={user} />
    </div>
  );
};

export default NavBarComponent;
