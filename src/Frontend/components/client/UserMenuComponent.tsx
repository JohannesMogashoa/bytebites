import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

import Link from "next/link";
import React from "react";
import type { User } from "@auth0/nextjs-auth0/types";

const UserMenuComponent = ({ user }: { user: User | null | undefined }) => {
  return (
    <div>
      {user === null || user === undefined ? (
        <div>
          <a href="/auth/login">Login</a>
        </div>
      ) : (
        <div className="flex space-x-3">
          <DropdownMenu>
            <DropdownMenuTrigger>{user.name}</DropdownMenuTrigger>
            <DropdownMenuContent className="w-56" align="start">
              <DropdownMenuLabel>My Account</DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuItem>
                <Link href={"/recipes"}>My Recipes</Link>
              </DropdownMenuItem>
              <DropdownMenuItem>
                <a href="/auth/logout" className="w-full">
                  Logout
                </a>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      )}
    </div>
  );
};

export default UserMenuComponent;
