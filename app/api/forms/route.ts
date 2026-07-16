import { NextResponse } from "next/server";
import type { FormDefinition } from "@/types";

const forms: FormDefinition[] = [];

export async function GET() {
  await new Promise((r) => setTimeout(r, 400));
  return NextResponse.json({ forms });
}

export async function POST(request: Request) {
  const form: FormDefinition = await request.json();

  await new Promise((r) => setTimeout(r, 800));

  if (!form.name?.trim() || !form.fields?.length) {
    return NextResponse.json(
      { message: "Form adı ve en az bir alan zorunludur" },
      { status: 400 }
    );
  }

  forms.push(form);
  return NextResponse.json({ form }, { status: 201 });
}