import type { ProcessStatus } from "@/types";

export const STATUS_STYLES: Record<ProcessStatus, string> = {
  Beklemede: "bg-yellow-100 text-yellow-800",
  DevamEdiyor: "bg-blue-100 text-blue-800",
  Tamamlandi: "bg-green-100 text-green-800",
  Reddedildi: "bg-red-100 text-red-800",
};
