"use client";

import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import { useProcessStore } from "@/stores/processStore";
import { useTranslation } from "react-i18next";
import type { ProcessStatus } from "@/types";
import Button from "@/components/ui/Button";
import { STATUS_STYLES } from "@/lib/statusStyles";

export default function ProcessesPage() {
  const forms = useFormStore((s) => s.forms);
  const processes = useProcessStore((s) => s.processes);
  const { t } = useTranslation();

  return (
    <div>
      <h1 className="mb-6 text-2xl font-bold">{t("processes.title")}</h1>

      <h2 className="mb-3 font-semibold">{t("processes.selectForm")}</h2>

      {forms.length === 0 && (
        <p className="text-sm text-ink-soft">{t("processes.noForms")}</p>
      )}

      <ul className="max-w-lg space-y-2">
        {forms.map((form) => (
          <li
            key={form.id}
            className="flex items-center justify-between rounded-lg border border-line bg-card px-4 py-3"
          >
            <div>
              <p className="font-medium">{form.name}</p>
              <p className="text-sm text-ink-soft">
                {form.fields.length} {t("processes.fields")} •{" "}
                {new Date(form.createdAt).toLocaleDateString("tr-TR")}
              </p>
            </div>
            <Link href={`/processes/new?formId=${form.id}`}>
              <Button>{t("processes.start")}</Button>
            </Link>
          </li>
        ))}
      </ul>

      <h2 className="mb-3 mt-8 font-semibold">{t("processes.started")}</h2>

      {processes.length === 0 && (
        <p className="text-sm text-ink-soft">{t("processes.noProcesses")}</p>
      )}

      <ul className="max-w-lg space-y-2">
        {[...processes].reverse().map((process) => (
          <li key={process.id}>
            <Link
              href={`/processes/${process.id}`}
              className="flex items-center justify-between rounded-lg border border-line bg-card px-4 py-3 hover:bg-page"
            >
              <div>
                <p className="font-medium">{process.formName}</p>
                <p className="text-sm text-ink-soft">
                  {new Date(process.createdAt).toLocaleString("tr-TR")}
                </p>
              </div>
              <span
                className={`rounded-full px-3 py-1 text-sm font-medium ${STATUS_STYLES[process.status]}`}
              >
                {process.status}
              </span>
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}
