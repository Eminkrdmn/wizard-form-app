"use client";

import { useState } from "react";
import { useAuthStore } from "@/stores/authStore";
import Select from "@/components/ui/Select";

export default function SettingsPage() {
  const user = useAuthStore((s) => s.user);

  const [theme, setTheme] = useState("Açık");
  const [language, setLanguage] = useState("Türkçe");

  return (
    <div className="max-w-md">
      <h1 className="mb-6 text-2xl font-bold">Ayarlar</h1>

      <div className="mb-6 rounded-lg border bg-white p-4">
        <h2 className="mb-3 font-semibold">Kullanıcı Bilgileri</h2>
        <dl className="space-y-1 text-sm">
          <div className="flex gap-2">
            <dt className="text-gray-500">Kullanıcı adı:</dt>
            <dd>{user?.username}</dd>
          </div>
          <div className="flex gap-2">
            <dt className="text-gray-500">Rol:</dt>
            <dd>{user?.role}</dd>
          </div>
        </dl>
      </div>

      <div className="rounded-lg border bg-white p-4">
        <h2 className="mb-3 font-semibold">Tercihler</h2>

        <Select
          label="Tema"
          options={["Açık", "Koyu"]}
          value={theme}
          onChange={(e) => setTheme(e.target.value)}
        />

        <Select
          label="Dil"
          options={["Türkçe", "English"]}
          value={language}
          onChange={(e) => setLanguage(e.target.value)}
        />

        <p className="text-xs text-gray-400">
          Tema ve dil desteği yakında aktif olacak.
        </p>
      </div>
    </div>
  );
}