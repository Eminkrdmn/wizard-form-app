import type { ButtonHTMLAttributes } from "react";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary" | "danger";
};

const STYLES = {
  primary: "bg-blue-600 text-white hover:bg-blue-700",
  secondary: "border border-line hover:bg-page",
  danger: "bg-red-600 text-white hover:bg-red-700",
};

export default function Button({
  variant = "primary",
  className = "",
  ...rest
}: ButtonProps) {
  return (
    <button
      className={`rounded px-4 py-2 text-sm disabled:opacity-50 ${STYLES[variant]} ${className}`}
      {...rest}
    />
  );
}
