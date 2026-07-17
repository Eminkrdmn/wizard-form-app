"use client";

import { useState } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import Link from "next/link";
import { useFormStore } from "@/stores/formStore";
import { useProcessStore } from "@/stores/processStore";
import { useAuthStore } from "@/stores/authStore";
import { validateForm, type FormErrors } from "@/lib/validation";
import DynamicForm from "@/components/form-renderer/DynamicForm";
import Button from "@/components/ui/Button";

export default function NewProcessPage() {
  const searchParams = useSearchParams();
  const router = useRouter();
  const formId = searchParams.get("formId");

  const form = useFormStore((s) => s.forms.find((f) => f.id === formId));
  const addProcess = useProcessStore((s) => s.addProcess);
  const user = useAuthStore((s) => s.user);

  const [values, setValues] = useState<Record<string, string | boolean>>({});
  const [errors, setErrors] = useState<FormErrors>({});
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState("");

  function handleChange(fieldId: string, value: string | boolean) {
    setValues((prev) => ({ ...prev, [fieldId]: value }));
  }

  async function handleSubmit() {
    if (!form) return;

    const validationErrors = validateForm(form, values);
    setErrors(validationErrors);
    if (Object.keys(validationErrors).length > 0) return;

    setSubmitting(true);
    setSubmitError("");

    try {
      const res = await fetch("/api/processes", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          formId: form.id,
          formName: form.name,
          data: values,
          by: user?.username,
        }),
      });

      const data = await res.json();

      if (!res.ok) {
        setSubmitError(data.message);
        return;
      }

      addProcess(data.process);
      router.push(`/processes/${data.process.id}`);
    } catch {
      setSubmitError("Sunucuya ulaşılamadı");
    } finally {
      setSubmitting(false);
    }
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

        <Button onClick={handleSubmit} disabled={submitting}>
          {submitting ? "Başlatılıyor..." : "Süreci Başlat"}
        </Button>

        {submitError && (
          <p className="mt-3 text-sm text-red-600">{submitError}</p>
        )}
      </div>
    </div>
  );
}