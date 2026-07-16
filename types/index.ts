export type Role = "admin" | "user";

export type User = {
  id: string;
  username: string;
  role: Role;
};

export type FieldType = "input" | "number" | "select" | "checkbox" | "date";

export type FormField = {
  id: string;
  label: string;
  type: FieldType;
  required: boolean;
  options?: string[];
  dependsOn?: { fieldId: string; value: string };
};

export type FormDefinition = {
  id: string;
  name: string;
  fields: FormField[];
  createdAt: string;
};