// /src/components/ProtectedRoute.jsx

import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { getAuthToken } from '/src/services/AuthService'; // <-- Função importada

function ProtectedRoute() {
    // ⭐️ CORREÇÃO AQUI: Chamar a função com o nome correto (getAuthToken)
    const isAuthenticated = getAuthToken(); // ✅ CORRIGIDO

    // Se o usuário estiver autenticado (tem token), renderiza a página (Outlet).
    if (isAuthenticated) {
        return <Outlet />;
    } 
    
    // Se NÃO estiver autenticado, redireciona para a página de login.
    return <Navigate to="/login" />;
}

export default ProtectedRoute;