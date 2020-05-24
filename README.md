# AnCoFT

![status in development](https://img.shields.io/badge/Status-In%20development-yellow)

AnCoFT is an open source server emulator project for the game **Fantasy Tennis** written in C# using .NET Core 3.1.

## Supported game versions

| Client language  | Client version | Status  |
| ------------- |---------------| ------|
| Thai        | 1.706 |  Working      |

Other versions need to be tested

## Software used

| Name      | Type          | URL      |  License  |  Platform  |
| ------------- |---------------| ------|--------------|-----------|
| x64dbg        | Debugging     |   [Link](https://x64dbg.com/) |      Open source        |  Windows |
| Ghidra        | Reverse Engineering | [Link](https://ghidra-sre.org/) |  Open source      |  Cross platform |
| .NET Core 3.1   | Software Framework  |    [Link](https://dotnet.microsoft.com/download/dotnet-core/3.1) |     Open source         |  Cross platform |
| PostgreSQL    | Database      |    [Link](https://www.postgresql.org/) |     Open source         |  Cross platform |

## Installation
1. Install PostgreSQL
2. Run the server files once.
3. Edit the server.json file according to your database/smtp info.
3. Run the server files again, the database will be created during the first run
4. Add an account to the Account table using a suitable tool e.g. PgAdmin 4 or the Website
5. Edit the ServerInfo.ini file of the game which should be located in the root folder
6. Run the game
