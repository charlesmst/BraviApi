This is a REST web api for a simple contact app.
## Requirements 
- SQL Server Express
- Dotnet core
- Asp net core 2.2

## Setting up
This app uses a SQL Server database, to create the necessary tables do the following steps:
- Create a database in Sql Server
- Update Connection string at `src/BraviApi/appsettings.Development.json`
- Enter folder `src/BraviApi`
- Run migration `dotnet ef database update`. Alternativelly you can run the migration manually by running the SQL file `sql/update_to_latest.sql` directly in database

## Running app
Enter the project folder.
### `cd src/BraviApi`
Execute the app.
### `dotnet run`
The front end for this application is available [here](https://github.com/charlesmst/BraviApp).

## Tests
Enter the test project folder.
### `cd tests/BraviApiTests`
Execute the app with the formula to validate as first argument.
### `dotnet test`
