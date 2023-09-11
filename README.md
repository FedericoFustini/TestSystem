# TestSystem


### Quiz system example with Cosmos DB
This is my first project with a nosql database and is build with Visual Studio

## How to run it:

### Database
Before run this project, create an azure cosmos db account with NoSQL API.

For database go to http://localhost:5172/swagger/index.html and call this two endpoint
1 - PUT /api/DatabaseInitialization/create (for create database and containers)
2 - POST /api/DatabaseInitialization/populate (for populate containers with randoms data)

At the end you can call this endpoint for delete DB:
DELETE /api/DatabaseInitialization

### Configuration
Change appsettings.json (or manage user secrets for TestSystem project, as you prefer) and add "uri" and "key" parameters using uri and key of your Cosmos DB.

### Run:
Start project "Test system" in debug from VS. launchSettings.json is configured to run in localhost:5172.