"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

const MENU_ITEMS = [
  { label: "Dashboard", href: "/dashboard" },
  { label: "Form Tasarımı", href: "/form-designer" },
  { label: "Süreçler / İşlerim", href: "/processes" },
  { label: "Ayarlar", href: "/settings" },
];

export default function LeftMenu() {
  const pathname = usePathname();

  return (
    <nav className="w-56 border-r bg-white p-4">
      <ul className="space-y-1">
        {MENU_ITEMS.map((item) => {
          const active = pathname.startsWith(item.href);
          return (
            <li key={item.href}>
              <Link
                href={item.href}
                className={`block rounded px-3 py-2 text-sm ${
                  active
                    ? "bg-blue-600 text-white"
                    : "text-gray-700 hover:bg-gray-100"
                }`}
              >
                {item.label}
              </Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
}