// src/services/fetchAuthenticated.js
import { API_BASE_URL } from "../utils";

export async function fetchAuthenticated(path, options = {}) {
  // path deve começar com '/' EX: '/dashboard' ou '/manobras/recentes'
  const token = localStorage.getItem("accessToken");
  if (!token) throw new Error("No token stored");

  const url = `${API_BASE_URL}${path.startsWith("/") ? path : `/${path}`}`;

  const res = await fetch(url, {
    ...options,
    headers: {
      ...(options.headers || {}),
      "Accept": "application/json",
      "Content-Type": options?.body ? "application/json" : undefined,
      "Authorization": `Bearer ${token}`
    }
  });

  if (res.status === 401) {
    // token inválido/expirado — trate refresh token aqui se tiver
    throw new Error("Unauthorized");
  }

  if (!res.ok) {
    const txt = await res.text();
    throw new Error(`Request failed ${res.status}: ${txt}`);
  }

  const contentType = res.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    return res.json();
  } else {
    const text = await res.text();
    try { return JSON.parse(text); } catch { return text; }
  }
}
