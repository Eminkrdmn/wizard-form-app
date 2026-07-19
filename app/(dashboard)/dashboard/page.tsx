"use client";

import Link from "next/link";
import { useAuthStore } from "@/stores/authStore";
import { useProcessStore } from "@/stores/processStore";
import type { ProcessInstance, ProcessStatus } from "@/types";

const COLUMNS: { title: string; statuses: ProcessStatus[]; style: string }[] = [
  {
    title: "Bekleyen İşler",
    statuses: ["Beklemede"],
    style: "border-yellow-400",
  },
  {
    title: "Devam Eden İşler",
    statuses: ["DevamEdiyor"],
    style: "border-blue-400",
  },
  {
    title: "Tamamlanan İşler",
    statuses: ["Tamamlandi", "Reddedildi"],
    style: "border-green-400",
  },
];

export default function DashboardPage() {
  const user = useAuthStore((s) => s.user);
  const processes = useProcessStore((s) => s.processes);

  return (
    <div>
      <h1 className="text-2xl font-bold">Dashboard</h1>
      <p className="mt-2 text-ink-soft">
        Hoş geldin, {user?.username} ({user?.role})
      </p>

      <div className="mt-6 grid gap-4 md:grid-cols-3">
        {COLUMNS.map((col) => {
          const items = processes.filter((p) =>
            col.statuses.includes(p.status),
          );
          return (
            <div
              key={col.title}
              className={`rounded-lg border-t-4 bg-card p-4 ${col.style}`}
            >
              <h2 className="mb-3 font-semibold">
                {col.title}{" "}
                <span className="text-sm font-normal text-ink-soft">
                  ({items.length})
                </span>
              </h2>

              {items.length === 0 && (
                <p className="text-sm text-ink-soft">Kayıt yok.</p>
              )}

              <ul className="space-y-2">
                {items.map((p: ProcessInstance) => (
                  <li key={p.id}>
                    <Link
                      href={`/processes/${p.id}`}
                      className="block rounded border border-line px-3 py-2 text-sm hover:bg-page"
                    >
                      <p className="font-medium">{p.formName}</p>
                      <p className="text-xs text-ink-soft">
                        {new Date(p.createdAt).toLocaleString("tr-TR")}
                        {p.status === "Reddedildi" && " • reddedildi"}
                      </p>
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          );
        })}
      </div>
    </div>
  );
}
