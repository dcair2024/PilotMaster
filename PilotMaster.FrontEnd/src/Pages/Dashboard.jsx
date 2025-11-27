import { useEffect, useState } from "react";
import fetchAuthenticated from "../Services/fetchAuthenticated";
import { logout } from "../Services/AuthService";

export default function Dashboard() {
  const [dados, setDados] = useState(null);
  const [erro, setErro] = useState("");

  useEffect(() => {
    const load = async () => {
      try {
        const data = await fetchAuthenticated("/dashboard");
        setDados(data);
      } catch (err) {
        setErro(err.message || "Erro inesperado.");
      }
    };

    load();
  }, []);

  return (
    <div>
      <h1>Dashboard</h1>

      {erro && <p style={{ color: "red" }}>{erro}</p>}

      {!dados && !erro && <p>Carregando...</p>}

      {dados && (
        <pre style={{ background: "#eee", padding: "10px" }}>
          {JSON.stringify(dados, null, 2)}
        </pre>
      )}

      <button onClick={logout}>Sair</button>
    </div>
  );
}
