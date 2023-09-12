# TestSystem


### Quiz system example with Cosmos DB
This is my first project with a NoSQL database and is built with Visual Studio
You can find the endpoint description in the Swagger doc http://localhost:5172/swagger/index.html

## How to run:

### Database

#### MS SQL Server
Before running this project, create an SQL Server Instance.
Change appsettings.json (or manage user secrets for the TestSystem project, as you prefer) and add TestSystemContext in ConnectionStrings. Use SQL Server connection string.

#### Cosmos DB (not the best solution)
Before running this project, create an Azure cosmos db account with NoSQL API.
Change appsettings.json (or manage user secrets for the TestSystem project, as you prefer) and add "uri" and "key" parameters using the uri and key of your Cosmos DB.
For run with cosmos db, edit ServiceCollectionExtensions.AddProjectServices() method as described in the comment.

### Create and fill DB
For the database go to http://localhost:5172/swagger/index.html and call these two endpoint
1 - PUT /api/DatabaseInitialization/create (for creating database and containers)
2 - POST /api/DatabaseInitialization/populate (for populate containers with randoms data)

At the end, you can call this endpoint to delete DB:
DELETE /api/DatabaseInitialization

### Run:
Start project "Test system" in debug from VS. launchSettings.json is configured to run in localhost:5172.