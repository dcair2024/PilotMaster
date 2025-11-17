// src/pages/Dashboard.jsx

// src/pages/Dashboard.jsx

import React, { useEffect, useState } from 'react';
import { fetchAuthenticated } from '../Services/ApiService'; // FE-05
import { logout } from '../Services/AuthService'; // Importa o logout

const Dashboard = () => {
    // Estados para os dados
    const [dashboardData, setDashboardData] = useState(null); 
    const [recentManeuvers, setRecentManeuvers] = useState([]); 
    const [activeShips, setActiveShips] = useState([]); 
    
    // Estados para UX (FE-06)
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Lógica do Logout (Nova posição)
    const handleLogout = () => {
        logout(); // Remove o token do localStorage
        window.location.href = '/login'; // Redireciona
    };

    useEffect(() => {
        const loadDashboardData = async () => {
            setLoading(true); // Inicia o loading
            setError(null);

            try {
                // Buscas paralelas (FE-05)
                const geralPromise = fetchAuthenticated('/dashboard'); 
                const manobrasPromise = fetchAuthenticated('/manobras/recentes');
                const naviosPromise = fetchAuthenticated('/navios/ativos');

                const [dataGeral, manobras, navios] = await Promise.all([
                    geralPromise, 
                    manobrasPromise, 
                    naviosPromise
                ]);

                // Atualiza o estado
                setDashboardData(dataGeral);
                setRecentManeuvers(manobras);
                setActiveShips(navios);

            } catch (err) {
                // Captura e exibe o erro (FE-06)
                setError(err.message);
            } finally {
                setLoading(false); // Finaliza o loading
            }
        };

        loadDashboardData();
    }, []); 

    // -----------------------------------------------------
    // RENDERIZAÇÃO CONDICIONAL (FE-06)
    // -----------------------------------------------------

    if (loading) {
        return <div>Carregando dados do Dashboard...</div>; 
    }

    if (error) {
        return <div>Ocorreu um erro: {error}. Por favor, verifique se o Back-end está ativo.</div>;
    }

    if (!dashboardData) {
        return <div>Sem dados para exibir.</div>;
    }

    // -----------------------------------------------------
    // RENDERIZAÇÃO PRINCIPAL 
    // -----------------------------------------------------
    return (
        <div className="dashboard-container">
            {/* Botão de Logout */}
            <button onClick={handleLogout}>Sair (Logout)</button>
            
            <h1>Dashboard PilotMaster</h1>
            
            {/* Seção 1: Métricas Gerais */}
            <div className="metrics-cards">
                <p>Navios Totais: {dashboardData.totalNavios}</p>
                <p>Manobras Concluídas: {dashboardData.manobrasConcluidas}</p>
                <p>Agentes Logados: {dashboardData.agentesAtivos}</p>
            </div>

            {/* Seção 2: Manobras Recentes */}
            <h2>Manobras Recentes</h2>
            <ul>
                {recentManeuvers.map((manobra, index) => (
                    <li key={index}>
                        {manobra.nomeNavio} - {manobra.status}
                    </li>
                ))}
            </ul>

            {/* Seção 3: Navios Ativos */}
            <h2>Navios Ativos</h2>
            <ul>
                {activeShips.map((navio, index) => (
                    <li key={index}>{navio.nome}</li>
                ))}
            </ul>
        </div>
    );
};

export default Dashboard;