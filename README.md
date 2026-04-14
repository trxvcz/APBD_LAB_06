# Workshop & Room Management API

Prosta aplikacja ASP.NET Core Web API służąca do zarządzania salami dydaktycznymi oraz rezerwacjami w centrum szkoleniowym. Projekt koncentruje się na praktycznym zastosowaniu routingu, metod HTTP oraz walidacji danych bez użycia zewnętrznej bazy danych.

## 🚀 Cel projektu
Celem zadania było przećwiczenie:
* Projektowania REST API opartego na kontrolerach.
* Obsługi różnych metod HTTP (GET, POST, PUT, DELETE).
* Model bindingu z różnych źródeł: Route, Query String oraz Body (JSON).
* Implementacji logiki biznesowej i walidacji (Data Annotations).
* Zwracania poprawnych kodów statusu HTTP (200, 201, 204, 400, 404, 409).

## 🛠️ Technologie
* **Framework:** ASP.NET Core 8.0/9.0 (Web API)
* **Język:** C#
* **Przechowywanie danych:** In-memory (statyczne listy w pamięci aplikacji)
* **Testowanie:** Postman

## 📋 Modele danych

### Room (Sala)
- `Id`, `Name`, `BuildingCode`, `Floor`, `Capacity`, `HasProjector`, `IsActive`

### Reservation (Rezerwacja)
- `Id`, `RoomId`, `OrganizerName`, `Topic`, `Date`, `StartTime`, `EndTime`, `Status` (np. planned, confirmed, cancelled)

## 📡 Endpointy API

### Kontroler Sal (`/api/rooms`)
| Metoda | Endpoint | Opis |
| :--- | :--- | :--- |
| **GET** | `/api/rooms` | Pobiera listę wszystkich sal. |
| **GET** | `/api/rooms/{id}` | Pobiera szczegóły konkretnej sali. |
| **GET** | `/api/rooms/building/{buildingCode}` | Pobiera sale w danym budynku (parametr z trasy). |
| **GET** | `/api/rooms?minCapacity=X&hasProjector=Y` | Filtrowanie sal po Query Stringu. |
| **POST** | `/api/rooms` | Dodaje nową salę. |
| **PUT** | `/api/rooms/{id}` | Pełna aktualizacja danych sali. |
| **DELETE** | `/api/rooms/{id}` | Usuwa salę. |

### Kontroler Rezerwacji (`/api/reservations`)
| Metoda | Endpoint | Opis |
| :--- | :--- | :--- |
| **GET** | `/api/reservations` | Pobiera listę wszystkich rezerwacji. |
| **GET** | `/api/reservations/{id}` | Pobiera szczegóły rezerwacji. |
| **GET** | `/api/reservations?date=...&roomId=...` | Filtrowanie rezerwacji (data, status, sala). |
| **POST** | `/api/reservations` | Tworzy nową rezerwację (z walidacją kolizji). |
| **PUT** | `/api/reservations/{id}` | Aktualizuje istniejącą rezerwację. |
| **DELETE** | `/api/reservations/{id}` | Usuwa rezerwację. |

## 🛡️ Reguły biznesowe i Walidacja
1. **Walidacja pól:** Pola tekstowe (nazwa, budynek, organizator) są wymagane. Pojemność sali musi być większa niż 0.
2. **Czas pracy:** Czas zakończenia rezerwacji musi być późniejszy niż czas rozpoczęcia.
3. **Logika rezerwacji:**
   - Nie można zarezerwować sali, która nie istnieje.
   - Nie można zarezerwować sali oznaczonej jako `IsActive = false`.
   - **Brak kolizji:** System uniemożliwia dodanie dwóch rezerwacji w tej samej sali, które nakładają się czasowo (zwraca `409 Conflict`).

## ⚙️ Uruchomienie projektu
1. Sklonuj repozytorium.
2. Otwórz projekt w Visual Studio lub VS Code.
3. Uruchom aplikację za pomocą `dotnet run` lub przycisku Start w IDE.
4. Aplikacja zainicjalizuje przykładowe dane (4-5 sal oraz 4-6 rezerwacji).
5. API będzie dostępne pod adresem `http://localhost:5000` (lub innym wskazanym w konsoli).

## 🧪 Testowanie (Postman)
Aby przetestować API, zaimportuj kolekcję do Postmana i sprawdź:
- **Happy Path:** Dodawanie poprawnej rezerwacji.
- **Error Handling:** Próba dodania rezerwacji w zajętym terminie (409).
- **Validation:** Wysłanie pustego pola `Name` (400).
- **Not Found:** Próba pobrania nieistniejącego ID (404).

---
*Projekt wykonany w ramach ćwiczeń z podstaw budowania systemów rozproszonych i API.*
