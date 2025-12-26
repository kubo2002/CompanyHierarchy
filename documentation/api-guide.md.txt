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

## Konvencie

API používa REST štýl komunikácie
- Endpointy používajú HTTP metódy GET, POST, PUT a DELETE
- Názvy endpointov sú v množnom čísle (napr. `/api/employees`)
- Vstupné a výstupné dáta sú prenášané vo formáte JSON

## Dokumentácia endpointov

Podrobný popis jednotlivých endpointov je dostupný v nasledujúcich dokumentoch:

- [Nodes API](nodes.md)
- [Employees API](employees.md)