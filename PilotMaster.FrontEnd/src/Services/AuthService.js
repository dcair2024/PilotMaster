// /src/services/AuthService.js

// Removido import axios; usando fetch por enquanto

// 1. A URL base que aponta para https://localhost:7031/api/auth/login
const API_BASE_URL = 'https://localhost:7031/api';

/**
 * Envia credenciais para o backend (/auth/login) e armazena o Access Token.
 * @param {string} email
 * @param {string} password
 * @returns {object} Dados da resposta da API.
 */
export const login = async (email, password) => {
    try {
        // O código de fetch começa diretamente aqui (sem o segundo 'export const login')
        const response = await fetch(`${API_BASE_URL}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        });

        const data = await response.json();

        if (response.ok) {
            // ⭐️ 1. ARMAZENAMENTO DO TOKEN ⭐️
            const { token } = data; 
            localStorage.setItem('pilotmaster_token', token); // Chave correta para guardar
            
            return { success: true, token };
        } else {
            throw new Error(data.message || 'Falha na autenticação. Verifique suas credenciais.');
        }

    } catch (error) {
        // Erro de rede (servidor fora do ar, por exemplo)
        console.error("Erro ao tentar login:", error);
        throw new Error('Não foi possível conectar ao servidor.');
    }
}; // Fim da função login

// Funções utilitárias
export const logout = () => {
    // A chave usada para armazenar é 'pilotmaster_token', então deve ser removida
    localStorage.removeItem('pilotmaster_token'); 
};

export const getAuthToken = () => {
    return localStorage.getItem('pilotmaster_token');
};