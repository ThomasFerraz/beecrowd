
# Sales API - Beecrowd Developer Test

Este projeto é uma API desenvolvida como parte do teste técnico para a vaga de desenvolvedor.  
A API foi construída em .NET 8, utilizando MediatR, AutoMapper, Ocelot Gateway, e dockerizada para facilitar a execução.

---

## 🚀 Como Rodar o Projeto

### 1. Clonando o Repositório

```bash
git clone https://github.com/thomasferraz/beecrowd.git
cd seu-repo
```

### 2. Rodando com Docker

- Crie a rede externa (se ainda não existir):

```bash
docker network create evaluation-network
```

- Suba os containers:

```bash
docker-compose up --build
```

Acesse a API através do Gateway:

- Produtos: `http://localhost:7777/products`
- Vendas: `http://localhost:7777/sales`

A documentação Swagger estará disponível nos próprios serviços, se necessário.

### 3. Rodando Localmente (Visual Studio)

- Abra o arquivo `SalesApi.sln` no Visual Studio.
- Defina o projeto **SalesApi** como projeto de inicialização.
- Execute o projeto (`F5`).
- Acesse o Swagger em:

```
https://localhost:{porta}/swagger/index.html
```

---

## 📦 Estrutura do Projeto

```plaintext
root/
├── docker-compose.yml
├── src/
│   ├── SalesApi/
│   │   ├── Dockerfile
│   │   └── SalesApi.csproj
│   ├── Gateway/
│   │   ├── Dockerfile
│   │   └── ocelot.json
│   ├── SalesApi.Application/
│   ├── SalesApi.Domain/
│   ├── SalesApi.Infrastructure/
├── tests/
│   └── SalesApi.Tests/
```

---

## ✨ Funcionalidades Implementadas

- **Products API**
  - `GET /products` → Listar produtos
  - `POST /products` → Criar novo produto
  - Evento `ProductCreated` logado no Console

- **Sales API**
  - `GET /sales` → Listar vendas
  - `POST /sales` → Criar venda (aplicando regras de desconto)
  - `PUT /sales/{id}` → Atualizar venda
  - `DELETE /sales/{id}` → Cancelar venda
  - Eventos `SaleCreated` e `SaleCancelled` logados no Console

- **Regras de Negócio de Desconto:**
  - 4+ produtos → 10% desconto
  - 10-20 produtos → 20% desconto
  - Mais de 20 produtos → Venda inválida (retorna erro 400)

- **Tratamento de Erros**
  - Respostas padronizadas em caso de erro:
  
```json
{
  "type": "BadRequest",
  "error": "Invalid Sell",
  "detail": "You cannot buy more than 20 pieces of same item"
}
```

- **Docker e Ocelot Gateway**
  - Gateway configurado para rotear `/products` e `/sales`.
  - Portas utilizadas: `8090` para SalesAPI e `7777` para Gateway.

---

## 🛠️ Tecnologias Utilizadas

- .NET 8
- MediatR
- AutoMapper
- Ocelot Gateway
- Docker / Docker Compose
- xUnit / Bogus / NSubstitute para testes

---

## 📢 Observação

- A persistência de dados é feita **em memória** para fins de teste, conforme instruções.
- Todas as operações seguem o padrão `{ data, status, message }` de resposta.

---
