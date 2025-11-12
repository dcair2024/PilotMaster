// /src/pages/Home.jsx

import React from 'react';
import { logout } from '../Services/AuthService'; // Importa o logout

function HomePage() {
    
    const handleLogout = () => {
        logout(); // Remove o token do localStorage
        // Você precisará redirecionar para /login após o logout (feito no roteador principal)
        window.location.href = '/login'; 
    };

    return (
        <div className="home-container">
            <h1>✅ Área do Piloto (Rota Protegida)</h1>
            <p>Se você está vendo esta página, seu token JWT foi validado com sucesso!</p>
            
            <button onClick={handleLogout}>Sair (Logout)</button>
            
            <p>Token atual no localStorage: {localStorage.getItem('accessToken') ? 'PRESENTE' : 'AUSENTE'}</p>
        </div>
    );
}

export default HomePage;