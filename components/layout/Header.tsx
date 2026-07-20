"use client";

import { useRouter } from "next/navigation";
import { useAuthStore } from "@/stores/authStore";
import { useTranslation } from "react-i18next";

export default function Header() {
  const router = useRouter();
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);
  const { t } = useTranslation();

  function handleLogout() {
    logout();
    router.replace("/login");
  }

  return (
    <header className="flex h-14 items-center justify-between border-b border-line bg-card px-6">
      <h1 className="text-lg font-bold">Wizard Form App</h1>

      <div className="flex items-center gap-4">
        <span className="text-sm text-ink-soft">
          {user?.username} <span>({user?.role})</span>
        </span>
        <button
          onClick={handleLogout}
          className="rounded border border-line px-3 py-1 text-sm hover:bg-page"
        >
          {t("header.logout")}
        </button>
      </div>
    </header>
  );
}
