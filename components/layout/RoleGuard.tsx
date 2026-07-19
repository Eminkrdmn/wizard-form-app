"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuthStore } from "@/stores/authStore";
import type { Role } from "@/types";

type RoleGuardProps = {
  allowed: Role[];
  children: React.ReactNode;
};

export default function RoleGuard({ allowed, children }: RoleGuardProps) {
  const router = useRouter();
  const role = useAuthStore((s) => s.user?.role);

  const permitted = role !== undefined && allowed.includes(role);

  useEffect(() => {
    if (role !== undefined && !permitted) {
      router.replace("/dashboard");
    }
  }, [role, permitted, router]);

  if (!permitted) {
    return null;
  }

  return <>{children}</>;
}
