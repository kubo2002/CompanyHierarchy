# Zamestnanci

##  CreateEmployee

`POST /api/createEmployee`

Endpoint slúži na vytvorenie nového zamestnanca v systéme.
Každý zamestnanec je jednoznačne identifikovaný pomocou atribútu `id`
typu `GUID`, ktorý slúži ako technický identifikátor a nie je závislý
od databázového generovania identít.

Použitie `GUID` umožňuje jednoznačnú identifikáciu zamestnanca
naprieč celým systémom a zjednodušuje prípadnú integráciu
s inými časťami aplikácie alebo externými systémami.

Atribút `email` slúži ako kontaktný údaj zamestnanca a nie je použitý
ako primárny identifikátor, aby sa predišlo problémom pri jeho zmene
alebo duplicitnom výskyte.

### Request body

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone" : "+123456789",
  "academicTitle": "Ing."
}
```

### Response
```json
{
  "success": true,
  "message": "Employee created",
  "data": "38abbfd0-e7c0-4a9b-a232-82f5c33cc31e"
}
```

## UpdateEmployee

`PUT /api/employees/update`

Endpoint slúži na úpravu údajov existujúceho zamestnanca.
Zamestnanec je identifikovaný pomocou atribútu `employeeId` typu `GUID`,
ktorý predstavuje stabilný technický identifikátor zamestnanca.

Endpoint podporuje čiastočnú aktualizáciu údajov zamestnanca.
Aktualizované sú iba tie atribúty, ktoré sú v požiadavke vyplnené.
Nevyplnené hodnoty ostávajú nezmenené.

Identifikátor zamestnanca (`employeeId`) nie je možné meniť,
keďže slúži ako jednoznačný identifikátor naprieč celým systémom.
Tento prístup zabraňuje nekonzistentným referenciám na zamestnanca
v iných častiach aplikácie.

### Request body

```json
{
  "employeeId": "guid",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": null,
  "academicTitle": "Ing."
}
```

### Response
```json
{
  "success": true,
  "message": "Employee updated",
  "data": "38abbfd0-e7c0-4a9b-a232-82f5c33cc31e"
}
```

## DeleteEmployee
`DELETE /api/employees/{employeeId}`

Endpoint slúži na odstránenie zamestnanca zo systému na základe jeho
jednoznačného identifikátora `id` typu `GUID`.

Odstránením zamestnanca dôjde k zrušeniu všetkých jeho personálnych
väzieb v rámci systému, napríklad priradenia k oddeleniu alebo roly
manažéra. Samotná organizačná štruktúra týmto zásahom nie je ovplyvnená.

Použitie technického identifikátora `GUID` zabezpečuje jednoznačné
a bezpečné odstránenie konkrétneho zamestnanca bez rizika
neúmyselného zásahu do iných entít.

### Parametre

- `employeeId` – identifikátor zamestnanca, ktorý má byť odstránený

### Response

```json
{
  "success": true,
  "message": "Employee deleted successfully",
  "data": null
}
```

## AssignEmployeeToDepartment

`POST /api/employees/assign-employee`

Endpoint slúži na priradenie zamestnanca k oddeleniu (Department).
Priradenie reprezentuje pracovné zaradenie zamestnanca v rámci
organizačnej štruktúry firmy.

Vzťah medzi zamestnancom a oddelením je riešený samostatným endpointom,
aby bolo možné nezávisle spravovať osobné údaje zamestnanca
a jeho organizačné zaradenie.

V systéme platí pravidlo, že zamestnanec môže byť v danom čase
priradený **najviac k jednému oddeleniu**.

### Request body
```json
{
  "nodeId": "guid",
  "employeeId": "guid"
}
```
### Response

```json
{
  "success": true,
  "message": "Employee assigned to department successfully",
  "data": null
}
```

## UnassignEmployeeFromDepartment
`POST /api/employees/{employeeId}/unassign`

Endpoint slúži na odobratie zamestnanca z oddelenia (Department).
Po vykonaní operácie nebude mať zamestnanec priradené žiadne
organizačné zaradenie.

Tento endpoint umožňuje flexibilnú správu organizačných vzťahov,
napríklad pri presune zamestnanca, dočasnom vyradení z oddelenia
alebo ukončení pracovného zaradenia.

Odobratie zamestnanca z oddelenia nemá vplyv na existenciu samotného
zamestnanca ani na štruktúru oddelení v systéme.

### Parametre

- `employeeId` – identifikátor zamestnanca, ktorého chceme odobrať z manažovania.

### Response

```json
{
  "success": true,
  "message": "Employee unassigned from department successfully",
  "data": null
}
```

## DepartmentEmployees
`POST /api/departments/{departmentId}/employees`

Endpoint slúži na získanie zoznamu zamestnancov priradených
k konkrétnemu oddeleniu (Department).

Výsledkom je plochý zoznam zamestnancov, ktorí sú aktuálne
organizačne zaradení v danom oddelení. Endpoint je určený najmä
na zobrazenie prehľadu zamestnancov v rámci oddelenia,
napríklad v používateľskom rozhraní alebo pri administratívnych operáciách.

Endpoint nevracia hierarchickú štruktúru, ale len zoznam
zamestnancov priradených k danému oddeleniu.

### Parametre

- `id` – identifikátor oddelenia, ktorého zamestnanci majú byť získaní

### Response

```json
{
  "success": true,
  "message": "Employees loaded successfully",
  "data": [
    {
      "id": "f84a88db-1e1c-463e-b288-eef9db8a08aa",
      "academicTitle": "",
      "firstName": "Peter",
      "lastName": "Dept",
      "email": "peter-534f43eb-9ad5-40f8-8c99-d2aef7fa96a2@test.sk"
    }
  ]
}
```