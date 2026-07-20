"use client";

import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { useTranslation } from "react-i18next";
import type { FormField } from "@/types";
import Button from "@/components/ui/Button";

type SortableFieldItemProps = {
  field: FormField;
  onRemove: (id: string) => void;
};

export default function SortableFieldItem({
  field,
  onRemove,
}: SortableFieldItemProps) {
  const { t } = useTranslation();
  const { attributes, listeners, setNodeRef, transform, transition } =
    useSortable({ id: field.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  };

  return (
    <li
      ref={setNodeRef}
      style={style}
      className="flex items-center justify-between rounded border border-line bg-card px-3 py-2 text-sm"
    >
      <div className="flex items-center gap-2">
        <button
          {...attributes}
          {...listeners}
          className="cursor-grab text-ink-soft hover:text-ink"
          aria-label="drag"
        >
          ⠿
        </button>
        <span>
          {field.label}{" "}
          <span className="text-ink-soft">
            ({field.type}){field.required && ` • ${t("designer.requiredTag")}`}
            {field.dependsOn && ` • ${t("designer.dependentTag")}`}
          </span>
        </span>
      </div>
      <Button variant="danger" onClick={() => onRemove(field.id)}>
        {t("designer.delete")}
      </Button>
    </li>
  );
}
