##  Úvod

Tento dokument poskytuje prehľad používania REST API aplikácie Company Management.
REST API slúži na správu organizačnej štruktúry firmy a zamestnancov.

## Base URL
Všetky endpointy API sú dostupné na nasledujúcej adrese:

http://localhost:5235/scalar

## Error responses
API používa štandardné HTTP stavové kódy:

- 200 – požiadavka bola úspešne spracovaná
- 400 – validačná chyba vstupných údajov
- 404 – požadovaný zdroj neexistuje
- 409 - požadovany zdroj už exsituje
- 500 – interná chyba servera
---
## Príklad postupu vytvárania organizačnej hierarchie
### 1. Vytvorenie Company uzla `CreateNode`

Company uzol je koreňom celej hierarchie a nemá rodičovský uzol (`parentId` = null).
```json
{
  "name": "Company A",
  "code": "COMP",
  "type": 1,
  "parentId": null
}
```

Odpoveď obsahuje `nodeId` vytvoreného Company uzla. Toto ID je potrebné si uložiť
pre ďalšie kroky.

---
### 2. Získanie ID Company uzla `ShowNodesByType`
Ak ID nebolo uložené z odpovede, je možné ho získať pomocou endpointu
na zobrazenie uzlov podľa typu:

Query Parameters
`types` = 1

Z odpovede použite hodnotu id uzla Company A.
---
### 3. Vytvorenie Division uzla (potomok Company) `CreateNode`

Division uzol musí mať parentId odkazujúci na existujúci Company uzol.

```json
{
  "name": "Division A",
  "code": "DIV",
  "type": 2,
  "parentId": "<CompanyId>"
}
```
---
### 4.  Získanie ID Division uzla `ShowNodesByType`

Query Parameters
`types` = 2

---

### 5. Vytvorenie Project uzla (potomok Division) `CreateNode`

Project uzol musí mať parentId odkazujúci na existujúci Division uzol.

```json
{
  "name": "Project A",
  "code": "PROJ",
  "type": 3,
  "parentId": "<DivisionId>"
}
```
---
### 6. Získanie ID Project uzla `ShowNodesByType`

Query Parameters
`types` = 3

---

### 7. Vytvorenie Department uzla (potomok Project) `CreateNode`

Department uzol musí mať parentId odkazujúci na existujúci Project uzol.

```json
{
  "name": "Department A",
  "code": "DEPA",
  "type": 4,
  "parentId": "<ProjectId>"
}
```
---

## Cyklus v hierarchii
Každý uzol, môže mať ľubovoľne mnoho potomkov. V projekte je kladený dôraz na jasnú následnosť uzlov podľa pravidla `Company -> Division -> Project -> Department`. Vytváranie a aktualizácia uzlov nedovoľuje zmenu `parentId` na ľubovoľný uzol z hierarchie. `parentId` je možné zmeniť len na uzol, ktorý je oficiálne priamym predchodcom aktuálneho uzla, napríklad `Project` môže pomocou `parentId` odkazovať len na uzly typu **Division**.

## Dokumentácia endpointov

Podrobný popis jednotlivých endpointov je dostupný v nasledujúcich dokumentoch:

- [Nodes API](nodes.md)
- [Employees API](employees.md)
