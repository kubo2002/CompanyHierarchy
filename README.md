# Company Management REST API

## Prehľad projektu
Tento projekt predstavuje REST API určené na správu organizačnej štruktúry firmy,
uzlov organizácie (company units) a zamestnancov. Aplikácia umožňuje vytváranie
hierarchie organizačných jednotiek, priraďovanie zamestnancov a správu vedúcich pozícií.

---

## Dokumentácia

Podrobná používateľská príručka REST API je dostupná v samostatných
Markdown dokumentoch v priečinku `documentation`.

- [Prehľad API](documentation/api-guide.md)
- [Nodes API](documentation/nodes.md)
- [Employees API](documentation/employees.md)

## Použité technológie

- .NET 10 (ASP.NET Core)
- .NET SDK 8 (ASP.NET Core pre TeaPie ) 
- SQL Server
- Docker (len databáza)
- Scalar (API dokumentácia)
- TeaPie (HTTP testovanie API endpointov)
- xUnit (Unity testy)

## Požiadavky
- .NET 10
- .NET SDK 8.0 
- Docker
- SQL Server Management Studio alebo Azure Data Studio

## Štruktúra projektu
- `CompanyManagement` – API controllery a konfigurácia aplikácie
- `CompanyManagement.Application` – aplikačné use-cases a DTO objekty
- `CompanyManagement.Domain` – doménové entity a biznis logika
- `CompanyManagement.Infrastructure` – databázové repository
- `database` – SQL skripty na inicializáciu databázy
- `Tests` – unit a integračné testy
- `TeaPieTests` – HTTP testy API

---

## Nastavenie databázy

Projekt využíva SQL Server spustený v Docker kontajneri.
Databáza a všetky tabuľky sú vytvorené manuálne pomocou jedného SQL skriptu.

### 1. Spustenie databázy
```bash
docker-compose up -d
```

### 2. Pripojenie na SQL Server
- Server: `localhost,1433`
- Používateľ: `sa`
- Heslo: `yourStrong(!)Password`
- Server Certificate: `True`
Connection string : 
```bash
Server=localhost,1433;User Id=sa;Password=yourStrong(!)Password;Encrypt=True;TrustServerCertificate=True;
```

### 3. Inicializácia databázy
- Otvorte súbor `database/init.sql`
- Spustite skript v SQL manageri
---
## Spustenie aplikácie

###  1. Spustenie projektu v CLI
```bash
dotnet run --project CompanyManagement
```
###  2. Otvorte API dokumentáciu
```bash
http://localhost:5235/scalar
```
---
## Testovanie

### Unit testy (xUnit)
```bash
dotnet test
```

### HTTP testy
V priečinku `TeaPieTests` sú pripravené HTTP súbory pre testovanie API pomocou nástroja **TeaPie**.

- **Inštalácia TeaPie (ak ešte nie je nainštalované)**
```bash
dotnet tool install --global teapie
teapie --version
```
- **Spustenie TeaPie testov v priečinku `./TeaPieTests`**

```bash
teapie init
teapie test .
```
---
