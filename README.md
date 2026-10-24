# Overload

**Uma API para organizar exercícios e construir a base de uma experiência de treinos.**

O Overload é um projeto em desenvolvimento feito com C# e ASP.NET Core. A API já permite criar usuários, autenticar com JWT e gerenciar exercícios associados à conta autenticada. A modelagem também começa a representar rotinas semanais, preparando o domínio para os próximos passos do produto.

[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-database-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Status](https://img.shields.io/badge/status-em%20desenvolvimento-7952B3)](#estado-do-projeto)

## Sobre o projeto

O Overload parte de uma ideia simples: dar estrutura aos dados que compõem um treino. Cada exercício pertence a um autor e pode registrar nome, grupo muscular, tipo de execução e indicação de visibilidade. A autenticação conecta essas informações ao usuário que está fazendo a requisição.

Mais do que um conjunto de rotas, o projeto explora conceitos como autenticação baseada em tokens, validação e transformação de dados, persistência relacional, enums representados por texto e controle de acesso aos recursos do usuário.

## O que já está implementado

- Cadastro de usuários com validação dos dados e senha armazenada com hash BCrypt.
- Login usando nome de usuário ou e-mail, com emissão de token JWT.
- Consulta de informações do usuário autenticado.
- Criação, listagem e atualização parcial de exercícios do próprio usuário.
- Associação de exercícios a um autor e verificação de propriedade durante a atualização.
- Classificação de exercícios por grupo muscular e tipo, com enums serializados como texto.
- Persistência PostgreSQL com Entity Framework Core e migrations.
- Respostas de erro padronizadas com `ProblemDetails`.

Os exercícios incluem um indicador `IsPublic`. No estado atual, as rotas de consulta retornam os exercícios do usuário autenticado; ainda não há feed público ou fluxo social implementado.

## API

As rotas abaixo usam o prefixo `/api`. As rotas de exercícios e de informações do usuário exigem autenticação. Envie no cabeçalho `Authorization` o esquema `Bearer` seguido pelo token JWT retornado no login.

| Método | Rota | Acesso | Descrição |
| --- | --- | --- | --- |
| `POST` | `/api/User` | Público | Cria uma conta |
| `GET` | `/api/User` | Público | Retorna uma mensagem de identificação da API |
| `GET` | `/api/User/info` | JWT | Retorna o nome de usuário autenticado |
| `POST` | `/api/Auth/login` | Público | Autentica por nome de usuário ou e-mail |
| `POST` | `/api/exercises` | JWT | Cria um exercício para o usuário autenticado |
| `GET` | `/api/exercises` | JWT | Lista os exercícios do usuário autenticado |
| `PATCH` | `/api/exercises/{exerciseId}` | JWT | Atualiza parcialmente um exercício próprio |

### Exemplos

Criar uma conta:

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

Entrar com nome de usuário ou e-mail:

```http
POST /api/Auth/login
Content-Type: application/json

{
  "username": "ana.silva",
  "email": "",
  "password": "senha-com-pelo-menos-8-caracteres"
}
```

A resposta contém um token e o tipo `Bearer`. Use o token para chamar as rotas protegidas.

Criar um exercício:

```http
POST /api/exercises
Content-Type: application/json

{
  "name": "Supino reto",
  "muscle": "Chest",
  "type": "FreeWeight",
  "isPublic": true
}
```

Envie essa requisição com o token JWT no cabeçalho de autorização.

Os grupos musculares disponíveis são `Chest`, `Back`, `Shoulders`, `Biceps`, `Triceps`, `Forearms`, `Abs`, `Quads`, `Hamstrings`, `Glutes` e `Calves`. Os tipos de exercício são `Machine`, `FreeWeight`, `Bodyweight` e `Cable`.

## Modelo do domínio

- **Usuário:** dados básicos de cadastro, credenciais e data de criação.
- **Exercício:** nome, grupo muscular, tipo, autor, visibilidade e data de criação.
- **Rotina:** modelo inicial para nome, dia da semana, autor, exercícios e visibilidade. Ainda não está conectado ao contexto de persistência nem a rotas da API.

## Organização do código

```text
overload/
├── Controllers/   # Endpoints HTTP
├── Services/      # Regras de cadastro, autenticação e exercícios
├── DTOs/          # Dados de entrada e atualização da API
├── Entities/      # Modelos do domínio e enums
├── Data/          # Contexto e configurações do Entity Framework Core
├── Migrations/    # Evolução do esquema do banco de dados
├── Exceptions/    # Tratamento de exceções e respostas de erro
└── Utils/         # Geração e validação de tokens JWT

overload.Tests/    # Testes automatizados
```

## Tecnologias

- C# e ASP.NET Core 10
- Entity Framework Core
- PostgreSQL com Npgsql
- JSON Web Token (JWT) e BCrypt
- xUnit e Moq para testes

## Executar localmente

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL
- Ferramenta `dotnet-ef` para aplicar migrations

### Configuração

Na raiz do repositório, configure uma connection string e uma chave para assinatura dos tokens. Por exemplo, no Bash:

```bash
export ConnectionStrings__PostgresConnection="Host=localhost;Port=5432;Database=overload;Username=postgres"
export AuthConfiguration__Key="substitua-por-uma-chave-secreta-local"
```

Use uma chave própria para seu ambiente e não a publique no repositório.

### Iniciar a API

```bash
dotnet restore overload/overload-csharp.csproj
dotnet ef database update --project overload/overload-csharp.csproj
dotnet run --project overload/overload-csharp.csproj
```

O endereço local é informado no terminal ao iniciar a aplicação.

### Executar os testes

```bash
dotnet test overload.Tests/overload.Tests.csproj
```

## Estado do projeto

O projeto está em desenvolvimento. Cadastro, login e gerenciamento de exercícios formam a base funcional atual. A entidade `Routine` registra o início da modelagem de rotinas, mas os fluxos de persistência e API correspondentes ainda não foram implementados.

## Licença

Distribuído sob a licença [MIT](./overload/LICENSE).
