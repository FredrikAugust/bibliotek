# Zanaris

_Der alvenes bøker bor._

## Setup

### Azure AD

Create an App Registration in Azure, and copy out the relevant values to the `appsettings.json` file.

Under "Authentication", create a SPA with the redirect URI `https://localhost:7232/swagger/oauth2-redirect.html` for Swagger authentication to work properly.

After this, create a "Web" authentication method with the redirect URI `https://oauth.pstmn.io/v1/callback` if you want to develop using Postman.

You also want to create a scope under "Expose an API", and copy the resulting scope into `appsettings.json`. You should call this something along the lines of `access_as_user` for consistency.

### Logging

Run the `docker-compose` to start Elasticsearch and Kibana.

Copy out the following file from the elastic container, `/usr/share/elasticsearch/config/certs/http_ca.crt` and install it locally. This is required for communication with the container from the ASP.Net application.

Navigate to http://localhost:5601 to access Kibana. Follow the steps necessary to set up the environment. As part of this you will have to generate a new password for the `elastic` user. Note this down as you will use it in the connection string for the Serilog to Elastic-sink (Serilog is the logging library used).

Now, create the connection string for elastic using the following template:

```
https://{username}:{password}@{host}:{port}
```

Set this either in `appsettings.json` or using 
```shell
dotnet user-secrets set "Serilog:Elastic:ConnectionString" "<value>"
```

You should now be able to start the web server from the `Web` project.

## Architecture

This is a lax implementation of the [clean architecture template](https://github.com/jasontaylordev/CleanArchitecture). Some parts are omitted for simplicity. However, the essence of the clean architecture pattern is kept.

A partial CQRS approach has been implemented using MediatR, although there is no separated, dedicated ViewStore to _really_ achieve CQRS.

- **Domain**: contains all the models and ValueObjects (complex types) which are stored in your database. This defines the baseline of available data in your app.
- **Application**: business logic. This is where we do all heavy lifting in terms of retrieving data, processing and sending back to the Web project. Does not contain any actual links to DB or other services, only interacts with defined interfaces.
- **Infrastructure**: other services. Here we connect to other services such as logging, database etc. We then provide a function which will inject these services into the dependency injection container in the Web project, from which it will propagate down to the other projects.
- **Web**: ASP.Net application which handles user interaction. This is also the only project which directly or transiently depends on all other projects. This is where we inject the services into DI.

## Database

EF Core is used for database interaction, and FluentValidation _will_ be used once it's necessary to get some validation up in here.

A Microsoft SQL Server database is used, and is available preconfigured in the docker compose.

You will, however, need to add the connection strings in the `appsettings.json` files for dev and test.

## Endpoint debugging

For debugging the endpoints, I recommend using Postman. You can configure postman to authenticate using Azure AD and automatically attach tokens to your requests.

You can import the Swagger JSON exposed by the app directly into Postman to get a complete setup of all the endpoints. Go to API > Link and paste the URI which should look something like `https://localhost:7232/swagger/v1/swagger.json`.