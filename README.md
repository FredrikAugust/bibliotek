# Zanaris

_Der alvene bor_

## Setup

### Azure AD

Create an App Registration in Azure, and copy out the relevant values to the `appsettings.json` file.

Under "Authentication", create a SPA with the redirect URI "https://localhost:7232/swagger/oauth2-redirect.html" for Swagger authentication to work properly.

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

## Database

EF Core is used for database interaction, and FluentValidation _will_ be used once it's necessary to get some validation up in here.