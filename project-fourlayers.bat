@echo off

REM Verificar si .NET Core SDK está instalado
dotnet --version > nul 2>&1
if %errorlevel% neq 0 (
    echo Error: .NET Core SDK no está instalado.
    exit /b 1
)

REM Solicitar el nombre de la carpeta del proyecto
set /p nombreCarpeta="Nombre de la carpeta del proyecto: "

REM Verificar si la carpeta ya existe
if exist %nombreCarpeta% (
    echo Error: La carpeta %nombreCarpeta% ya existe.
    exit /b 1
)

REM Crear la carpeta del proyecto
mkdir %nombreCarpeta%
cd %nombreCarpeta%

REM Crear la solución
dotnet new sln

REM Crear el proyecto API
dotnet new webapi -o API

REM Agregar el proyecto API a la solución
dotnet sln add ./API/

REM Crear el proyecto Application
dotnet new classlib -o Application

REM Agregar el proyecto Application a la solución
dotnet sln add ./Application/

REM Crear el proyecto Domain
dotnet new classlib -o Domain

REM Agregar el proyecto Domain a la solución
dotnet sln add ./Domain/

REM Crear el proyecto Persistence
dotnet new classlib -o Persistence

REM Agregar el proyecto Persistence a la solución
dotnet sln add ./Persistence/

REM Cambiar al directorio del proyecto API
cd ./API/
REM Crea las carpetas en API
mkdir Controllers
mkdir Dtos
mkdir Extensions
mkdir Helpers
mkdir Profiles
mkdir Services

REM Instala dependencias
dotnet add package AspNetCoreRateLimit --version 5.0.0
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 7.0.10
dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.1.0
dotnet add package Microsoft.AspNetCore.OpenApi --version 7.0.10
dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.10
dotnet add package System.IdentityModel.Tokens.Jwt --version 6.32.2
dotnet add package Serilog.AspNetCore --version 7.0.1-dev-00320
dotnet add package Microsoft.Extensions.DependencyInjection --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 7.0.13

REM Conectar el proyecto Infrastructure con el proyecto API
dotnet add reference ../Application/

REM Volver al directorio principal
cd ..

REM Cambiar al directorio del proyecto Application
cd ./Application/
dotnet add reference ../Domain/
dotnet add reference ../Persistence/

REM Crea las carpetas en Application

mkdir Repositories
mkdir UnitOfWork

REM Volver al directorio principal
cd ..

REM Cambiar el directorio a Domain
cd ./Domain/

REM Instala dependencias
dotnet add package FluentValidation.AspNetCore --version 11.3.0
dotnet add package itext7.pdfhtml --version 5.0.2
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 7.0.13

mkdir Entities
mkdir Interfaces

REM Volver al directorio principal
cd ..

REM Cambiar el directorio a Persistence
cd ./Persistence/

REM Instala dependecias
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.13
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 7.0.0

REM Conectar el proyecto Persistence con Domain
dotnet add reference ../Domain/

REM Crea el directirio Data
mkdir Data
cd ./Data/
mkdir Configuration
cd ..

REM Volver al directorio principal
cd ..

REM Instalar la herramienta dotnet-ef
dotnet tool install --global dotnet-ef

REM Crear el archivo de migración
echo @echo off > migration.bat
echo. >> migration.bat
echo REM Solicitar el nombre de la migración >> migration.bat
echo set /p nombreMigracion="Nombre de la migracion: " >> migration.bat
echo. >> migration.bat
echo REM Generar la migración >> migration.bat
echo dotnet ef migrations add !nombreMigracion! --project ./Persistence/ --startup-project ./API/ --output-dir ./Data/Migrations >> migration.bat
echo. >> migration.bat
echo REM Actualizar la base de datos >> migration.bat
echo dotnet ef database update --project ./Persistence/ --startup-project ./API/ >> migration.bat


echo "Se generó el archivo de migración migration.bat"

echo "Se creó el proyecto webApi de %nombreCarpeta%"

@echo off
setlocal enabledelayedexpansion

:: Define la URL del archivo y el token de autorización
set "url=https://raw.githubusercontent.com/Lenin2611/snippets-vsc/main/snippets-fourlayers.txt"
set "token=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"

:: Define la ruta del archivo de salida
set "rutaArchivo=snippets.txt"

:: Ejecuta la solicitud HTTP utilizando curl y guarda la respuesta en un archivo
curl -H "Authorization: Bearer %token%" -o "%rutaArchivo%" -L "%url%"

:: Comprueba si la descarga se realizó correctamente
if %errorlevel% equ 0 (
    echo Descarga exitosa. El archivo se guardó en %rutaArchivo%.
) else (
    echo Error en la descarga.
)

endlocal


@echo off
setlocal enabledelayedexpansion

:: Define la URL del archivo y el token de autorización
set "url=https://raw.githubusercontent.com/zzfuture/vscodeSnippets/main/gitignore.txt"
set "token=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"

:: Define la ruta del archivo de salida
set "rutaArchivo=.gitignore"

:: Ejecuta la solicitud HTTP utilizando curl y guarda la respuesta en un archivo
curl -H "Authorization: Bearer %token%" -o "%rutaArchivo%" -L "%url%"

:: Comprueba si la descarga se realizó correctamente
if %errorlevel% equ 0 (
    echo Descarga exitosa. El archivo se guardó en %rutaArchivo%.
) else (
    echo Error en la descarga.
)

endlocal



REM Abrir el proyecto en Visual Studio Code
code .

exit /b 0