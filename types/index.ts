export type Role = "admin" | "user";

export type User = {
  id: string;
  username: string;
  role: Role;
};