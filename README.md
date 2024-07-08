# Buy.Net - Ecommerce API

Buy.Net é uma API de ecommerce desenvolvida em .NET, projetada para gerenciar usuários, categorias, produtos e pedidos. Este repositório contém o código-fonte e a configuração necessária para rodar a aplicação utilizando Docker.

<img src="https://github.com/murilloliveiraz/Buy.NET/blob/main/images/image.png">

## Tecnologias Utilizadas

- .NET
- Domain-Driven Design (DDD)
- Entity Framework Core com PostgreSQL
- AutoMapper para mapeamento de objetos
- Autenticação JWT
- Exceções personalizadas
- Docker e Docker Compose

## Como Usar

1. Clone o repositório:
    ```sh
    git clone https://github.com/seu_usuario/buy-net.git
    ```
2. Navegue até o diretório do projeto:
    ```sh
    cd buy-net
    ```
3. Configure a string de conexão com o banco de dados PostgreSQL no arquivo `appsettings.json`.
4. Execute o Docker Compose para iniciar a aplicação e o banco de dados:
    ```sh
    docker-compose up -d
    ```
5. Acesse a aplicação em `http://localhost:5000`.

## Estrutura do Projeto

```plaintext
Buy-NET/
│
├── Domain/
│   ├── Models/
│   ├── Exceptions/
│   ├── Services/
│   └── IRepositories/
│
├── Infrastructure/
│   └──  Data/
│        ├── Contexts/
│        ├── Mappings/
│        ├── Migrations/
│        ├── Repositories/
│        └── Services/
│
├── Application/
│   ├── Contracts/
│   └── Mappers/
│
├── API/
│   └── Controllers/
├── Buy-NET.API.csproj
├── Dockerfile
├── Program.cs
├── app.settings.json.cs
├── Dockerfile
└── docker-compose.yml
```

## Segurança

Utilizamos autenticação JWT para garantir que os dados dos usuários estejam sempre protegidos.

## Documentação Completa

A documentação da API está disponível via Swagger para facilitar a integração e uso. Acesse `http://localhost:8000/` após iniciar a aplicação.

## Contribua

Confira o projeto no GitHub e contribua! Feedbacks e sugestões são sempre bem-vindos.

### Links

- Docker Hub: [[link_da_imagem_no_docker_hub]](https://hub.docker.com/repository/docker/murilloxz/buynet/general)
- GitHub: [[link_do_projeto_no_github]](https://github.com/murilloliveiraz/Buy.NET)

---

Agradecemos a todos que apoiaram no desenvolvimento deste projeto e estamos animados para os próximos desafios! 🚀
