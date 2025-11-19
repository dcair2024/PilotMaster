// Salvar token
export const saveAuthToken = (token) => {
  localStorage.setItem("pilotmaster_token", token);
};

// Ler token
export const getAuthToken = () => {
  return localStorage.getItem("pilotmaster_token");
};

// Remover token e deslogar
export const logout = () => {
  localStorage.removeItem("pilotmaster_token");
  window.location.href = "/"; // volta pro login
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

  if (!response.ok) {
    throw new Error("Usuário ou senha inválidos.");
  }

  const data = await response.json();
  saveAuthToken(data.token);
  return data;
};
