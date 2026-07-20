"use client";

import { useEffect } from "react";
import "@/lib/i18n";
import i18n from "@/lib/i18n";
import { useSettingsStore } from "@/stores/settingsStore";

export default function I18nProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const language = useSettingsStore((s) => s.language);

  useEffect(() => {
    i18n.changeLanguage(language);
  }, [language]);

  return <>{children}</>;
}
