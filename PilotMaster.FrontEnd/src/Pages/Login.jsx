// src/Services/AuthService.js
import { API_BASE_URL } from "../utils.js";

export async function login(email, password, twoFactorCode = null, twoFactorRecoveryCode = null) {
  const url = `${API_BASE_URL}/auth/login`;

  const body = {
    email,
    password,
    twoFactorCode,
    twoFactorRecoveryCode
  };

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Accept": "application/json"
    },
    body: JSON.stringify(body)
  });

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Login falhou: ${res.status} ${text}`);
  }

  const data = await res.json();

  const token = data?.accessToken ?? data?.token;
  const refreshToken = data?.refreshToken ?? null;

  if (!token) throw new Error("Token não recebido do servidor.");

  localStorage.setItem("accessToken", token);
  if (refreshToken) localStorage.setItem("refreshToken", refreshToken);

  if (data?.userRole) {
    localStorage.setItem("userRole", data.userRole);
  }

  return data;
}
