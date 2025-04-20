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
   git clone https://github.com/Amirreza10101998/dts-developer--challenge.git
   cd dts-developer--challenge
 
2. Backend Setup: 
- cd TaskManagement.API
- dotnet restore
- dotnet ef database update 
 -dotnet run

3. Frontend setup:
 - cd ../task-manager-ui
 - npm install
 - ng serve 

## Running Tests
 - cd TaskManagement.Tests
 - dotnet test

## API Documentation
Endpoints
Method	Endpoint	Description
GET	/api/v1/task	Get all tasks
GET	/api/v1/task/{id}	Get single task by ID
POST	/api/v1/task	Create new task
PATCH	/api/v1/task/{id}	Update task status
DELETE	/api/v1/task/{id}	Delete task
