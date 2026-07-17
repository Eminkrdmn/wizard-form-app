"use client";

import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import { useProcessStore } from "@/stores/processStore";
import type { ProcessStatus } from "@/types";
import Button from "@/components/ui/Button";

const STATUS_STYLES: Record<ProcessStatus, string> = {
  Beklemede: "bg-yellow-100 text-yellow-800",
  DevamEdiyor: "bg-blue-100 text-blue-800",
  Tamamlandi: "bg-green-100 text-green-800",
  Reddedildi: "bg-red-100 text-red-800",
};

export default function ProcessesPage() {
  const forms = useFormStore((s) => s.forms);
  const processes = useProcessStore((s) => s.processes);

  return (
    <div>
      <h1 className="mb-6 text-2xl font-bold">Süreçler / İşlerim</h1>

      <h2 className="mb-3 font-semibold">Form Seç ve Süreç Başlat</h2>

      {forms.length === 0 && (
        <p className="text-sm text-gray-400">
          Henüz kayıtlı form yok. Önce Form Tasarımı ekranından bir form oluştur.
        </p>
      )}

      <ul className="max-w-lg space-y-2">
        {forms.map((form) => (
          <li
            key={form.id}
            className="flex items-center justify-between rounded-lg border bg-white px-4 py-3"
          >
            <div>
              <p className="font-medium">{form.name}</p>
              <p className="text-sm text-gray-400">
                {form.fields.length} alan •{" "}
                {new Date(form.createdAt).toLocaleDateString("tr-TR")}
              </p>
            </div>
            <Link href={`/processes/new?formId=${form.id}`}>
              <Button>Süreç Başlat</Button>
            </Link>
          </li>
        ))}
      </ul>

      <h2 className="mb-3 mt-8 font-semibold">Başlatılan Süreçler</h2>

      {processes.length === 0 && (
        <p className="text-sm text-gray-400">Henüz başlatılmış süreç yok.</p>
      )}

      <ul className="max-w-lg space-y-2">
        {[...processes].reverse().map((process) => (
          <li key={process.id}>
            <Link
              href={`/processes/${process.id}`}
              className="flex items-center justify-between rounded-lg border bg-white px-4 py-3 hover:bg-gray-50"
            >
              <div>
                <p className="font-medium">{process.formName}</p>
                <p className="text-sm text-gray-400">
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