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

  // select seçenekleri
  const [options, setOptions] = useState<string[]>([]);
  const [optionDraft, setOptionDraft] = useState("");

  // bağımlı zorunluluk
  const [dependsOnFieldId, setDependsOnFieldId] = useState("");
  const [dependsOnValue, setDependsOnValue] = useState("");

  // kaydetme durumu
  const [saving, setSaving] = useState(false);
  const [saveMessage, setSaveMessage] = useState<{
    type: "success" | "error";
    text: string;
  } | null>(null);

  function handleAddOption() {
    const value = optionDraft.trim();
    if (!value || options.includes(value)) return;
    setOptions((prev) => [...prev, value]);
    setOptionDraft("");
  }

  function handleRemoveOption(opt: string) {
    setOptions((prev) => prev.filter((o) => o !== opt));
  }

  function handleAddField() {
    if (!label.trim()) return;
    if (type === "select" && options.length === 0) return;

    const newField: FormField = {
      id: crypto.randomUUID(),
      label: label.trim(),
      type,
      required,
      ...(type === "select" && { options }),
      ...(dependsOnFieldId &&
        dependsOnValue && {
          dependsOn: { fieldId: dependsOnFieldId, value: dependsOnValue },
        }),
    };

    setFields((prev) => [...prev, newField]);
    setLabel("");
    setRequired(false);
    setOptions([]);
    setOptionDraft("");
    setDependsOnFieldId("");
    setDependsOnValue("");
  }

  function handleRemoveField(id: string) {
    setFields((prev) => prev.filter((f) => f.id !== id));
  }

  async function handleSaveForm() {
    if (!formName.trim() || fields.length === 0) return;

    setSaving(true);
    setSaveMessage(null);

    const form = {
      id: crypto.randomUUID(),
      name: formName.trim(),
      fields,
      createdAt: new Date().toISOString(),
    };

    try {
      const res = await fetch("/api/forms", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form),
      });

      const data = await res.json();

      if (!res.ok) {
        setSaveMessage({ type: "error", text: data.message });
        return;
      }

      addForm(data.form);
      setFormName("");
      setFields([]);
      setSaveMessage({ type: "success", text: "Form kaydedildi ✓" });
    } catch {
      setSaveMessage({ type: "error", text: "Sunucuya ulaşılamadı" });
    } finally {
      setSaving(false);
    }
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

          {type === "select" && (
            <div className="mb-4 rounded border bg-gray-50 p-3">
              <p className="mb-2 text-sm font-medium text-gray-600">Seçenekler</p>

              <div className="flex gap-2">
                <input
                  value={optionDraft}
                  onChange={(e) => setOptionDraft(e.target.value)}
                  placeholder="Örn: Yıllık İzin"
                  className="w-full rounded border px-3 py-2 text-sm"
                />
                <Button variant="secondary" onClick={handleAddOption}>
                  Ekle
                </Button>
              </div>

              <ul className="mt-2 space-y-1">
                {options.map((opt) => (
                  <li
                    key={opt}
                    className="flex items-center justify-between rounded bg-white px-3 py-1 text-sm"
                  >
                    {opt}
                    <button
                      onClick={() => handleRemoveOption(opt)}
                      className="text-red-500 hover:text-red-700"
                    >
                      ✕
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          )}

          {fields.some((f) => f.type === "select") && (
            <div className="mb-4 rounded border bg-gray-50 p-3">
              <p className="mb-2 text-sm font-medium text-gray-600">
                Bağımlı Zorunluluk (opsiyonel)
              </p>

              <Select
                label="Şu alana bağlı"
                options={fields
                  .filter((f) => f.type === "select")
                  .map((f) => f.label)}
                placeholder="Bağımlılık yok"
                value={fields.find((f) => f.id === dependsOnFieldId)?.label ?? ""}
                onChange={(e) => {
                  const selected = fields.find((f) => f.label === e.target.value);
                  setDependsOnFieldId(selected?.id ?? "");
                  setDependsOnValue("");
                }}
              />

              {dependsOnFieldId && (
                <Select
                  label="Şu değer seçilirse zorunlu olur"
                  options={
                    fields.find((f) => f.id === dependsOnFieldId)?.options ?? []
                  }
                  placeholder="Değer seçin"
                  value={dependsOnValue}
                  onChange={(e) => setDependsOnValue(e.target.value)}
                />
              )}
            </div>
          )}

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
                    {f.dependsOn && " • bağımlı"}
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
              <Button onClick={handleSaveForm} disabled={saving}>
                {saving ? "Kaydediliyor..." : "Formu Kaydet"}
              </Button>
            </div>
          )}

          {saveMessage && (
            <p
              className={`mt-3 text-sm ${
                saveMessage.type === "success"
                  ? "text-green-600"
                  : "text-red-600"
              }`}
            >
              {saveMessage.text}
            </p>
          )}
        </div>
      </div>
    </div>
  );
}