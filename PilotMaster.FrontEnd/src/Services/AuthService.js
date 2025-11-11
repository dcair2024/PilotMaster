// /src/services/AuthService.js

import axios from 'axios';

// 1. A URL base que aponta para http://localhost:5000/api
const API_URL = import.meta.env.VITE_API_BASE_URL;

/**
 * Envia credenciais para o backend (/auth/login) e armazena o Access Token.
 * @param {string} email
 * @param {string} password
 * @returns {object} Dados da resposta da API.
 */
export const login = async (email, password) => {
    try {
        // Chamada POST para o endpoint do Davi: /auth/login
        const response = await axios.post(`${API_URL}/auth/login`, {
            email,
            password,
        });

        const { accessToken } = response.data;

        if (accessToken) {
            // Requisito da Tarefa 3: Armazena o token no localStorage
            localStorage.setItem('accessToken', accessToken);
        }

        return response.data;

    } catch (error) {
        // Lança um erro com a mensagem do backend, se disponível
        throw new Error(error.response?.data.message || 'Erro ao conectar com o serviço de autenticação.');
    }
};

// Funções utilitárias que serão usadas nas rotas protegidas
export const logout = () => {
    localStorage.removeItem('accessToken');
};

export const getToken = () => {
    return localStorage.getItem('accessToken');
};