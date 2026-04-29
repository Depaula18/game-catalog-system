# 🎮 Game Catalog System

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-4EA94B?style=for-the-badge&logo=mongodb&logoColor=white)

Uma aplicação Full Stack completa para gerenciamento de catálogos de jogos, construída com foco em Arquitetura Limpa (Clean Architecture), resiliência de dados e boas práticas de desenvolvimento.

## 🌐 Acesso Online (Demo)

- Front-end (Vercel): https://game-catalog-system.vercel.app/
- Login: `jeffe@bahia.com`
- Senha: `123456`

## ✨ Funcionalidades

- **Gerenciamento de Jogos:** Cadastro, edição, listagem e exclusão de jogos (CRUD completo).
- **Upload de Capas:** Sistema de upload de imagens para as capas dos jogos com tratamento de fallback estático.
- **Busca e Paginação:** Listagem otimizada com suporte a paginação e filtros de busca por título ou descrição.
- **Autenticacao:** Sistema de Login e Registro protegido por JWT (JSON Web Tokens).
- **Auditoria Transparente:** Todo registro de criação de jogo gera automaticamente um log de auditoria em um banco NoSQL paralelo.

## 🚀 Tecnologias Utilizadas

### Front-end
- React com TypeScript
- Vite
- Tailwind CSS para estilização
- Axios para consumo da API
- Lucide React para ícones

### Back-end
- C# .NET 8 (Web API)
- Entity Framework Core
- Autenticação JWT
- Padrões de Projeto: Repository Pattern, Dependency Injection

### Bancos de Dados & Infraestrutura
- **PostgreSQL (Neon):** Banco relacional principal para usuários, jogos e gêneros em produção.
- **MongoDB (Atlas):** Banco NoSQL utilizado exclusivamente para o serviço de logs e auditoria.
- **Hospedagem:** Render (Back-end) e Vercel (Front-end).

> Observação: no ambiente local deste repositório, o backend também suporta SQL Server via `appsettings.json`/Docker. Em produção, usa `DATABASE_URL` (PostgreSQL).

## 📁 Estrutura do Projeto

```bash
.
├── GameCatalogSystem/      # Solução .NET (API, Application, Domain, Infrastructure)
├── frontend-react/         # Front-end React + TypeScript
├── docker-compose.yml      # Containers locais de banco
└── Dockerfile
```

## 🛠️ Como rodar o projeto localmente

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recomendado para subir bancos locais)

### Passo a Passo

1. Clone o repositório:
   ```bash
   git clone https://github.com/Depaula18/game-catalog-system
   cd game-catalog-system
   ```

2. Suba os bancos locais com Docker (SQL Server + MongoDB):
   ```bash
   docker compose up -d
   ```

3. Configure o backend:
   - Arquivo principal: `GameCatalogSystem/WebApplication1/appsettings.json`
   - Por padrão, ele já contém:
     - `ConnectionStrings:DefaultConnection` (SQL Server local)
     - `MongoDbSettings:ConnectionString` (Mongo local)
     - `JwtSettings` (chave/issuer/audience)

4. (Opcional) Usar PostgreSQL local/remoto no backend:
   - Defina a variável de ambiente `DATABASE_URL` com sua string PostgreSQL.
   - Se `DATABASE_URL` existir, a API usa PostgreSQL automaticamente.

5. Execute a API .NET:
   ```bash
   cd GameCatalogSystem/WebApplication1
   dotnet restore
   dotnet run
   ```
   A API deve iniciar em algo como:
   - `https://localhost:7277`
   - `http://localhost:5273`

6. Em outro terminal, execute o front-end:
   ```bash
   cd frontend-react
   npm install
   npm run dev
   ```

7. Configure a URL da API no front-end:
   - Arquivo: `frontend-react/src/services/api.ts`
   - Para local, use:
     - `https://localhost:7277` (ou `http://localhost:5273`)
   - Para produção, use:
     - `https://game-catalog-api-xk1h.onrender.com`

8. Acesse no navegador:
   - Front-end Vite: `http://localhost:5173`
   - Swagger API: `https://localhost:7277/swagger`

## 🔐 Variáveis de Ambiente (Back-end)

As variáveis abaixo são suportadas pelo backend:

- `DATABASE_URL`: string de conexão PostgreSQL (quando presente, tem prioridade sobre SQL Server local).
- `MONGODB_URI`: string de conexão MongoDB (sobrescreve o valor local de `appsettings.json`).

Exemplo (PowerShell):

```powershell
$env:DATABASE_URL="Host=...;Database=...;Username=...;Password=...;SSL Mode=Require;Trust Server Certificate=true"
$env:MONGODB_URI="mongodb+srv://usuario:senha@cluster.mongodb.net/?retryWrites=true&w=majority"
dotnet run --project .\GameCatalogSystem\WebApplication1
```

## 🧪 Scripts Úteis

### Front-end (`frontend-react`)

```bash
npm run dev      # ambiente de desenvolvimento
npm run build    # build de produção
npm run preview  # preview do build
npm run lint     # lint
```

### Back-end (`GameCatalogSystem/WebApplication1`)

```bash
dotnet run
dotnet build
```

## 📌 Roadmap (Sugestões)

- Melhorar cobertura de testes automatizados (unitários e integração).
- Mover URL da API no front para variavel `.env` (`VITE_API_BASE_URL`).
- Adicionar CI/CD para build + lint + testes.

## 👨‍💻 Autor

Murilo de Paula - [@Depaula18](https://github.com/Depaula18)

Projeto desenvolvido para fins de estudo e prática de arquitetura Full Stack com C#, .NET e React.

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.