"use client";

import { useRouter } from "next/navigation";
import { useAuthStore } from "@/stores/authStore";

export default function Header() {
  const router = useRouter();
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);

  function handleLogout() {
    logout();
    router.replace("/login");
  }

  return (
    <header className="flex h-14 items-center justify-between border-b bg-white px-6">
      <h1 className="text-lg font-bold text-gray-800">Wizard Form App</h1>

      <div className="flex items-center gap-4">
        <span className="text-sm text-gray-600">
          {user?.username} <span className="text-gray-400">({user?.role})</span>
        </span>
        <button
          onClick={handleLogout}
          className="rounded border px-3 py-1 text-sm text-gray-700 hover:bg-gray-100"
        >
          Çıkış
        </button>
      </div>
    </header>
  );
}