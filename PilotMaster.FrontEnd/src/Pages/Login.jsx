// /src/pages/Login.jsx

// Arquivo: src/pages/Login.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../Services/AuthService';
// Importe as funções de loading e toast que o Claudio criou (FE-06)
// import { showToast, showLoading } from '../utils/utils'; 

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false); // Para o loading manual (FE-06)

    const handleSubmit = async (e) => {
        e.preventDefault();

        setIsLoading(true); // ⭐️ 1. Exibir Loading (Início da FE-06)

        try {
            // A credencial de teste que deve funcionar agora: admin@pilotmaster.com / Admin@123
            const result = await login(email, password);

            if (result.success) {
                // Login Aprovado (QA-05 Aprovado!)
                navigate('/home'); // Redireciona para a rota protegida
                // showToast('success', 'Bem-vindo ao PilotMaster!'); 
            }

        } catch (error) {
            // ⭐️ 2. Exibir Toast de Erro (Fim da FE-06)
            console.error(error.message);
            // showToast('error', error.message); 
        } finally {
            setIsLoading(false); // ⭐️ 3. Ocultar Loading (Fim da FE-06)
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            {/* Input para Email, ligando ao estado 'email' */}
            <input 
                type="email" 
                value={email} 
                onChange={(e) => setEmail(e.target.value)} 
                placeholder="Email" 
            />
            {/* Input para Senha, ligando ao estado 'password' */}
            <input 
                type="password" 
                value={password} 
                onChange={(e) => setPassword(e.target.value)} 
                placeholder="Senha" 
            />
            
            [cite_start]{/* O botão 'ENTRAR' do wireframe [cite: 48] */}
            <button type="submit" disabled={isLoading}>
                {isLoading ? 'Carregando...' : 'ENTRAR'} 
            </button>
            {/* Aqui você integraria o componente de spinner de loading do Claudio */}
        </form>
    );
};

export default Login;