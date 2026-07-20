"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useAuthStore } from "@/stores/authStore";
import { useTranslation } from "react-i18next";

export default function LoginPage() {
  const router = useRouter();
  const setUser = useAuthStore((s) => s.setUser);
  const { t } = useTranslation();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const res = await fetch("/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password }),
      });

      const data = await res.json();

      if (!res.ok) {
        setError(data.message);
        return;
      }

      setUser(data.user);
      router.push("/dashboard");
    } catch {
      setError(t("login.serverError"));
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="flex min-h-screen items-center justify-center bg-page">
      <form
        onSubmit={handleSubmit}
        className="w-full max-w-sm rounded-lg border border-line bg-card p-8 shadow"
      >
        <h1 className="mb-6 text-2xl font-bold">{t("login.title")}</h1>

        <label className="mb-1 block text-sm text-ink-soft">
          {t("login.username")}
        </label>
        <input
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="mb-4 w-full rounded border border-line bg-field px-3 py-2"
          required
        />

        <label className="mb-1 block text-sm text-ink-soft">
          {t("login.password")}
        </label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="mb-4 w-full rounded border border-line bg-field px-3 py-2"
          required
        />

        {error && <p className="mb-4 text-sm text-red-500">{error}</p>}

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded bg-blue-600 py-2 text-white hover:bg-blue-700 disabled:opacity-50"
        >
          {loading ? t("login.loading") : t("login.submit")}
        </button>
      </form>
    </main>
  );
}
