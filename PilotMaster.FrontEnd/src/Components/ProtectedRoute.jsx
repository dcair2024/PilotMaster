// /src/components/ProtectedRoute.jsx

import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { getToken } from '../Services/AuthService'; // Importa a função do token

function ProtectedRoute() {
    // Verifica se o token existe no localStorage (Função que criamos no AuthService)
    const isAuthenticated = getToken();

    // Se o usuário estiver autenticado (tem token), renderiza a página (Outlet).
    if (isAuthenticated) {
        return <Outlet />;
    } 
    
    // Se NÃO estiver autenticado, redireciona para a página de login.
    return <Navigate to="/login" />;
}

export default ProtectedRoute;