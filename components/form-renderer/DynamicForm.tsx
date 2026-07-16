"use client";

import type { FormDefinition } from "@/types";
import type { FormValues, FormErrors } from "@/lib/validation";
import Input from "@/components/ui/Input";
import Select from "@/components/ui/Select";
import Checkbox from "@/components/ui/Checkbox";

type DynamicFormProps = {
  form: FormDefinition;
  values: FormValues;
  errors?: FormErrors;
  onChange: (fieldId: string, value: string | boolean) => void;
};

export default function DynamicForm({
  form,
  values,
  errors = {},
  onChange,
}: DynamicFormProps) {
  return (
    <div>
      {form.fields.map((field) => {
        const value = values[field.id];
        const error = errors[field.id];

        switch (field.type) {
          case "select":
            return (
              <Select
                key={field.id}
                label={field.label + (field.required ? " *" : "")}
                options={field.options ?? []}
                placeholder="Seçiniz..."
                value={(value as string) ?? ""}
                error={error}
                onChange={(e) => onChange(field.id, e.target.value)}
              />
            );

          case "checkbox":
            return (
              <Checkbox
                key={field.id}
                label={field.label + (field.required ? " *" : "")}
                checked={(value as boolean) ?? false}
                error={error}
                onChange={(e) => onChange(field.id, e.target.checked)}
              />
            );

          default:
            return (
              <Input
                key={field.id}
                label={field.label + (field.required ? " *" : "")}
                type={field.type === "input" ? "text" : field.type}
                value={(value as string) ?? ""}
                error={error}
                onChange={(e) => onChange(field.id, e.target.value)}
              />
            );
        }
      })}
    </div>
  );
}