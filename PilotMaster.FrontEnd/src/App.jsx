// /src/App.jsx (ou onde você está definindo as rotas)

import React from 'react';
// Importa componentes do roteador
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';

// Importa as páginas e a Guarda de Rota
import LoginPage from './Pages/Login'; 
import HomePage from './Pages/Home';
import ProtectedRoute from './components/ProtectedRoute';

function App() {
    return (
        // Envolve toda a aplicação no Roteador
        <BrowserRouter>
            <div className="App">
                <Routes>
                    {/* Rota 1: /login (Página pública) */}
                    <Route path="/login" element={<LoginPage />} />
                    
                    {/* Rota 2: Rota Raiz (Redireciona para /login se não houver login) */}
                    <Route path="/" element={<Navigate replace to="/login" />} />

                    {/* Rota 3: /home (ROTA PROTEGIDA - usa o ProtectedRoute) */}
                    <Route element={<ProtectedRoute />}>
                        {/* A página HomePage só será acessada se ProtectedRoute retornar <Outlet> */}
                        <Route path="/home" element={<HomePage />} />
                    </Route>
                    
                    {/* Rota 4: Qualquer outra rota (Pode ser uma página 404) */}
                    <Route path="*" element={<h1>404: Página não encontrada</h1>} />
                </Routes>
            </div>
        </BrowserRouter>
    );
}

export default App;