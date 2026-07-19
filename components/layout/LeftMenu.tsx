"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAuthStore } from "@/stores/authStore";
import type { Role } from "@/types";

type MenuItem = {
  label: string;
  href: string;
  roles: Role[];
};

const MENU_ITEMS: MenuItem[] = [
  { label: "Dashboard", href: "/dashboard", roles: ["admin", "user"] },
  { label: "Form Tasarımı", href: "/form-designer", roles: ["admin"] },
  { label: "Süreçler / İşlerim", href: "/processes", roles: ["admin", "user"] },
  { label: "Ayarlar", href: "/settings", roles: ["admin", "user"] },
];

export default function LeftMenu() {
  const pathname = usePathname();
  const role = useAuthStore((s) => s.user?.role);

  const visibleItems = MENU_ITEMS.filter(
    (item) => role && item.roles.includes(role),
  );

  return (
    <nav className="w-56 border-r border-line bg-card p-4">
      <ul className="space-y-1">
        {visibleItems.map((item) => {
          const active = pathname.startsWith(item.href);
          return (
            <li key={item.href}>
              <Link
                href={item.href}
                className={`block rounded px-3 py-2 text-sm ${
                  active ? "bg-blue-600 text-white" : "hover:bg-page"
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
