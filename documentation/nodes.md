# Organizačná štruktúra

## CreateNode
`POST /api/nodes`

Endpoint slúži na vytvorenie nového uzla organizačnej štruktúry.

Su dostupné nasledovne typy uzlov, ktore je možné vytvoriť : 

- Company = 1
- Division = 2
- Project = 3
- Department = 4

Každý uzol je jednoznačne identifikovaný pomocou atribútu `id`,
ktorý je typu `GUID`. Použitie `GUID` zabezpečuje globálnu jedinečnosť
identifikátorov a nezávislosť od databázovej implementácie.

Atribút `code` slúži ako doplnkový identifikátor uzla.
V súčasnej verzii aplikácie nemá špecifický funkčný význam,
avšak je pripravený na budúce využitie, napríklad v používateľskom rozhraní
alebo pri integrácii s externými systémami.

`parentId` v sebe nesie informáciu o rodičovskom uzle. Uzol `Company` má `parentId` nastavený na `null` kežde sám o sebe je koreňom hierarchie (otcom všetkých uzlov v hierarchii.)

Každý novo vytvorený uzol môže mať `parentId` odkazujúci na uzol, ktorého `type` odpovedá logickej postupnosti uzlov v hierarchii. Napríklad nemôžem vytvoriť uzol Department, ktorý bude priamym potomkom uzla Company ale musím dodržať správnu postupnosť uzlov Company -> Division -> Project -> Department.
### Request body
```json
{
  "name": "Firma A",
  "code": "FA",
  "type": 1,
  "parentId": "guid | null"
}

```

## UpdateNode
`PUT /api/nodes/update`

Endpoint slúži na úpravu existujúceho uzla organizačnej štruktúry.
Uzol je identifikovaný pomocou atribútu `nodeId`, ktorý je typu `GUID`
a slúži ako primárny technický identifikátor uzla.

Endpoint podporuje čiastočnú aktualizáciu údajov uzla.
Aktualizované sú iba tie atribúty, ktoré sú v požiadavke vyplnené.
Nevyplnené hodnoty ostávajú nezmenené.

Pri zmene rodičovského uzla (`parentId`) je vykonaná kontrola hierarchických
pravidiel, aby bola zachovaná platná štruktúra organizácie.
Nie je možné priradiť uzol k rodičovi s nekompatibilným typom
(napr. Department priamo pod Division).

### Request body
```json
{
  "nodeId": "guid",
  "name": "Updated node name",
  "code": "NEW_CODE",
  "parentId": "guid | null"
}
```

## DeleteNode
`DELETE /api/nodes/{nodeId}`

Endpoint slúži na odstránenie uzla organizačnej štruktúry na základe jeho
jednoznačného identifikátora `nodeId` typu `GUID`.

Odstránením uzla dôjde zároveň k odstráneniu celého jeho podstromu,
t. j. všetkých podriadených uzlov, ktoré sú na daný uzol naviazané.
Tento prístup zjednodušuje správu organizačnej štruktúry a zabraňuje
vzniku neúplných alebo "osirelých" uzlov.

Použitie technického identifikátora `GUID` zabezpečuje jednoznačné
a bezpečné odstránenie konkrétneho uzla bez rizika kolízie identifikátorov.

### Parametre 
- `nodeId` – identifikátor uzla, ktorý má byť odstránený

### Response 

```json
{
  "Success": true,
  "Message": "Node deleted successfully",
  "Data": null
}
```

## ShowNodesByType
`GET /api/nodes/by-type`

Endpoint slúži na získanie zoznamu uzlov organizačnej štruktúry
na základe ich typu. Typ uzla je reprezentovaný číselnou hodnotou,
ktorá zodpovedá konkrétnemu typu organizačnej jednotky
(napr. Company, Division alebo Department).

Tento endpoint je určený najmä na zobrazenie alebo spracovanie
uzlov rovnakého typu, napríklad pri výbere konkrétnej organizačnej úrovne
v používateľskom rozhraní alebo pri analytickom spracovaní dát.

Použitie typu uzla ako filtra umožňuje efektívne a prehľadné
oddelenie jednotlivých úrovní organizačnej štruktúry.

V požiadavke je možné uviesť aj zoznam viacerých typov oddelených čiarkou napr. `1,4`, V tomto prípade endpoint vypíše zoznam všetkých uzlov typu 1 a zároveň aj typu 4, **neutriedene**.

### Parametre

- `type` – typ/typy uzla/uzlov (číselná hodnota reprezentujúca typ organizačnej jednotky)

### Response
```json
{
  "success": true,
  "message": "Nodes loaded by type",
  "data": [
    {
      "id": "fe4462e3-0ea5-4f4a-a4d7-0846c20867be",
      "name": "IT Department",
      "code": "IT-847251ff-4589-4902-9d45-03d4a9c77cd1",
      "type": 1,
      "parent": null
    },
    {
      "id": "2b49d99b-f049-414c-84b5-2134bccd6451",
      "name": "Company A",
      "code": "COMP-2cd90462-9478-4e85-bcac-752f7951cf41",
      "type": 1,
      "parent": null
    },
    {
      "id": "dc2679d5-153f-4bd9-b50e-5238feb25763",
      "name": "Department With Leader",
      "code": "LEADER-e0fe969b-a86c-4352-a219-473bc1be1b76",
      "type": 1,
      "parent": null
    },
    {
      "id": "146a4e9e-3d89-46ef-b6e1-8d1612b427d7",
      "name": "Company A",
      "code": "COMP",
      "type": 1,
      "parent": null
    }
  ]
}
```

## ShowSubHierarchy
`GET /api/nodes/{nodeId}/tree`

Endpoint slúži na získanie podhierarchie organizačnej štruktúry
od zvoleného uzla. Výsledkom je stromová štruktúra obsahujúca
zvolený uzol a všetky jeho podriadené uzly.

Tento endpoint je určený najmä na zobrazenie hierarchie organizácie
v používateľskom rozhraní, napríklad vo forme stromového zobrazenia
alebo navigačnej štruktúry.

Hierarchia je vracaná v rekurzívnej forme, kde každý uzol môže
obsahovať zoznam svojich podriadených uzlov.

### Parametre

- `nodeId` – identifikátor koreňového uzla, od ktorého má byť hierarchia získaná

### Response
```json
{
  "success": true,
  "message": "Node tree loaded successfully",
  "data": {
    "id": "2b49d99b-f049-414c-84b5-2134bccd6451",
    "name": "Company A",
    "code": "COMP-2cd90462-9478-4e85-bcac-752f7951cf41",
    "type": 1,
    "manager": null,
    "children": [
      {
        "id": "0cfc2d48-57ba-4110-a782-5564b78ac6a3",
        "name": "Division A",
        "code": "DIV-ecf48aaa-7462-4692-af83-7927156efe3e",
        "type": 2,
        "manager": null,
        "children": [
          {
            "id": "00eb1c94-6e15-4040-ab91-4d0973c576a4",
            "name": "Project A",
            "code": "Pro-629071da-d7ae-49c9-824a-49778b936ac5",
            "type": 3,
            "manager": null,
            "children": []
          }
        ]
      }
    ]
  }
}
```

## AssignManager
`POST /api/employees/assign-manager`

Endpoint slúži na priradenie zamestnanca do roly manažéra
k vybranému uzlu organizačnej štruktúry.

Manažér je viazaný priamo na uzol a reprezentuje vedúcu osobu
danej organizačnej jednotky. Priradenie manažéra je riešené
samostatným endpointom, aby bola zachovaná jednoznačnosť
a kontrola nad vedúcimi rolami v organizácii.

Použitie samostatného endpointu umožňuje nezávislé riadenie
organizačnej štruktúry a personálnych vzťahov.

**Konkrétny zamestnanec môže manažovať len jeden uzol.**

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
  "Success": true,
  "Message": "Manager assigned successfully",
  "data" : null
}
```

## UnassignManagerFromNode

`POST /api/employees/unassign-manager`

Endpoint slúži na odobratie manažéra z uzla organizačnej štruktúry.
Po vykonaní operácie nebude mať daný uzol priradeného žiadneho manažéra.

Tento endpoint umožňuje flexibilnú správu vedúcich rolí v organizácii,
napríklad pri organizačných zmenách alebo dočasnom uvoľnení manažérskej pozície.

Použitie samostatného endpointu zabezpečuje jasné oddelenie
medzi priradením a odobratím manažérskej roly.

### Request body
```json
{
  "nodeId": "guid"
}
```

### Response
```json
{
  "Success": true,
  "Message": "Manager unassigned successfully",
  "data" : null
}
```

