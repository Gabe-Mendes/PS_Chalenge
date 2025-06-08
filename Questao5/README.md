# Sistema de Movimentação de Conta Corrente

Implementação de uma solução API RESTful para gerenciamento de movimentações financeiras em contas correntes, com suporte a operações de crédito e débito, consulta de saldo e mecanismo de idempotência para garantir a consistência das transações.

## Arquitetura

O projeto possui conceitos de Clean Architecture:

### Domain
Entidades de negócio, enum e mensagens padrão do sistema:
- **Entities**: `ContaCorrente`, `Movimento`, `Idempotencia`
- **Enumerators**: `TipoMovimento` (Crédito/Débito)
- **Language**: Mensagens de erro padronizadas

### Application
Lógica da aplicação usando padrão CQRS (Command Query Responsibility Segregation) com MediatR:

### Infrastructure
Persistência de dados e serviços:
- **Database**: Repositórios, queries e commands para acesso ao banco de dados
- **Services**: Controllers, middlewares e extensões
- **Sqlite**: Configuração e bootstrap do banco de dados SQLite
- **Swagger**: Configuração da documentação da API
- **Converters**: Conversores personalizados para serialização/deserialização

## Tecnologias Utilizadas

- **.NET 6**: Framework de desenvolvimento
- **MediatR**: Implementação do padrão Mediator para CQRS
- **Dapper**: Micro ORM para acesso ao banco de dados
- **SQLite**: Banco de dados relacional leve
- **Swagger**: Documentação da API

## Funcionalidades Principais

### 1. Criação de Movimentos Financeiros
- Endpoint: `POST /api/movimento/criar`
- Permite registrar operações de crédito ou débito em uma conta corrente
- Validações:
  - Existência e atividade da conta corrente
  - Tipo de movimento válido (crédito/débito)
  - Valor positivo para o movimento

### 2. Consulta de Saldo
- Endpoint: `GET /api/movimento/saldo/{idContaCorrente}`
- Retorna o saldo atual da conta, nome do titular e número da conta
- Calcula o saldo com base nos movimentos de crédito e débito registrados

### 3. Mecanismo de Idempotência
- Implementado via middleware e cabeçalho `X-Chave-Idempotencia`
- Garante que requisições duplicadas não gerem movimentos duplicados
- Armazena o resultado da primeira execução e o retorna para chamadas subsequentes com a mesma chave
- Utiliza dois middlewares especializados:
  - `IdempotenciaMiddleware`: Verifica se a requisição já foi processada
  - `ResponseCapturaMiddleware`: Captura e armazena o resultado para uso futuro

## Padrões de Projeto Implementados

- **CQRS**: Separação entre comandos (escrita) e consultas (leitura)
- **Repository Pattern**: Abstração do acesso a dados
- **Mediator**: Desacoplamento entre componentes usando MediatR
- **Middleware**: Processamento de requisições HTTP em pipeline
- **Dependency Injection**: Injeção de dependências para acoplamento fraco


## Fluxo de Processamento

1. A requisição HTTP é recebida e passa pelo pipeline de middlewares (A lógica de idempotência fica isolada do código de negócio)
2. O `IdempotenciaMiddleware` verifica se a requisição já foi processada:
   - Se já foi processada, retorna o resultado armazenado e interrompe o fluxo
   - Se é nova, permite que a requisição continue
3. O controller recebe a requisição, extrai a chave de idempotência e a adiciona ao objeto de requisição
4. O controller encaminha a requisição para o handler apropriado via MediatR
5. O handler executa a lógica de negócio e acessa os repositórios
6. Os repositórios interagem com o banco de dados SQLite
7. O `ResponseCapturaMiddleware` captura a resposta e a armazena para futuras verificações de idempotência
8. O resultado é retornado ao cliente em formato JSON

## Tratamento de Erros

O sistema implementa tratamento de erros padronizado, retornando mensagens claras para situações como:
- Conta corrente inexistente
- Conta corrente inativa
- Tipo de movimento inválido
- Valor de movimento inválido
- Erros de processamento de movimento

## Segurança

- Validação de entradas para prevenir injeção de SQL
- Uso de parâmetros em consultas SQL via Dapper
- Tratamento adequado de erros sem exposição de detalhes sensíveis
