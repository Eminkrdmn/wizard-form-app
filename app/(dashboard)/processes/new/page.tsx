"use client";

import { useState } from "react";
import { useSearchParams } from "next/navigation";
import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import { validateForm, type FormErrors } from "@/lib/validation";
import DynamicForm from "@/components/form-renderer/DynamicForm";
import Button from "@/components/ui/Button";

export default function NewProcessPage() {
  const searchParams = useSearchParams();
  const formId = searchParams.get("formId");

  const form = useFormStore((s) => s.forms.find((f) => f.id === formId));

  const [values, setValues] = useState<Record<string, string | boolean>>({});
  const [errors, setErrors] = useState<FormErrors>({});

  function handleChange(fieldId: string, value: string | boolean) {
    setValues((prev) => ({ ...prev, [fieldId]: value }));
  }

  function handleSubmit() {
    if (!form) return;

    const validationErrors = validateForm(form, values);
    setErrors(validationErrors);

    if (Object.keys(validationErrors).length > 0) return;

    console.log("Form geçerli, değerler:", values);
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
        <DynamicForm
          form={form}
          values={values}
          errors={errors}
          onChange={handleChange}
        />

        <Button onClick={handleSubmit}>Süreci Başlat</Button>
      </div>
    </div>
  );
}