"use client";

import { useAuthStore } from "@/stores/authStore";

export default function DashboardPage() {
  const user = useAuthStore((s) => s.user);

  return (
    <main className="p-8">
      <h1 className="text-2xl font-bold">Dashboard</h1>
      <p className="mt-2 text-gray-600">
        Hoş geldin, {user?.username} ({user?.role})
      </p>
    </main>
  );
}