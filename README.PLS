# 🏋️‍♂️ Overload C# - Social Workout Tracker API

O **Overload** é uma Web API robusta desenvolvida em **C#** e **.NET 10** projetada para funcionar como uma rede social de treinos de musculação. Mais do que um simples rastreador de exercícios, a plataforma permite que entusiastas do fitness compartilhem rotinas, sigam perfis e descubram novos treinos com base em agrupamentos musculares.

O foco técnico deste projeto é aplicar padrões de arquitetura limpa, segurança rigorosa e modelagem de relacionamentos complexos em banco de dados utilizando o ecossistema .NET moderno.

⚠️ **Status do Projeto:** 🛠️ Em Desenvolvimento (Work in Progress - WIP). Cerca de 10% concluído.

---

## 📝 Escopo e Regras de Negócio

O grande diferencial do **Overload** é a sua dinâmica social voltada para a hipertrofia e performance:

*   **Controle de Privacidade de Perfis:** Usuários podem definir seus perfis como **Públicos** ou **Privados** (onde apenas seguidores aprovados possuem acesso aos dados).
*   **Visibilidade de Treinos:** Ao criar um treino, o usuário decide se ele será **Público** (visível na busca global para qualquer um) ou **Privado** (restrito ao criador e/ou seguidores).
*   **Rede de Conexões:** Sistema de interações ("Seguir" e "Ser seguido") para criar uma comunidade ativa.
*   **Rotinas Inteligentes:** Ferramentas para montar cronogramas semanais misturando treinos autorais, treinos buscados na plataforma e filtros avançados por músculos-alvo.

---

## 🚀 O que já foi implementado?

O projeto está em sua fase inicial de infraestrutura backend e segurança:

*   **Arquitetura Camadas/Pastas:** Estrutura organizada com separação de responsabilidades claras:
    *   `Controllers`: Exposição dos endpoints da API.
    *   `Services`: Onde reside a lógica de negócios central.
    *   `DTOs`: Objetos de transferência de dados para proteção das entidades.
    *   `Entities`: Classes de domínio de negócio.
    *   `Data` & `Migrations`: Contexto de persistência configurado via EF Core.
*   **Autenticação & Segurança (WIP):** 
    *   Mecanismo de validação de senhas de usuários.
    *   Verificação de credenciais de login.
    *   Estrutura base para geração de tokens **JWT (JSON Web Token)**.

---

## 🛠️ Tecnologias e Ferramentas

*   **Linguagem:** C#
*   **Framework Backend:** .NET 10.0 (Web API)
*   **ORM / Persistência:** Entity Framework Core
*   **Segurança:** JWT (JSON Web Token) & Criptografia de senhas
*   **Banco de dados** PostgreSQL

---

## 🗺️ Roadmap de Desenvolvimento

Acompanhe o cronograma planejado para a evolução da plataforma:

- [x] Estrutura base da API e injeção de dependências (.NET 10)
- [/] Sistema de Autenticação JWT (Fase final de implementação)
- [ ] **Módulo de Usuários & Perfis:** Cadastro, hashing de senha e flags de privacidade (`IsPrivate`)
- [ ] **Módulo de Relacionamentos:** Funcionalidade de follow/unfollow (Auto-relacionamento Many-to-Many)
- [ ] **Módulo de Treinos (Core):** Criação de treinos, definição de exercícios e flags de visibilidade (`IsPublic`)
- [ ] **Módulo de Rotinas & Busca:** Sistema de filtros avançados por músculo e feed customizado baseado em quem você segue

---

## ⚙️ Como executar o projeto localmente

Se você deseja clonar o projeto e rodar o ambiente de desenvolvimento em sua máquina:

1. **Clone o repositório:**
   ```bash
   git clone https://github.com
   ```

2. **Navegue até o diretório do projeto:**
   ```bash
   cd overload-csharp
   ```

3. **Restaure as dependências do .NET:**
   ```bash
   dotnet restore
   ```

4. **Execute as Migrations para estruturar o Banco de Dados:**
   ```bash
   dotnet ef database update
   ```

5. **Inicie a aplicação:**
   ```bash
   dotnet run
   ```
   *A API ficará disponível nos endereços e portas locais configurados no arquivo `appsettings.Development.json`.*

---

## 📄 Licença

Este projeto é open-source e está licenciado sob a [MIT License](LICENSE).
