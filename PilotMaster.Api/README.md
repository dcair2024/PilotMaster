# PilotMaster Backend (Sprint 1: Autenticação Base)

Este documento resume a infraestrutura e os pontos de acesso criados pelo Davi na Sprint 1.

## 🚀 Como Rodar

1.  **Framework:** Certifique-se de ter o **SDK .NET 8.0** instalado.
2.  **Banco de Dados:** Garanta que seu SQL Server LocalDB está ativo. O EF Core criou o banco de dados `PilotMasterDB` via migrations.
3.  **Execução:**
    * No Visual Studio: Selecione `PilotMaster.Api` como projeto de inicialização e pressione F5.
    * No Terminal (na pasta PilotMaster.Api): `dotnet run`

A API estará acessível em `https://localhost:[PORTA]/` (A porta é definida pelo Visual Studio ou `launchSettings.json`).

## 🔗 Endpoints de Autenticação (JWT)

Todos os testes de consumo devem ser feitos pelo **Swagger UI** (`/swagger/index.html`).

| Endpoint | Método | Descrição | Status da Sprint 1 |
| :--- | :--- | :--- | :--- |
| `/api/auth/login` | `POST` | Autentica e retorna **AccessToken** e **RefreshToken**. | **PRONTO** (Requer um usuário no DB) |
| `/api/auth/test` | `GET` | Rota protegida. Requer um JWT válido no cabeçalho `Authorization`. | **PRONTO** |
| `/api/auth/refresh` | `POST` | Renovação de token. | **NÃO IMPLEMENTADO** (Retorna 501 - Not Implemented) |

## 🔑 Exemplo de Request de Login

Use este corpo de requisição (JSON) no endpoint `/api/auth/login`:

```json
{
  "email": "teste@pilotmaster.com",
  "senha": "senha" 
  // LEMBRETE: Em produção, a senha será um hash (ex: BCrypt),
  // mas para testes iniciais, está em plain text no banco.
}