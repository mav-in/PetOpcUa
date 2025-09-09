# README
PET project processing example of visualization of OPC UA data in WPF project.

**������ API (��������)**
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
**������ API (Docker)**

```
docker compose up --build
# ���������: http://localhost:8080/api/...
``` 

**������ WPF**

1.  � `OpcClient` ���������� ���������� ��������� `OPC_API_BASE` (��� �������� � `App.xaml.cs`), ��-��������� `http://localhost:8080`.
    
2.  ��������� ������ `OpcClient`.
    
3.  ������� **Refresh** ����� �������� ������, ��� **Auto refresh**, ��� ���������� � �������������� ������. �������� �������: ������� � ���������� ��������, ����� `Tank1.TrendLevel`, ������� `Tank1.Level` �� ��������, � ������.
    

**UI � ������**

-   Grid: ����������� `DataGrid` (WPF).

-   ������: `OxyPlot.Wpf`.

-   MVVM: `CommunityToolkit.Mvvm`.
    

**�����������**

-   ������� � ����� �������: Domain (������/��������), Api (���������/DI), Wpf (MVVM, ����������� code-behind), Tests.

-   ����������� - ����������� ASP.NET Core, DI, async/await, ������ ����� `CancellationToken`.

-   �����������/�������� - ISO 8601 UTC, � UI ������������ ��� ����.

