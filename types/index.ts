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

export type ProcessStatus = "Beklemede" | "DevamEdiyor" | "Tamamlandi" | "Reddedildi";

export type ProcessAction = {
  action: string;
  by: string;
  at: string;
};

export type ProcessInstance = {
  id: string;
  formId: string;
  formName: string;
  data: Record<string, string | boolean>;
  status: ProcessStatus;
  createdAt: string;
  completedAt?: string;
  history: ProcessAction[];
};
