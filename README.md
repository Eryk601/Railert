Railert

Railert to aplikacja, która umożliwia pasażerom zgłaszanie opóźnień i utrudnień w transporcie publicznym oraz przeglądanie aktualnych raportów od innych użytkowników. System automatycznie analizuje podróże i ostrzega o możliwych problemach z przesiadkami, pomagając w planowaniu trasy. Projekt został stworzony przez zespół Drift w ramach hackathonu HackYeah 2025 w ciągu 24 godzin.

Jak uruchomić aplikację
Backend (API – ASP.NET Core)
Zainstaluj .NET 8 SDK i PostgreSQL.
W folderze server/ uruchom:

dotnet ef database update
dotnet run

Otwórz przeglądarkę i przejdź pod adres:
https://localhost:7265/swagger

gdzie znajduje się interfejs testowy API.

Frontend (React)
Przejdź do folderu client/.
Zainstaluj zależności i uruchom projekt:

npm install
npm start

Aplikacja uruchomi się pod adresem:
http://localhost:3000

Konta testowe
Możesz zalogować się jednym z gotowych użytkowników:
Rola	Email	Hasło	Uprawnienia

Użytkownik	user	user	Dodawanie raportów, tworzenie podróży

Moderator	moderator	moderator	Zatwierdzanie i usuwanie raportów

Administrator	admin	admin	Pełny dostęp do systemu i danych

Cel aplikacji
Celem Railerta jest umożliwienie pasażerom szybkiego reagowania na opóźnienia oraz wymiany informacji w czasie rzeczywistym.
W przyszłości planowana jest integracja z API PKP Intercity, predykcja opóźnień przy użyciu AI oraz rozszerzenie o aplikację mobilną.


Celem Railerta jest umożliwienie pasażerom szybkiego reagowania na opóźnienia oraz wymiany informacji w czasie rzeczywistym.
W przyszłości planowana jest integracja z API PKP Intercity, predykcja opóźnień przy użyciu AI oraz rozszerzenie o aplikację mobilną.
