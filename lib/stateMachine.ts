import type { ProcessStatus } from "@/types";

export const TRANSITIONS: Record<ProcessStatus, ProcessStatus[]> = {
  Beklemede: ["DevamEdiyor", "Reddedildi"],
  DevamEdiyor: ["Tamamlandi", "Reddedildi"],
  Tamamlandi: [],
  Reddedildi: [],
};

export const ACTION_LABELS: Record<ProcessStatus, string> = {
  Beklemede: "Beklemeye Al",
  DevamEdiyor: "İşleme Al",
  Tamamlandi: "Tamamla",
  Reddedildi: "Reddet",
};

export function canTransition(from: ProcessStatus, to: ProcessStatus): boolean {
  return TRANSITIONS[from].includes(to);
}
