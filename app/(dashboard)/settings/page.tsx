"use client";

import { useAuthStore } from "@/stores/authStore";
import {
  useSettingsStore,
  type Theme,
  type Language,
} from "@/stores/settingsStore";
import { useTranslation } from "react-i18next";
import Select from "@/components/ui/Select";

const THEME_LABELS: Record<Theme, string> = { light: "Açık", dark: "Koyu" };
const LANGUAGE_LABELS: Record<Language, string> = {
  tr: "Türkçe",
  en: "English",
};

export default function SettingsPage() {
  const user = useAuthStore((s) => s.user);
  const theme = useSettingsStore((s) => s.theme);
  const language = useSettingsStore((s) => s.language);
  const setTheme = useSettingsStore((s) => s.setTheme);
  const setLanguage = useSettingsStore((s) => s.setLanguage);
  const { t } = useTranslation();

  return (
    <div className="max-w-md">
      <h1 className="mb-6 text-2xl font-bold">{t("settings.title")}</h1>

      <div className="mb-6 rounded-lg border border-line bg-card p-4">
        <h2 className="mb-3 font-semibold">{t("settings.userInfo")}</h2>
        <dl className="space-y-1 text-sm">
          <div className="flex gap-2">
            <dt className="text-ink-soft">{t("settings.username")}:</dt>
            <dd>{user?.username}</dd>
          </div>
          <div className="flex gap-2">
            <dt className="text-ink-soft">{t("settings.role")}:</dt>
            <dd>{user?.role}</dd>
          </div>
        </dl>
      </div>

      <div className="rounded-lg border border-line bg-card p-4">
        <h2 className="mb-3 font-semibold">{t("settings.preferences")}</h2>

        <Select
          label={t("settings.theme")}
          options={Object.values(THEME_LABELS)}
          value={THEME_LABELS[theme]}
          onChange={(e) => {
            const selected = (Object.keys(THEME_LABELS) as Theme[]).find(
              (k) => THEME_LABELS[k] === e.target.value,
            );
            if (selected) setTheme(selected);
          }}
        />

        <Select
          label={t("settings.language")}
          options={Object.values(LANGUAGE_LABELS)}
          value={LANGUAGE_LABELS[language]}
          onChange={(e) => {
            const selected = (Object.keys(LANGUAGE_LABELS) as Language[]).find(
              (k) => LANGUAGE_LABELS[k] === e.target.value,
            );
            if (selected) setLanguage(selected);
          }}
        />
      </div>
    </div>
  );
}
