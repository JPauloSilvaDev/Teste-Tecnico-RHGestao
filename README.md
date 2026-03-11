## Teste Técnico - Sistema de Gestão

Aplicação fullstack para gestão de **clientes**, **produtos** e **pedidos**, composta por:
- **Backend**: API REST em ASP.NET Core 9 / EF Core / SQL Server.
- **Frontend**: SPA em Angular 19 (projeto `StoreFront`).

---

## Arquitetura Utilizada

- **Camadas**:
  - **Domain** (`backend/Domain`): entidades e interfaces de domínio (`Customer`, `Product`, `Order`, `OrderItem`, `User` etc.).
  - **Application** (`backend/Application`): DTOs, serviços de aplicação (`OrderService`), repositórios de aplicação, validadores com FluentValidation.
  - **Infrastructure** (`backend/Infrastructure`): `DbContext` (`ManagementSystemDbContext`), configurações Fluent API, repositórios concretos (Customer, Product, Order), Migrations do EF Core.
  - **API** (`backend/API`): camada de apresentação HTTP (Controllers, Middleware de exceção, Program.cs, configuração de DI, CORS, Swagger, logging).
- **Padrões e práticas**:
  - Injeção de dependência para serviços e repositórios.
  - Validação de entrada com **FluentValidation** (ex.: `CreateOrderValidator`, `CustomerValidators`, `ProductValidators`).
  - Tratamento centralizado de erros via `ExceptionHandlingMiddleware`.
  - Documentação automática da API com **Swagger/OpenAPI**.
  - Logging estruturado com **Serilog** (console e arquivo em `logs/app-.log`).
  - Banco de dados relacional **SQL Server** via **Entity Framework Core** (Code First + migrations).
  - Frontend consumindo a API via `environment.apiUrl`.

---

## Tecnologias Utilizadas

- **Backend**
  - .NET **9.0**
  - ASP.NET Core Web API
  - Entity Framework Core **9.0.13** (SQL Server, Tools, Design)
  - FluentValidation
  - Swagger / Swashbuckle (`Swashbuckle.AspNetCore`)
  - Serilog (Sinks Console e File)

- **Frontend**
  - Angular **19** (CLI 19.2.22)
  - Angular Material / CDK
  - RxJS
  - Typescript 5.7

---

## Banco de Dados e Migrations

- **Provider**: SQL Server
- **Connection string padrão** (`backend/API/appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Scripts / Migrations

As migrations do EF Core estão em `backend/Infrastructure/Migrations`:
- `20260311152824_InitialCreate` – Cria tabelas `Customers`, `Orders`, `Products`, `OrderItems` e relacionamentos.
- `20260311171145_AddProductDescription` – Adiciona coluna de descrição ao produto.
- `20260311171624_SeedInitialData` – Popula dados iniciais (clientes, produtos etc.).

### Aplicando as migrations

1. Certifique-se de ter o **SQL Server** rodando localmente e que a connection string aponte para o servidor correto.
2. No diretório `backend` (onde está a solution `Domain.sln`), execute:

```bash
dotnet restore
dotnet ef database update --project Infrastructure --startup-project API
```

> Observação: se o `dotnet-ef` não estiver instalado globalmente, instale com:

```bash
dotnet tool install --global dotnet-ef
```

---

## Instruções Gerais para Rodar o Projeto

### Pré-requisitos

- **.NET SDK 9.0** instalado.
- **Node.js** (versão compatível com Angular 19) e **npm**.
- **Angular CLI** instalado globalmente:

```bash
npm install -g @angular/cli
```

- **SQL Server** disponível em `localhost` (ou ajuste a connection string).

---

## Backend - Como Executar

1. Abra um terminal na pasta `backend`:

```bash
cd backend
dotnet restore
```

2. (Opcional, se ainda não aplicou) Aplicar migrations / criar banco:

```bash
dotnet ef database update --project Infrastructure --startup-project API
```

3. Executar a API:

```bash
dotnet run --project API
```

4. Endpoints principais:
   - Base address (HTTPS, padrão .NET): algo como `https://localhost:7039/`
   - API: `https://localhost:7039/api`
   - Swagger UI: `https://localhost:7039/swagger`

5. CORS:
   - O backend está configurado para aceitar requisições do `http://localhost:4200` (Angular), via policy `AllowAngularClient` configurada em `Program.cs`.

---

## Frontend (Angular) - Como Executar

1. Abra outro terminal na pasta do frontend:

```bash
cd frontend/StoreFront
npm install
```

2. Rodar o servidor de desenvolvimento Angular:

```bash
npm start
```

ou

```bash
ng serve
```

3. Acessar no navegador:
   - `http://localhost:4200/`

4. Configuração da URL da API:
   - Ambiente de desenvolvimento (`src/environments/environment.ts`):

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7039/api',
};
```

   - Ambiente de produção (`src/environments/environment.prod.ts`):

```ts
export const environment = {
  production: true,
  apiUrl: '/api',
};
```

Se você alterar a porta ou URL do backend, ajuste `apiUrl` de acordo.

---

## Fluxo de Execução Completo (Backend + Frontend)

- **Passo 1**: Subir o banco de dados (SQL Server) local.
- **Passo 2**: Na pasta `backend`, restaurar pacotes, aplicar migrations e rodar a API:

```bash
cd backend
dotnet restore
dotnet ef database update --project Infrastructure --startup-project API
dotnet run --project API
```

- **Passo 3**: Na pasta `frontend/StoreFront`, instalar dependências e rodar o Angular:

```bash
cd frontend/StoreFront
npm install
npm start
```

- **Passo 4**: Acessar `http://localhost:4200` e utilizar o sistema. A API estará documentada em `/swagger`.

