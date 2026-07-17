import { NextResponse } from "next/server";
import type { ProcessInstance } from "@/types";

const processes: ProcessInstance[] = [];

export async function GET() {
  await new Promise((r) => setTimeout(r, 400));
  return NextResponse.json({ processes });
}

export async function POST(request: Request) {
  const body = await request.json();

  await new Promise((r) => setTimeout(r, 800));

  if (!body.formId || !body.data) {
    return NextResponse.json(
      { message: "formId ve form verisi zorunludur" },
      { status: 400 }
    );
  }

  const process: ProcessInstance = {
    id: crypto.randomUUID(),
    formId: body.formId,
    formName: body.formName,
    data: body.data,
    status: "Beklemede",
    createdAt: new Date().toISOString(),
    history: [
      {
        action: "Süreç başlatıldı",
        by: body.by ?? "bilinmiyor",
        at: new Date().toISOString(),
      },
    ],
  };

  processes.push(process);
  return NextResponse.json({ process }, { status: 201 });
}