# CRUD de Usuários - Minimal API

**Introdução**

*Esta API RESTful foi desenvolvida utilizando o .NET Minimal API para gerenciar usuários em um banco de dados PostgreSQL. A autenticação é realizada através de tokens JWT para garantir a segurança das operações.*
- criada com o intuito de aprimorar conhecimentos em Docker e autenticação de usuários.

**Pré-requisitos**

* Docker: Certifique-se de que o Docker está instalado e em execução.
* .NET 8 SDK: Certifique-se de que o .NET SDK está instalado.

## Tecnologias Utilizadas

- **Entity Framework Core**: ORM para interação com o banco de dados.
- **PostgreSQL**: Sistema de gerenciamento de banco de dados relacional.
- **Docker**: Ferramenta de containerização para desenvolvimento e implantação.

## CQRS (Command Query Responsibility Segregation)
- Este projeto segue o padrão CQRS, uma abordagem de arquitetura que separa a responsabilidade de manipulação de comandos (alteração de estado) da responsabilidade de consultas (leitura de estado).
 Essa separação pode proporcionar uma melhor escalabilidade, flexibilidade e manutenção.

## Funcionalidades do Projeto

A API oferece o serviço de cadastro de usuários, sendo eles: **POST**, **PUT**, **GET**, **DELETE**

## Dependências

```bash
- **Auto Mapper**
- **Fluent Validation**
- **MediatR**
- **Entity Framework Core**
- **Entity Framework Core - Design**
- ***Entity Framework Core - PostgreSQL**
- **JWT Bearer Authentication**
- **JWT Tokens**
- **Dependency Injection**
```

Para configurar o Docker Compose, adicione o seguinte código ao seu arquivo `docker-compose.yaml`:

```yaml
version: '3.8'
services:
  postgres:
    image: postgres:latest
    stdin_open: true
    tty: true
    ports:
      - "5432:5432"
    container_name: nome_container
    restart: always
    environment:
      POSTGRES_USER: seu_usuario
      POSTGRES_PASSWORD: sua_senha
      POSTGRES_DB: nome_banco
    volumes:
      - ./db/:/var/lib/postgresql/data
```

## Geração de Migrações

### Para realizar a geração de migrações e atualizar o banco de dados, siga os passos abaixo:

## Passo 1: Abra um terminal na raiz do projeto.

## Passo 2: Execute o seguinte comando para criar uma nova migração:

```bash
dotnet ef migrations add NomeDaMigracao

Substitua "NomeDaMigracao" pelo nome desejado para a migração.
```
## Passo 3: Execute o seguinte comando para aplicar a migração ao banco de dados:

```bash
dotnet ef database update
```

