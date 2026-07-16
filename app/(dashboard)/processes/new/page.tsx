"use client";

import { useState } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import DynamicForm from "@/components/form-renderer/DynamicForm";
import Button from "@/components/ui/Button";

export default function NewProcessPage() {
  const searchParams = useSearchParams();
  const formId = searchParams.get("formId");

  const form = useFormStore((s) => s.forms.find((f) => f.id === formId));

  const [values, setValues] = useState<Record<string, string | boolean>>({});

  function handleChange(fieldId: string, value: string | boolean) {
    setValues((prev) => ({ ...prev, [fieldId]: value }));
  }

  if (!form) {
    return (
      <div>
        <p className="text-gray-500">Form bulunamadı.</p>
        <Link href="/processes" className="text-blue-600 underline">
          Süreçler sayfasına dön
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-lg">
      <h1 className="mb-6 text-2xl font-bold">{form.name}</h1>

      <div className="rounded-lg border bg-white p-6">
        <DynamicForm form={form} values={values} onChange={handleChange} />

        <Button onClick={() => console.log("Form değerleri:", values)}>
          Süreci Başlat
        </Button>
      </div>
    </div>
  );
}