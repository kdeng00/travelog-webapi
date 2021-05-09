# travelog-webapi

## Built with

* C# (.NET 3.1) 


## Getting started

Clone the repository

```BASH
git clone https://github.com/kheneahm-ares/travelog-webapi
```

Install dotnet migrations

```BASH
dotnet tool install --global dotnet-ef --interactive --version 3.1
```

Change directory to Persistance and run the following commands:

```BASH
dotnet ef migrations add TravelLogInitialCreate
dotnet ef database update
```



Set the appsettings file:

```Json
{
  "IdentityServerUrl": "https://localhost:5001",
  "ConnectionStrings": {
    "IdentityServer": "Server=someserver;Database=IdentityServer;User Id=TravelLogUser;Password=Coast2coast;",
    "TravelogApi": "Server=someserver;Database=TravelogApi;User Id=TravelLogUser;Password=Coast2coast;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```
