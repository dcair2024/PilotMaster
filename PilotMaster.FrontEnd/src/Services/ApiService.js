import { getToken, logout } from "./AuthService";

const API_BASE_URL = "https://localhost:7031/api";

export const fetchAuthenticated = async (endpoint, options = {}) => {
  const token = getToken();

  const headers = {
    "Content-Type": "application/json",
    ...(options.headers || {}),
  };

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers,
  });

  // Se o token vencer ou for inválido
  if (response.status === 401) {
    logout();
    throw new Error("Sessão expirada. Faça login novamente.");
  }

  // Erros gerais
  if (!response.ok) {
    const text = await response.text();
    const msg = text || response.statusText || "Erro ao buscar dados.";
    throw new Error(msg);
  }

  return response.json();
};
