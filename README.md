# McDonaldsGame
**ASP.NET Core API + WPF клієнт + Docker + SQL Server**

Усе працює на `http://localhost:8080`
---
## Швидкий старт (3 кроки)
### 1. Клонуйте репозиторій
```bash
git clone https://github.com/YOUR-USERNAME/McDonaldsApi.git
cd McDonaldsApi
```
### 2. Запустіть через Docker (Рекомендовано)
Це запустить API та базу даних SQL Server у контейнерах.
```bash
docker-compose up --build
```
**API:** http://localhost:8080/swagger

**База даних:** localhost,1433 (логін: sa, пароль: YourPass!123)
### 3. Запустіть WPF клієнт (Локально)
Клієнт автоматично підключиться до API, запущеного в Docker.
```bash
cd McDonalds.Wpf
dotnet run
```
---
### Структура проекту
```bash
textMcDonaldsApi/
├── McDonalds.Api/          ← ASP.NET Core API (C#)
├── McDonalds.Data/         ← EF Core, DbContext + Міграції
├── McDonalds.Contracts/    ← DTO (Data Transfer Objects)
├── McDonalds.Wpf/          ← WPF клієнт (.NET)
│
├── docker-compose.yml      ← Docker (конфігурація API + SQL Server)
├── .dockerignore           ← Ігнорування /bin, /obj для Docker
├── README.md               ← Ви тут
```

## Запуск без Docker (Локально)
### 1. База даних (SQL Server)
Встановіть SQL Server Express (або іншу версію).

Змініть ConnectionStrings у файлі McDonalds.Api/appsettings.json на ваш локальний рядок підключення.

Запустіть міграції, щоб створити базу даних та таблиці: 
```bash
cd McDonalds.Api
dotnet ef database update --project ../McDonalds.Data
```

### 2. API
```bash
cd McDonalds.Api
dotnet run --launch-profile http
```
Доступ: http://localhost:8080/swagger
### 3. WPF клієнт
Переконайтеся, що ApiUrl у файлі McDonalds.Wpf/appsettings.json вказує на http://localhost:8080/swagger.
```bash
cd McDonalds.Wpf
dotnet run
```

## Налаштування
appsettings.json (WPF)
```json
{
  "ApiUrl": "http://localhost:8080/swagger"
}
```

## Технології
| Компонент       | Технологія                     |
|-----------------|--------------------------------|
| Backend         | ASP.NET Core 8.0               |
| База Даних      | SQL Server 2022 (у Docker)     |
| ORM             | Entity Framework Core 8        |
| Клієнт          | WPF (.NET 8)                   |
| Контейнери      | Docker + docker-compose        |
| API Docs        | Swagger (Swashbuckle)          |
| Маппінг         | AutoMapper                     |

## Вирішення проблем
### API не запускається або впав?
```bash
docker logs mcdonaldsapi-api-1 -f
```
### База даних не запускається або "unhealthy"?
```bash
docker logs mcdonaldsapi-db-1 -f
```
### Помилка There is already an object named... або SocketException?

Це означає, що база даних у "брудному" стані або не встигла запуститися.

Очистіть том Docker та почніть спочатку:
```bash
1. Зупинити контейнери ТА видалити том (-v)
docker-compose down -v

2. Запустити збірку знову
docker-compose up --build
```
### WPF не підключається?
Перевірте, що ApiUrl в McDonalds.Wpf/appsettings.json точно: `http://localhost:8080/swagger`
