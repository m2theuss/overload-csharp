# Overload

**Uma API para organizar exercícios e montar rotinas de treino.**

Overload é uma API REST em C# e ASP.NET Core para cadastro e autenticação de usuários, gerenciamento de exercícios e criação de rotinas semanais. Os recursos são associados ao usuário autenticado, com persistência em PostgreSQL.

[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-database-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Status](https://img.shields.io/badge/status-em%20desenvolvimento-7952B3)](#estado-do-projeto)

## Visão geral

O projeto oferece uma base para representar os dados de um treino:

- **Usuários** podem criar uma conta e entrar com nome de usuário ou e-mail.
- **Exercícios** registram nome, grupo muscular, tipo, visibilidade e autor.
- **Rotinas** organizam exercícios por dia da semana e mantêm a ordem de execução.

A autenticação usa JWT. Exercícios e rotinas são associados ao usuário que os criou; o campo de visibilidade existe no modelo, mas ainda não há feed público nem compartilhamento entre usuários.

## Funcionalidades

- Cadastro de usuários, validação dos dados e armazenamento de senha com hash BCrypt.
- Login com nome de usuário ou e-mail e emissão de token JWT.
- Consulta das informações da conta autenticada.
- Criação e listagem de exercícios próprios, além de atualização parcial com verificação de autoria.
- Criação e listagem de rotinas próprias.
- Inclusão de exercícios próprios em uma rotina própria, com controle de ordem e prevenção de duplicatas.
- Persistência PostgreSQL com Entity Framework Core e migrations.
- Enums serializados como texto e respostas de erro no formato `ProblemDetails`.

## Comece aqui

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL
- Ferramenta `dotnet-ef` para aplicar migrations

### Configure o ambiente

Crie um banco PostgreSQL e configure a connection string e a chave JWT no ambiente. Exemplo para Bash:

```bash
export ConnectionStrings__PostgresConnection="Host=localhost;Port=5432;Database=overload;Username=postgres;Password=sua-senha"
export AuthConfiguration__Key="substitua-por-uma-chave-secreta-local"
```

Use uma chave secreta própria para cada ambiente. Não inclua credenciais ou segredos no repositório.

### Restaure, aplique as migrations e inicie a API

Na raiz do repositório:

```bash
dotnet restore overload/overload-csharp.csproj
dotnet ef database update --project overload/overload-csharp.csproj
dotnet run --project overload/overload-csharp.csproj
```

O endereço HTTP será exibido no terminal. Use-o como base para as chamadas abaixo.

## API

Todas as rotas usam o prefixo `/api`. Rotas protegidas precisam do cabeçalho:

```http
Authorization: Bearer <token>
```

Obtenha o token em `POST /api/Auth/login`. As rotas de exercícios e rotinas atuam sobre recursos do usuário autenticado; as rotas de listagem não são um feed público.

| Método | Rota | Acesso | Descrição |
| --- | --- | --- | --- |
| `GET` | `/api/User` | Público | Retorna uma mensagem de identificação da API |
| `POST` | `/api/User` | Público | Cria uma conta |
| `POST` | `/api/Auth/login` | Público | Autentica por nome de usuário ou e-mail |
| `GET` | `/api/User/info` | JWT | Retorna o nome e o identificador do usuário autenticado |
| `POST` | `/api/exercises` | JWT | Cria um exercício para o usuário autenticado |
| `GET` | `/api/exercises` | JWT | Lista os exercícios do usuário autenticado |
| `PATCH` | `/api/exercises/{exerciseId}` | JWT | Atualiza parcialmente um exercício próprio |
| `POST` | `/api/routines` | JWT | Cria uma rotina para o usuário autenticado |
| `POST` | `/api/routines/{routineId}/exercises` | JWT | Adiciona um exercício próprio a uma rotina própria |
| `GET` | `/api/routines/users/{anonId}/routines` | JWT | Lista as rotinas do usuário autenticado; outro identificador retorna `404` |

### Fluxo de autenticação

Cadastre a conta:

```http
POST /api/User
Content-Type: application/json

{
  "firstName": "Ana",
  "lastName": "Silva",
  "username": "ana.silva",
  "email": "ana@example.com",
  "password": "senha-com-pelo-menos-8-caracteres",
  "birthDate": "1998-04-12"
}
```

Entre com o nome de usuário ou e-mail e a senha:

```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "ana.silva",
  "email": "",
  "password": "senha-com-pelo-menos-8-caracteres"
}
```

A resposta contém o token e o tipo `Bearer`. Envie-os no cabeçalho `Authorization` para as rotas protegidas.

### Exercícios

Crie um exercício:

```http
POST /api/exercises
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Supino reto",
  "muscle": "Chest",
  "type": "FreeWeight",
  "isPublic": true
}
```

Grupos musculares aceitos: `Chest`, `Back`, `Shoulders`, `Biceps`, `Triceps`, `Forearms`, `Abs`, `Quads`, `Hamstrings`, `Glutes` e `Calves`. Tipos aceitos: `Machine`, `FreeWeight`, `Bodyweight` e `Cable`.

### Rotinas

Crie uma rotina. `day` aceita os dias da semana em inglês: `Monday`, `Tuesday`, `Wednesday`, `Thursday`, `Friday`, `Saturday` ou `Sunday`:

```http
POST /api/routines
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Treino A",
  "day": "Monday",
  "isPublic": false
}
```

Adicione à rotina um exercício que pertença ao mesmo usuário. O `routineId` é informado na rota; `order` define a posição do exercício:

```http
POST /api/routines/10/exercises
Authorization: Bearer <token>
Content-Type: application/json

{
  "exerciseId": 17,
  "order": 1
}
```

Liste as rotinas da própria conta, substituindo `{anonId}` pelo identificador do usuário autenticado:

```http
GET /api/routines/users/10/routines
Authorization: Bearer <token>
```

## Testes

Execute a suíte automatizada a partir da raiz:

```bash
dotnet test overload.Tests/overload.Tests.csproj
```

A cobertura automatizada ainda é inicial e focada nos serviços. Os cenários existentes verificam:

- criação de usuário com dados válidos e rejeição de cadastro duplicado;
- criação de exercício para usuário existente;
- atualização de exercício pelo autor e rejeição da atualização por outro usuário.

Os testes usam SQLite em memória e xUnit; não precisam de uma instância PostgreSQL. O arquivo `UnitTest1` ainda contém apenas um teste-template vazio, sem assertions. A suíte não cobre atualmente os endpoints HTTP nem os fluxos de rotina.

### Chamadas manuais com Bruno

Há uma coleção Bruno em [`bruno/overload_csharp`](./bruno/overload_csharp) com chamadas de exemplo para cadastro, login, informações do usuário, exercícios e rotinas. Importe a coleção no Bruno, inicie a API e substitua os valores de autorização mascarados por um token válido. As URLs da coleção usam `localhost:5071`; ajuste o endereço caso a API esteja rodando em outra porta.

## Organização do código

```text
overload/
├── Controllers/   # Endpoints HTTP
├── Services/      # Regras de cadastro, autenticação, exercícios e rotinas
├── DTOs/          # Contratos de entrada e atualização da API
├── Entities/      # Modelos do domínio e enums
├── Data/          # Contexto e configurações do Entity Framework Core
├── Migrations/    # Evolução do esquema do banco de dados
├── Exceptions/    # Tratamento de exceções e respostas de erro
└── Utils/         # Geração e validação de tokens JWT

overload.Tests/    # Testes automatizados de serviços
bruno/             # Coleção de chamadas HTTP para Bruno
```

## Tecnologias

- C# e ASP.NET Core 10
- Entity Framework Core e PostgreSQL com Npgsql
- JWT e BCrypt
- xUnit, Moq e SQLite para testes
- Bruno para chamadas HTTP manuais

## Estado do projeto

O projeto está em desenvolvimento. Cadastro, autenticação, exercícios e operações básicas de rotina já estão implementados. A visibilidade dos recursos ainda não oferece uma experiência pública, e a cobertura automatizada dos endpoints e das rotinas é trabalho futuro.

## Licença

Distribuído sob a licença [MIT](./overload/LICENSE).
