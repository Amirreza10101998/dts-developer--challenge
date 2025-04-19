# Task Management System

A system for HMCTS caseworkers to manage their tasks with API and frontend components.

## Features

- Create, read, update, and delete tasks
- Task properties: Title, Description (optional), Status, Due Date
- RESTful API with proper HTTP status codes
- Clean, user-friendly interface
- Unit tested backend

## Technologies Used

### Backend
- ASP.NET Core
- Entity Framework Core
- SQLite Server
- xUnit/Moq for testing

### Frontend
- Angular
- TypeScript
- HTML5/CSS3

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/amashaik/task-management-system.git
   cd task-management-system
 
2. Backend Setup: 
   cd TaskManagement.Api
   dotnet restore
   dotnet ef database update 
   dotnet run

3. Frontend setup:
   cd ../task-management-ui
   npm install
   npm start

## Running Tests

cd TaskManagement.Tests
dotnet test

