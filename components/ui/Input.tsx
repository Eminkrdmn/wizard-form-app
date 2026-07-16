import type { InputHTMLAttributes } from "react";

type InputProps = InputHTMLAttributes<HTMLInputElement> & {
  label: string;
  error?: string;
};

export default function Input({ label, error, ...rest }: InputProps) {
  return (
    <div className="mb-4">
      <label className="mb-1 block text-sm text-gray-600">{label}</label>
      <input
        className={`w-full rounded border px-3 py-2 ${
          error ? "border-red-500" : "border-gray-300"
        }`}
        {...rest}
      />
      {error && <p className="mt-1 text-sm text-red-600">{error}</p>}
    </div>
  );
}