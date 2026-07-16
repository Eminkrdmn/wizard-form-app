"use client";

import { useState } from "react";
import { useFormStore } from "@/stores/formStore";
import type { FieldType, FormField } from "@/types";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Checkbox from "@/components/ui/Checkbox";
import Button from "@/components/ui/Button";

const FIELD_TYPES: FieldType[] = ["input", "number", "select", "checkbox", "date"];

export default function FormDesignerPage() {
  const addForm = useFormStore((s) => s.addForm);

  // form geneli
  const [formName, setFormName] = useState("");
  const [fields, setFields] = useState<FormField[]>([]);

  // alan ekleme paneli
  const [label, setLabel] = useState("");
  const [type, setType] = useState<FieldType>("input");
  const [required, setRequired] = useState(false);

  function handleAddField() {
    if (!label.trim()) return;

    const newField: FormField = {
      id: crypto.randomUUID(),
      label: label.trim(),
      type,
      required,
    };

    setFields((prev) => [...prev, newField]);
    setLabel("");
    setRequired(false);
  }

  function handleRemoveField(id: string) {
    setFields((prev) => prev.filter((f) => f.id !== id));
  }

  function handleSaveForm() {
    if (!formName.trim() || fields.length === 0) return;

    addForm({
      id: crypto.randomUUID(),
      name: formName.trim(),
      fields,
      createdAt: new Date().toISOString(),
    });

    setFormName("");
    setFields([]);
  }

  return (
    <div>
      <h1 className="mb-6 text-2xl font-bold">Form Tasarımı</h1>

      <div className="mb-6 max-w-md">
        <Input
          label="Form Adı"
          value={formName}
          onChange={(e) => setFormName(e.target.value)}
          placeholder="Örn: İzin Talebi"
        />
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        {/* Sol: alan ekleme paneli */}
        <div className="rounded-lg border bg-white p-4">
          <h2 className="mb-4 font-semibold">Alan Ekle</h2>

          <Input
            label="Etiket"
            value={label}
            onChange={(e) => setLabel(e.target.value)}
            placeholder="Örn: Ad Soyad"
          />

          <Select
            label="Tip"
            options={FIELD_TYPES}
            value={type}
            onChange={(e) => setType(e.target.value as FieldType)}
          />

          <Checkbox
            label="Zorunlu alan"
            checked={required}
            onChange={(e) => setRequired(e.target.checked)}
          />

          <Button onClick={handleAddField}>Alan Ekle</Button>
        </div>

        {/* Sağ: eklenen alanlar */}
        <div className="rounded-lg border bg-white p-4">
          <h2 className="mb-4 font-semibold">Eklenen Alanlar</h2>

          {fields.length === 0 && (
            <p className="text-sm text-gray-400">Henüz alan eklenmedi.</p>
          )}

          <ul className="space-y-2">
            {fields.map((f) => (
              <li
                key={f.id}
                className="flex items-center justify-between rounded border px-3 py-2 text-sm"
              >
                <span>
                  {f.label}{" "}
                  <span className="text-gray-400">
                    ({f.type}){f.required && " • zorunlu"}
                  </span>
                </span>
                <Button variant="danger" onClick={() => handleRemoveField(f.id)}>
                  Sil
                </Button>
              </li>
            ))}
          </ul>

          {fields.length > 0 && (
            <div className="mt-4">
              <Button onClick={handleSaveForm}>Formu Kaydet</Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}