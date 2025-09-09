# README

**Запуск API (локально)**
```
dotnet build OpcMock.sln
set OPC_MOCK_PATH=./app/data/opc_mock.json # Windows
export OPC_MOCK_PATH=./app/data/opc_mock.json # Linux/Mac
dotnet run --project ./OpcApi/OpcApi.csproj --urls "http://localhost:8080"
# Swagger: http://localhost:8080/swagger
# GET http://localhost:8080/api/opc/last-read
# GET http://localhost:8080/api/opc/tags?status=Good
# GET http://localhost:8080/api/opc/summary
```
**Запуск API (Docker)**

```
docker compose up --build
# эндпоинты: http://localhost:8080/api/...
``` 

**Запуск WPF**

1.  В `OpcClient` установите переменную окружения `OPC_API_BASE` (или измените в `App.xaml.cs`), по-умолчанию `http://localhost:8080`.
    
2.  Запустите проект `OpcClient`.
    
3.  Нажмите **Refresh** чтобы обновить данные, или **Auto refresh**, для обновления в автоматическом режиме. Доступны вкладки: таблица с подсветкой качества, тренд `Tank1.TrendLevel`, текущий `Tank1.Level` со статусом, и сводку.
    

**UI и пакеты**

-   Grid: стандартный `DataGrid` (WPF).

-   График: `OxyPlot.Wpf`.

-   MVVM: `CommunityToolkit.Mvvm`.
    

**Архитектура**

-   Монолит в одном решении: Domain (модель/маппинги), Api (эндпоинты/DI), Wpf (MVVM, минимальный code-behind), Tests.

-   Логирование - стандартное ASP.NET Core, DI, async/await, отмена через `CancellationToken`.

-   Локализация/культура - ISO 8601 UTC, в UI отображается как есть.
