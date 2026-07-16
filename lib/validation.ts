import type { FormDefinition } from "@/types";

export type FormValues = Record<string, string | boolean>;
export type FormErrors = Record<string, string>;

export function validateForm(form: FormDefinition, values: FormValues): FormErrors {
  const errors: FormErrors = {};

  for (const field of form.fields) {
    const raw = values[field.id];
    const value = typeof raw === "string" ? raw.trim() : raw;

    // 1) Zorunlu mu? (sabit zorunluluk VEYA bağımlılık tetiklendi)
    const dependencyTriggered =
      field.dependsOn !== undefined &&
      values[field.dependsOn.fieldId] === field.dependsOn.value;

    const isRequired = field.required || dependencyTriggered;

    const isEmpty =
      value === undefined || value === "" || value === false;

    if (isRequired && isEmpty) {
      errors[field.id] = dependencyTriggered
        ? "Bu alan, yaptığınız seçim nedeniyle zorunludur"
        : "Bu alan zorunludur";
      continue;
    }

    // 2) Tip bazlı kontroller (alan doluysa)
    if (!isEmpty && typeof value === "string") {
      if (field.type === "number" && isNaN(Number(value))) {
        errors[field.id] = "Geçerli bir sayı giriniz";
        continue;
      }
      if (field.type === "date" && isNaN(new Date(value).getTime())) {
        errors[field.id] = "Geçerli bir tarih giriniz";
        continue;
      }
    }
  }

  return errors;
}