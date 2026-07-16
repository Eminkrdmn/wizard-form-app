import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { FormDefinition } from "@/types";

type FormState = {
  forms: FormDefinition[];
  addForm: (form: FormDefinition) => void;
  removeForm: (id: string) => void;
};

export const useFormStore = create<FormState>()(
  persist(
    (set) => ({
      forms: [],
      addForm: (form) => set((state) => ({ forms: [...state.forms, form] })),
      removeForm: (id) =>
        set((state) => ({ forms: state.forms.filter((f) => f.id !== id) })),
    }),
    { name: "form-storage" }
  )
);