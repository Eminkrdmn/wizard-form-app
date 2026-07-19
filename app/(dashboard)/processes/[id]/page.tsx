"use client";

import { use } from "react";
import Link from "next/link";
import { useProcessStore } from "@/stores/processStore";
import { useAuthStore } from "@/stores/authStore";
import { useFormStore } from "@/stores/formStore";
import { TRANSITIONS, ACTION_LABELS } from "@/lib/stateMachine";
import type { ProcessStatus } from "@/types";
import Button from "@/components/ui/Button";

const STATUS_STYLES: Record<ProcessStatus, string> = {
  Beklemede: "bg-yellow-100 text-yellow-800",
  DevamEdiyor: "bg-blue-100 text-blue-800",
  Tamamlandi: "bg-green-100 text-green-800",
  Reddedildi: "bg-red-100 text-red-800",
};

export default function ProcessDetailPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = use(params);

  const process = useProcessStore((s) => s.processes.find((p) => p.id === id));
  const updateStatus = useProcessStore((s) => s.updateStatus);
  const user = useAuthStore((s) => s.user);
  const form = useFormStore((s) =>
    s.forms.find((f) => f.id === process?.formId),
  );

  if (!process) {
    return (
      <div>
        <p className="text-ink-soft">Süreç bulunamadı.</p>
        <Link href="/processes" className="text-blue-500 underline">
          Süreçler sayfasına dön
        </Link>
      </div>
    );
  }

  const nextStatuses = TRANSITIONS[process.status];

  function fieldLabel(fieldId: string): string {
    return form?.fields.find((f) => f.id === fieldId)?.label ?? fieldId;
  }

  return (
    <div className="max-w-2xl">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-2xl font-bold">{process.formName}</h1>
        <span
          className={`rounded-full px-3 py-1 text-sm font-medium ${STATUS_STYLES[process.status]}`}
        >
          {process.status}
        </span>
      </div>

      {/* Süreç bilgileri */}
      <div className="mb-6 rounded-lg border border-line bg-card p-4">
        <h2 className="mb-3 font-semibold">Süreç Bilgileri</h2>
        <dl className="space-y-1 text-sm">
          <div className="flex gap-2">
            <dt className="text-ink-soft">Süreç ID:</dt>
            <dd className="font-mono">{process.id}</dd>
          </div>
          <div className="flex gap-2">
            <dt className="text-ink-soft">Başlangıç:</dt>
            <dd>{new Date(process.createdAt).toLocaleString("tr-TR")}</dd>
          </div>
          {process.completedAt && (
            <div className="flex gap-2">
              <dt className="text-ink-soft">Bitiş:</dt>
              <dd>{new Date(process.completedAt).toLocaleString("tr-TR")}</dd>
            </div>
          )}
        </dl>
      </div>

      {/* Form verileri */}
      <div className="mb-6 rounded-lg border border-line bg-card p-4">
        <h2 className="mb-3 font-semibold">Form Verileri</h2>
        <dl className="space-y-1 text-sm">
          {Object.entries(process.data).map(([fieldId, value]) => (
            <div key={fieldId} className="flex gap-2">
              <dt className="text-ink-soft">{fieldLabel(fieldId)}:</dt>
              <dd>{String(value)}</dd>
            </div>
          ))}
        </dl>

        <details className="mt-3">
          <summary className="cursor-pointer text-sm text-blue-500">
            JSON çıktısını göster
          </summary>
          <pre className="mt-2 overflow-auto rounded bg-gray-950 p-3 text-xs text-green-400">
            {JSON.stringify(process.data, null, 2)}
          </pre>
        </details>
      </div>

      {/* Aksiyonlar */}
      {nextStatuses.length > 0 && (
        <div className="mb-6 rounded-lg border border-line bg-card p-4">
          <h2 className="mb-3 font-semibold">Aksiyonlar</h2>
          <div className="flex gap-2">
            {nextStatuses.map((status) => (
              <Button
                key={status}
                variant={status === "Reddedildi" ? "danger" : "primary"}
                onClick={() =>
                  updateStatus(
                    process.id,
                    status,
                    user?.username ?? "bilinmiyor",
                  )
                }
              >
                {ACTION_LABELS[status]}
              </Button>
            ))}
          </div>
        </div>
      )}

      {/* Geçmiş */}
      <div className="rounded-lg border border-line bg-card p-4">
        <h2 className="mb-3 font-semibold">Süreç Geçmişi</h2>
        <ul className="space-y-2 text-sm">
          {process.history.map((h, i) => (
            <li key={i} className="flex gap-2 text-ink-soft">
              <span>{new Date(h.at).toLocaleString("tr-TR")}</span>
              <span className="font-medium text-ink">{h.by}</span>
              <span>→ {h.action}</span>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
