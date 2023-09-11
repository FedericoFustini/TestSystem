# TestSystem


### Quiz system example with Cosmos DB
This is my first project with a nosql database and is build with Visual Studio
You can find endpoint description in the swagger doc http://localhost:5172/swagger/index.html

## How to run:

### Database

#### MS SQL Server
Before run this project, create an SQL Server Instance.
Change appsettings.json (or manage user secrets for TestSystem project, as you prefer) and add TestSystemContext in ConnectionStrings. Use sql server connection string.

#### Cosmos DB (not the best solution)
Before run this project, create an azure cosmos db account with NoSQL API.
Change appsettings.json (or manage user secrets for TestSystem project, as you prefer) and add "uri" and "key" parameters using uri and key of your Cosmos DB.
For run with cosmos db, edit ServiceCollectionExtensions.AddProjectServices() method as described in comment.

### Create and fill DB
For database go to http://localhost:5172/swagger/index.html and call this two endpoint
1 - PUT /api/DatabaseInitialization/create (for create database and containers)
2 - POST /api/DatabaseInitialization/populate (for populate containers with randoms data)

At the end you can call this endpoint for delete DB:
DELETE /api/DatabaseInitialization

### Run:
Start project "Test system" in debug from VS. launchSettings.json is configured to run in localhost:5172.