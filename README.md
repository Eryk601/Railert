Tworzenie bazy danych

1. Utwóż bazę danych w pgadmin 4 o nazwie SzklanaStrefaDB
2. Edytuj plik server/appsettings.json i ustaw swoje dane do PostgreSQL(port, hasło)
3. W termianlu w katalogu server/ uruchom dotnet ef database update

Migracja bazy dancyh

1. dotnet ef migrations add NazwaMigracji
2. dotnet ef database update

Reset bazy danych

1. dotnet ef database drop --force
2. dotnet ef database update

Biblioteki:

1. npm install leaflet react-leaflet
2. npm install date-fns
3. npm install react-icons
