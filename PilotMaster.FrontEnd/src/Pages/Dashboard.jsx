// src/pages/Dashboard.jsx

import React, { useEffect, useState } from 'react';
import { fetchAuthenticated } from '../Services/ApiService'; // Importa a função criada

const Dashboard = () => {
    // ⭐️ Criar estados para os 3 conjuntos de dados ⭐️
    const [dashboardData, setDashboardData] = useState(null); // Dados gerais do /dashboard
    const [recentManeuvers, setRecentManeuvers] = useState([]); // Dados do /manobras/recentes
    const [activeShips, setActiveShips] = useState([]); // Dados do /navios/ativos
    
    // 💡 Estados para a Tarefa FE-06 💡
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadDashboardData = async () => {
            setLoading(true); // Inicia o loading (FE-06)
            setError(null);

            try {
                // 🔑 1. BUSCAR DADOS GERAIS (BK-07)
                const geralPromise = fetchAuthenticated('/dashboard'); 
                
                // 🔑 2. BUSCAR MANOBRAS RECENTES (BK-08)
                const manobrasPromise = fetchAuthenticated('/manobras/recentes');
                
                // 🔑 3. BUSCAR NAVIOS ATIVOS (BK-09)
                const naviosPromise = fetchAuthenticated('/navios/ativos');

                // Executa todas as buscas em paralelo para otimizar o tempo de carregamento
                const [dataGeral, manobras, navios] = await Promise.all([
                    geralPromise, 
                    manobrasPromise, 
                    naviosPromise
                ]);

                // ⭐️ Atualiza o estado com os dados reais ⭐️
                setDashboardData(dataGeral);
                setRecentManeuvers(manobras);
                setActiveShips(navios);

            } catch (err) {
                // Captura e exibe o erro (FE-06)
                setError(err.message);
                // Aqui você integraria o Toast do Claudio (AS-04)
            } finally {
                setLoading(false); // Finaliza o loading (FE-06)
            }
        };

        loadDashboardData();
    }, []); 

    // -----------------------------------------------------
    // 🛠️ RENDERIZAÇÃO DO COMPONENTE (Integração com FE-06) 🛠️
    // -----------------------------------------------------

    if (loading) {
        // Exibir o spinner de loading do Claudio (AS-04)
        return <div>Carregando dados do Dashboard...</div>; 
    }

    if (error) {
        // Exibir a mensagem de erro (Toast)
        return <div>Ocorreu um erro: {error}. Tente novamente.</div>;
    }

    if (!dashboardData) {
         return <div>Sem dados para exibir.</div>;
    }

    // -----------------------------------------------------
    // 🚢 RENDERIZAÇÃO PRINCIPAL 🚢
    // -----------------------------------------------------
    return (
        <div className="dashboard-container">
            <h1>Dashboard PilotMaster</h1>
            
            {/* Seção 1: Métricas Gerais (Ex: Navios, Manobras, Agentes) */}
            <div className="metrics-cards">
                <p>Navios Totais: {dashboardData.totalNavios}</p>
                <p>Manobras Concluídas: {dashboardData.manobrasConcluidas}</p>
                <p>Agentes Logados: {dashboardData.agentesAtivos}</p>
            </div>

            {/* Seção 2: Manobras Recentes (Lista) */}
            <h2>Manobras Recentes</h2>
            <ul>
                {recentManeuvers.map(manobra => (
                    <li key={manobra.id}>
                        {manobra.nomeNavio} - {manobra.status}
                    </li>
                ))}
            </ul>

            {/* Seção 3: Navios Ativos (Lista) */}
            <h2>Navios Ativos</h2>
            <ul>
                {activeShips.map(navio => (
                    <li key={navio.id}>{navio.nome}</li>
                ))}
            </ul>

            {/* ... (Implementar o layout completo do wireframe da Carol) */}
        </div>
    );
};

export default Dashboard;