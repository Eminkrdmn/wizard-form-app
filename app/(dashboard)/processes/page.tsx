"use client";

import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import Button from "@/components/ui/Button";

export default function ProcessesPage() {
  const forms = useFormStore((s) => s.forms);

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
    </div>
  );
}