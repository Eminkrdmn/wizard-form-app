import type { SelectHTMLAttributes } from "react";

type SelectProps = SelectHTMLAttributes<HTMLSelectElement> & {
  label: string;
  options: string[];
  error?: string;
  placeholder?: string;
};

export default function Select({
  label,
  options,
  error,
  placeholder,
  ...rest
}: SelectProps) {
  return (
    <div className="mb-4">
      <label className="mb-1 block text-sm text-ink-soft">{label}</label>
      <select
        className={`w-full rounded border bg-field px-3 py-2 ${
          error ? "border-red-500" : "border-line"
        }`}
        {...rest}
      >
        {placeholder && <option value="">{placeholder}</option>}
        {options.map((opt) => (
          <option key={opt} value={opt}>
            {opt}
          </option>
        ))}
      </select>
      {error && <p className="mt-1 text-sm text-red-500">{error}</p>}
    </div>
  );
}
