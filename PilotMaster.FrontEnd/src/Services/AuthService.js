const API_BASE_URL = "https://localhost:7031/api";

// Nome da chave usada para guardar o token no navegador
const TOKEN_KEY = 'pilotmaster_token';

/**
 * Realiza login na API e armazena o token JWT.
 * @param {string} email
 * @param {string} senha
 * @returns {Promise<void>}
 */
export const login = async (email, senha) => {
    try {
        const response = await fetch(`${API_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ 
                email, 
                password: senha 
            }),
        });

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            const errorMessage = errorData.message || 'Falha no login.';
            throw new Error(errorMessage);
        }

        const data = await response.json();

        // compatibilidade entre diferentes nomes de retorno
        const token = data.token || data.accessToken;

        if (!token) {
            throw new Error('Token não recebido do servidor.');
        }

        localStorage.setItem(TOKEN_KEY, token);

    } catch (error) {
        console.error('Erro no processo de login:', error.message);
        throw error;
    }
};

/**
 * Remove o token do localStorage (logout).
 */
export const logout = () => {
    localStorage.removeItem(TOKEN_KEY);
};

/**
 * Informa se o usuário está autenticado.
 * @returns {boolean}
 */
export const isAuthenticated = () => {
    return !!localStorage.getItem(TOKEN_KEY);
};

/**
 * Retorna o token armazenado no navegador.
 * @returns {string|null}
 */
export const getToken = () => {
    return localStorage.getItem(TOKEN_KEY);
};

