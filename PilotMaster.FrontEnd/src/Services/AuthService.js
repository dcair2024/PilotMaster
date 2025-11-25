// Salvar token
export const saveAuthToken = (token) => {
  localStorage.setItem("pilotmaster_token", token);
};

// Ler token
export const getAuthToken = () => {
  return localStorage.getItem("pilotmaster_token");
};

// Remover token e redirecionar
export const logout = () => {
  localStorage.removeItem("pilotmaster_token");
  window.location.href = "/"; // volta para login
};

// Fazer login
export const login = async (email, password) => {
  const response = await fetch("https://localhost:7031/api/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ email, password })
  });

  // Se der erro, já retorna mensagem adequada
  if (!response.ok) {
    const msg = await response.text();
    throw new Error(msg || "Usuário ou senha inválidos.");
  }

  const data = await response.json();

  // Salva o token
  if (data.token) {
    saveAuthToken(data.token);
  } else {
    throw new Error("Token não recebido do servidor.");
  }

  return data;
};
