// src/services/ApiService.js

import { getAuthToken } from './AuthService'; // Reutiliza a função que você já tem

// URL base da API (a mesma usada no AuthService)
const API_BASE_URL = 'https://localhost:7031/api'; 

/**
 * Função utilitária para fazer requisições GET autenticadas para o Backend.
 * @param {string} path - O caminho do endpoint (ex: '/dashboard').
 * @returns {object} Os dados da resposta da API.
 */
export const fetchAuthenticated = async (path) => {
    const token = getAuthToken();

    // Se por algum motivo o token sumiu ou a página foi acessada diretamente
    if (!token) {
        // Isso forçará o erro e o ProtectedRoute deve redirecionar ou tratar
        throw new Error('Sessão expirada. Redirecionando para login.');
    }

    try {
        const response = await fetch(`${API_BASE_URL}${path}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                // 🔑 PASSO CRÍTICO: Anexar o token na header 'Authorization' 🔑
                'Authorization': `Bearer ${token}`, 
            },
        });

        const data = await response.json();

        // Tratamento de erro comum do Back-end: 401 (Não autorizado)
        if (response.status === 401) {
            localStorage.removeItem('pilotmaster_token'); // Limpa token inválido
            throw new Error('Sessão expirada. Faça login novamente.');
        }

        // Se o status for 400 ou 500, o Backend enviou uma mensagem de erro
        if (!response.ok) {
            throw new Error(data.message || `Falha ao carregar dados do ${path}`);
        }

        return data; // Retorna os dados esperados do Back-end

    } catch (error) {
        // Erro de rede (servidor fora do ar, por exemplo)
        console.error(`Erro na requisição ${path}:`, error);
        throw new Error('Não foi possível conectar ao servidor ou dados inválidos.');
    }
};