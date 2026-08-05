# ControleMultEstoque

Sistema full-stack de controle de estoque com múltiplos armazéns, desenvolvido para o **Hackathon entre Amigos** (Tema 3 — Controle de estoque com múltiplos armazéns).

> Projeto individual, sem uso de backend-as-a-service pronto, com autenticação e autorização implementadas do zero.

**Nota de nomenclatura:** o nome técnico do repositório e dos projetos/namespaces é `ControleMultEstoque`. "**StockMaster**" é o nome de marca/produto, usado apenas na interface (título das telas, logo "SM") — não aparece em nenhum nome de projeto, namespace ou classe.

---

## Stack Tecnológica

| Camada | Tecnologia |
|---|---|
| Backend (API) | ASP.NET Core Web API — .NET 10 |
| Frontend (Web) | ASP.NET Core Razor Pages — .NET 10 |
| Banco de Dados | SQL Server |
| ORM | Entity Framework Core (a configurar) |
| Autenticação | Própria, via JWT (a implementar — sem provedores prontos) |
| IDE | Visual Studio 2026 |

---

## Arquitetura

O projeto segue **Clean Architecture** (arquitetura em camadas), com as dependências sempre apontando para dentro — o `Domain` não conhece nenhuma outra camada.

```
ControleMultEstoque (solution)
 ├─ src
 │   ├─ ControleMultEstoque.Domain           → regras de negócio puras (entidades)
 │   ├─ ControleMultEstoque.Application      → casos de uso, interfaces, DTOs
 │   ├─ ControleMultEstoque.Infrastructure   → EF Core, repositórios, acesso a dados
 │   ├─ ControleMultEstoque.API              → Web API (controllers, autenticação JWT)
 │   └─ ControleMultEstoque.Web              → Razor Pages (consome a API via HTTP;
 │                                              interface exibida ao usuário como "StockMaster")
 └─ tests
     └─ ControleMultEstoque.Domain.Tests     → testes unitários do Domain
```

**Referências entre projetos:**
- `Application` → referencia `Domain`
- `Infrastructure` → referencia `Domain` e `Application`
- `API` → referencia `Application` e `Infrastructure`
- `Web` → **não referencia nenhum projeto do backend**; comunica-se somente via HTTP/HttpClient com a `API`, como dois sistemas independentes de verdade
- `Domain.Tests` → referencia `Domain`

Projetos de inicialização configurados no Visual Studio: `ControleMultEstoque.API` + `ControleMultEstoque.Web` (múltiplos projetos de inicialização).

---

## Progresso até o momento

### ✅ Estrutura da Solution
- [x] Solution `ControleMultEstoque` criada
- [x] 5 projetos criados (`Domain`, `Application`, `Infrastructure`, `API`, `Web`) + projeto de testes (`Domain.Tests`)
- [x] Referências entre projetos configuradas respeitando a Clean Architecture
- [x] Múltiplos projetos de inicialização (API + Web) configurados

### ✅ Domain — Entidades

| Entidade | Arquivo | Descrição |
|---|---|---|
| `Armazem` | `Entities/Armazem.cs` | Unidade de armazenamento física. Controla status (Ativo/EmManutencao/Inativo) e responsável. |
| `StatusArmazem` | `Entities/StatusArmazem.cs` | Enum de status do armazém. |
| `Produto` | `Entities/Produto.cs` | Cadastro de catálogo (SKU, nome, categoria, preço, quantidade mínima). Independente de armazém. |
| `ItemEstoque` | `Entities/ItemEstoque.cs` | Vínculo entre `Produto` + `Armazem`, com quantidade física, lote e validade. Movimentação apenas via `Adicionar()`/`Remover()`. |
| `Transferencia` | `Entities/Transferencia.cs` | Ordem de movimentação de estoque entre dois armazéns. Máquina de estados: `Pendente → EmTransito → Concluida` ou `Cancelada`. |
| `StatusTransferencia` | `Entities/StatusTransferencia.cs` | Enum de status da transferência. |

**Decisões de design aplicadas em todas as entidades:**
- Propriedades com `private set`; toda alteração de estado passa por métodos de comportamento (nunca por atribuição direta) — protege contra estados inválidos.
- Construtores com validação (`ArgumentException` para dados inválidos, `InvalidOperationException` para transições de estado não permitidas).
- Construtor `protected` vazio, exclusivo para o EF Core reconstruir objetos vindos do banco.
- `Transferencia` guarda apenas os IDs de produto/armazéns envolvidos — a execução efetiva da movimentação (chamando `Remover`/`Adicionar` nos `ItemEstoque` de origem e destino, dentro de uma transação) será responsabilidade de um caso de uso na camada `Application`, garantindo consistência.

### ⚠️ Pendência a investigar
- Um `git pull` recente não trouxe as entidades commitadas na sessão anterior (aparentemente só `Produto` chegou). Precisa verificar no GitHub se o push foi feito, se foi para a branch correta (`git branch -a`, `git status`, `git log --oneline`) antes de continuar commitando novo código por cima.

---

## Roadmap — o que falta fazer

### Domain (finalizar núcleo de regras)
- [ ] Entidade `Usuario` (necessária para autenticação própria — papéis: Administrador Geral, Responsável por Armazém)
- [ ] Value Objects, se necessário (ex: `Email`, `Senha`/hash)
- [ ] Interfaces de repositório (`IArmazemRepository`, `IProdutoRepository`, `IItemEstoqueRepository`, `ITransferenciaRepository`, `IUsuarioRepository`) — o "contrato" entre Domain/Application e Infrastructure
- [ ] Testes unitários das entidades já criadas (`ControleMultEstoque.Domain.Tests`)

### Application
- [ ] DTOs de entrada/saída
- [ ] Casos de uso (ex: `CriarTransferenciaUseCase`, `ConfirmarTransferenciaUseCase`, `GerarSugestaoReposicaoUseCase`)
- [ ] Interfaces de serviços (hash de senha, geração de token JWT)
- [ ] Validações de aplicação (ex: FluentValidation ou Data Annotations)

### Infrastructure
- [ ] `DbContext` (EF Core) e mapeamento das entidades (Fluent API)
- [ ] Migrations iniciais + conexão com SQL Server
- [ ] Implementação concreta dos repositórios
- [ ] Implementação do hash de senha (ex: BCrypt/PBKDF2) e geração de JWT
- [ ] Log de auditoria (interceptor ou decorator registrando quem/o quê/quando)

### API
- [ ] Autenticação própria: registro, login, emissão de token JWT, refresh token
- [ ] Autorização por papel (`[Authorize(Roles = "...")]`)
- [ ] Controllers: Armazéns, Produtos, Estoque, Transferências, Reposição, Auditoria
- [ ] Validação de entrada e tratamento de erros consistente (middleware de exceções)
- [ ] Documentação via Swagger/OpenAPI

### Web (Razor Pages)
- [ ] Tela de Login/Registro (consumindo a API)
- [ ] Dashboard (visão consolidada, alertas)
- [ ] Gestão de Armazéns
- [ ] Estoque (visão geral por armazém)
- [ ] Transferências (criação e acompanhamento de status)
- [ ] Reposição (sugestões de compra/transferência)
- [ ] Log de Auditoria (com filtros)
- [ ] Aplicação da identidade visual definida (dark mode, preto/verde esmeralda/vermelho, estilo pill)

### Transversal / Requisitos do regulamento
- [ ] Testes automatizados cobrindo regras de negócio centrais (unitários + integração)
- [ ] Tratamento de condição de corrida na Transferência (transação de banco)
- [ ] README final com instruções de execução (instalação, banco, variáveis de ambiente)
- [ ] `.gitignore` revisado, licença, organização de pastas
- [ ] Commits semânticos (Conventional Commits) e uso coerente de branches
- [ ] Vídeo de demonstração (até 5 min) para entrega

---
