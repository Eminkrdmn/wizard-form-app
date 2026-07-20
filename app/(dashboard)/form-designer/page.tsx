"use client";

import { useState } from "react";
import { useFormStore } from "@/stores/formStore";
import { useTranslation } from "react-i18next";
import type { FieldType, FormField } from "@/types";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Checkbox from "@/components/ui/Checkbox";
import Button from "@/components/ui/Button";
import RoleGuard from "@/components/layout/RoleGuard";
import { DndContext, closestCenter, type DragEndEvent } from "@dnd-kit/core";
import {
  SortableContext,
  verticalListSortingStrategy,
  arrayMove,
} from "@dnd-kit/sortable";
import SortableFieldItem from "@/components/form-builder/SortableFieldItem";

const FIELD_TYPES: FieldType[] = [
  "input",
  "number",
  "select",
  "checkbox",
  "date",
];

export default function FormDesignerPage() {
  const addForm = useFormStore((s) => s.addForm);
  const { t } = useTranslation();

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

  function handleDragEnd(event: DragEndEvent) {
    const { active, over } = event;
    if (!over || active.id === over.id) return;

    setFields((prev) => {
      const oldIndex = prev.findIndex((f) => f.id === active.id);
      const newIndex = prev.findIndex((f) => f.id === over.id);
      return arrayMove(prev, oldIndex, newIndex);
    });
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
      setSaveMessage({ type: "success", text: t("designer.saved") });
    } catch {
      setSaveMessage({ type: "error", text: t("designer.serverError") });
    } finally {
      setSaving(false);
    }
  }

  return (
    <RoleGuard allowed={["admin"]}>
      <div>
        <h1 className="mb-6 text-2xl font-bold">{t("designer.title")}</h1>

        <div className="mb-6 max-w-md">
          <Input
            label={t("designer.formName")}
            value={formName}
            onChange={(e) => setFormName(e.target.value)}
            placeholder={t("designer.formNamePlaceholder")}
          />
        </div>

        <div className="grid gap-6 md:grid-cols-2">
          {/* Sol: alan ekleme paneli */}
          <div className="rounded-lg border border-line bg-card p-4">
            <h2 className="mb-4 font-semibold">{t("designer.addField")}</h2>

            <Input
              label={t("designer.label")}
              value={label}
              onChange={(e) => setLabel(e.target.value)}
              placeholder={t("designer.labelPlaceholder")}
            />

            <Select
              label={t("designer.type")}
              options={FIELD_TYPES}
              value={type}
              onChange={(e) => setType(e.target.value as FieldType)}
            />

            {type === "select" && (
              <div className="mb-4 rounded border border-line bg-page p-3">
                <p className="mb-2 text-sm font-medium text-ink-soft">
                  {t("designer.options")}
                </p>

                <div className="flex gap-2">
                  <input
                    value={optionDraft}
                    onChange={(e) => setOptionDraft(e.target.value)}
                    placeholder={t("designer.optionPlaceholder")}
                    className="w-full rounded border border-line bg-field px-3 py-2 text-sm"
                  />
                  <Button variant="secondary" onClick={handleAddOption}>
                    {t("designer.add")}
                  </Button>
                </div>

                <ul className="mt-2 space-y-1">
                  {options.map((opt) => (
                    <li
                      key={opt}
                      className="flex items-center justify-between rounded bg-card px-3 py-1 text-sm"
                    >
                      {opt}
                      <button
                        onClick={() => handleRemoveOption(opt)}
                        className="text-red-500 hover:text-red-400"
                      >
                        ✕
                      </button>
                    </li>
                  ))}
                </ul>
              </div>
            )}

            {fields.some((f) => f.type === "select") && (
              <div className="mb-4 rounded border border-line bg-page p-3">
                <p className="mb-2 text-sm font-medium text-ink-soft">
                  {t("designer.dependency")}
                </p>

                <Select
                  label={t("designer.dependsOnField")}
                  options={fields
                    .filter((f) => f.type === "select")
                    .map((f) => f.label)}
                  placeholder={t("designer.noDependency")}
                  value={
                    fields.find((f) => f.id === dependsOnFieldId)?.label ?? ""
                  }
                  onChange={(e) => {
                    const selected = fields.find(
                      (f) => f.label === e.target.value,
                    );
                    setDependsOnFieldId(selected?.id ?? "");
                    setDependsOnValue("");
                  }}
                />

                {dependsOnFieldId && (
                  <Select
                    label={t("designer.dependsOnValue")}
                    options={
                      fields.find((f) => f.id === dependsOnFieldId)?.options ??
                      []
                    }
                    placeholder={t("designer.selectValue")}
                    value={dependsOnValue}
                    onChange={(e) => setDependsOnValue(e.target.value)}
                  />
                )}
              </div>
            )}

            <Checkbox
              label={t("designer.required")}
              checked={required}
              onChange={(e) => setRequired(e.target.checked)}
            />

            <Button onClick={handleAddField}>{t("designer.addField")}</Button>
          </div>

          {/* Sağ: eklenen alanlar */}
          <div className="rounded-lg border border-line bg-card p-4">
            <h2 className="mb-4 font-semibold">{t("designer.addedFields")}</h2>

            {fields.length === 0 && (
              <p className="text-sm text-ink-soft">{t("designer.noFields")}</p>
            )}

            <DndContext
              collisionDetection={closestCenter}
              onDragEnd={handleDragEnd}
            >
              <SortableContext
                items={fields.map((f) => f.id)}
                strategy={verticalListSortingStrategy}
              >
                <ul className="space-y-2">
                  {fields.map((f) => (
                    <SortableFieldItem
                      key={f.id}
                      field={f}
                      onRemove={handleRemoveField}
                    />
                  ))}
                </ul>
              </SortableContext>
            </DndContext>

            {fields.length > 0 && (
              <div className="mt-4">
                <Button onClick={handleSaveForm} disabled={saving}>
                  {saving ? t("designer.saving") : t("designer.save")}
                </Button>
              </div>
            )}

            {saveMessage && (
              <p
                className={`mt-3 text-sm ${
                  saveMessage.type === "success"
                    ? "text-green-500"
                    : "text-red-500"
                }`}
              >
                {saveMessage.text}
              </p>
            )}
          </div>
        </div>
      </div>
    </RoleGuard>
  );
}
