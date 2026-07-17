import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { ProcessInstance, ProcessStatus } from "@/types";

type ProcessState = {
  processes: ProcessInstance[];
  addProcess: (process: ProcessInstance) => void;
  updateStatus: (id: string, status: ProcessStatus, by: string) => void;
};

export const useProcessStore = create<ProcessState>()(
  persist(
    (set) => ({
      processes: [],
      addProcess: (process) =>
        set((state) => ({ processes: [...state.processes, process] })),
      updateStatus: (id, status, by) =>
        set((state) => ({
          processes: state.processes.map((p) =>
            p.id === id
              ? {
                  ...p,
                  status,
                  ...(TERMINAL.includes(status) && {
                    completedAt: new Date().toISOString(),
                  }),
                  history: [
                    ...p.history,
                    {
                      action: status,
                      by,
                      at: new Date().toISOString(),
                    },
                  ],
                }
              : p
          ),
        })),
    }),
    { name: "process-storage" }
  )
);

const TERMINAL: ProcessStatus[] = ["Tamamlandi", "Reddedildi"];