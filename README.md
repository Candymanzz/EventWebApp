# EventWebApp

Веб-приложение для управления событиями и мероприятиями.

## Требования

- Docker
- Docker Compose

## Запуск приложения

1. Клонируйте репозиторий:
```bash
git clone https://github.com/Candymanzz/EventWebApp
cd EventWebApp
```

2. Запустите приложение с помощью Docker Compose:
```bash
docker-compose up -d
```

Приложение будет доступно по следующим адресам:
- Фронтенд: http://localhost:3000
- Бэкенд API: http://localhost:8080
- Swagger UI: http://localhost:8080/swagger

## Остановка приложения

Для остановки приложения выполните:
```bash
docker-compose down
```

## Структура проекта

- `event-client/` - React фронтенд приложение
- `EventWebApp.WebAPI/` - ASP.NET Core бэкенд
- `EventWebApp.Core/` - Основные модели и интерфейсы
- `EventWebApp.Infrastructure/` - Реализация репозиториев и сервисов
- `EventWebApp.Application/` - Бизнес-логика и DTO

## Технологии

- Frontend:
  - React
  - TypeScript
  - Bootstrap
  - Axios

- Backend:
  - ASP.NET Core
  - Entity Framework Core
  - PostgreSQL
  - JWT Authentication

- Infrastructure:
  - Docker
  - Nginx
  - PostgreSQL 
