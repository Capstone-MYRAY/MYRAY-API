ECHO OFF
CLS
TITLE Run MYRAY.API

ECHO dotnet restore - Restore Dependency
dotnet restore

ECHO dotnet build  - Build Project
dotnet build

ECHO dotnet run - Run Project
dotnet run --project src/MYRAY.Api/MYRAY.Api.csproj

pause