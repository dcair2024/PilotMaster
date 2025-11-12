// /src/pages/Login.jsx

import React, { useState } from 'react';
// 1. IMPORTAÇÃO: Adiciona a função 'login' do seu serviço
import { login } from '../Services/AuthService'; 

function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null); 
    const [isLoading, setIsLoading] = useState(false); 

    // 2. ATUALIZAÇÃO: A função agora é assíncrona (async)
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setIsLoading(true);

        try {
            // 3. USO DO SERVIÇO: Chama a API do Davi
            await login(email, password);

            // Se chegou aqui, o login foi SUCESSO e o token está no localStorage.
            console.log('Login Sucedido! Redirecionando...');

            // 4. PRÓXIMA TAREFA: Aqui deve ocorrer o redirecionamento para a rota /home
            // (Isso será implementado na Tarefa 4 com um roteador)
            alert('Login Sucedido! Preparado para a página /home');

        } catch (err) {
            // Se houver erro (login inválido, API fora do ar)
            setError(err.message || 'Erro de autenticação.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login-container">
            <h1>Acesso PilotMaster</h1>
            {error && <p style={{ color: 'red' }}>{error}</p>} 
            
            <form onSubmit={handleSubmit}>
                
                <div>
                    <label htmlFor="email">Email</label>
                    <input
                        id="email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                
                <div>
                    <label htmlFor="password">Senha</label>
                    <input
                        id="password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                
                <button type="submit" disabled={isLoading}>
                    {isLoading ? 'Conectando...' : 'Entrar'}
                </button>
            </form>
        </div>
    );
}

export default LoginPage;