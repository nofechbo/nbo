# Project: Launchers Management System
Owner: Nofech Ben Or

Theis project simulates a logistic system that supports different missile launchers.
this is a learning-project, aimed at understanding basic concepts in c#.

the project consists of 4 main parts:
1. main server
2. human client program
	(aimed to be used by a user/soldier, to update information about the launcher)
3. launcher client program
	(a program representing the launcher, that automatically alerts malfunctions)
4. Report-generator

## Prerequisites
To run this project, you need the following installed on your system:
- **.NET SDK**: Version 8.0 or later. 
- **System.Net.Sockets**
- **SQLite**
- **QuestPDF**: Install using the following command:
  dotnet add package QuestPDF
- **Entity Framework Core**:  Install using the following commands:
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.Sqlite


steps to run the project:
1. Clone the repository.

## Running the human client program:
2. Build the project:
	run `dotnet build` in "TCPServer" directory.
	run `dotnet build` in "TCPClient" directory.
3. run:
	run 'dotnet run' in "TCPServer" directory.
	run 'dotnet run' in "TCPClient" directory.
- The server and client will connect on `127.0.0.1:12345` by default.


## Running the launcher client program:
2. Build the project:
	run `dotnet build` in "TCPServer" directory.
	run `dotnet build` in "LauncherManagement" directory.
3. run:
	run 'dotnet run' in "TCPServer" directory.
	run 'dotnet run' in "LauncherManagement" directory.
- The server and client will connect on `127.0.0.1:12345` by default.


## Running the Report-generator program:
2. Build the project:
	run `dotnet build` in "ReportGenerator" directory.
3. run:
	run 'dotnet run' in "ReportGenerator" directory.






