# LibraHub - Sistema de Gestão de Biblioteca

Aplicação web para gestão de uma biblioteca, permitindo gerir livros, autores, géneros e empréstimos.

## Tecnologias Utilizadas

- ASP.NET Core MVC 5.0
- Entity Framework Core
- SQL Server (LocalDB)
- ASP.NET Core Identity (Autenticação e Autorização)
- Bootstrap 4
- Font Awesome 5

## Funcionalidades

- **Autenticação:** Login, Registo, Logout, Alteração de palavra-passe e dados do perfil
- **Gestão de Géneros:** CRUD completo (apenas Admin)
- **Gestão de Autores:** CRUD completo com upload de imagem (apenas Admin)
- **Gestão de Livros:** CRUD completo com upload de imagem e associação a autor/género (apenas Admin)
- **Empréstimos:** Leitores podem requisitar livros disponíveis; Admins gerem os empréstimos ativos
- **Controlo de Stock:** Atualização automática do stock ao criar, devolver ou apagar empréstimos
- **Proteção de páginas:** Acesso restrito por roles (Admin e Reader)

## Como Executar

1. Clonar o repositório
2. Abrir a solução no Visual Studio 2019/2022
3. A base de dados utiliza **LocalDB** e será criada automaticamente ao executar a aplicação (via Migrations)
4. Executar o projeto (F5 ou Ctrl+F5)

## Configuração da Base de Dados

A connection string está definida no ficheiro `appsettings.json` e utiliza SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LibraHub;Trusted_Connection=True;"
}
```

As migrations são aplicadas automaticamente ao iniciar a aplicação. É também criado um utilizador Admin por defeito.

## Estrutura do Projeto

- **Data/Entities** — Entidades do modelo (Genre, Author, Book, Loan, User)
- **Data** — Repositórios (Generic e específicos) e DataContext
- **Controllers** — Controladores MVC
- **Models** — ViewModels
- **Helpers** — Serviços auxiliares (UserHelper, ImageHelper, ConverterHelper)
- **Views** — Vistas organizadas por controlador
