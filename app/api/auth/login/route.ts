import { NextResponse } from "next/server";

const USERS = [
  { id: "1", username: "admin", password: "admin", role: "admin" },
  { id: "2", username: "user", password: "user", role: "user" },
];

export async function POST(request: Request) {
  const { username, password } = await request.json();

  // gerçek backend gecikmesi simülasyonu
  await new Promise((r) => setTimeout(r, 800));

  const found = USERS.find(
    (u) => u.username === username && u.password === password
  );

  if (!found) {
    return NextResponse.json(
      { message: "Kullanıcı adı veya şifre hatalı" },
      { status: 401 }
    );
  }

  const { password: _, ...user } = found;
  return NextResponse.json({ user });
}