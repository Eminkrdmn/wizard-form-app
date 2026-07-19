"use client";

import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
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
      className="flex items-center justify-between rounded border bg-white px-3 py-2 text-sm"
    >
      <div className="flex items-center gap-2">
        <button
          {...attributes}
          {...listeners}
          className="cursor-grab text-gray-400 hover:text-gray-600"
          aria-label="Sürükle"
        >
          ⠿
        </button>
        <span>
          {field.label}{" "}
          <span className="text-gray-400">
            ({field.type}){field.required && " • zorunlu"}
            {field.dependsOn && " • bağımlı"}
          </span>
        </span>
      </div>
      <Button variant="danger" onClick={() => onRemove(field.id)}>
        Sil
      </Button>
    </li>
  );
}
