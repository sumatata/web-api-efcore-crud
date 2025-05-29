# ASP.NET Core Web API with Entity Framework Core – CRUD Operations

This project demonstrates a RESTful Web API built with ASP.NET Core and Entity Framework Core. It supports full Create, Read, Update, and Delete (CRUD) operations on a sample data model, serving as a foundational backend for data-driven applications.

## Technologies Used

- ASP.NET Core
- Entity Framework Core
- AutoMapper
- Visual Studio / .NET CLI

## Project Structure

- `WebRestAPI/` – Main API project with controllers and services
- `WebRestEF/` – Data layer with EF Core models and DbContext
- `MappingProfile.cs` – AutoMapper configuration
- `WebRest.sln` – Visual Studio solution file
- `notes.txt` – Notes and checklist for development setup

##  Getting Started

### 1. Clone the repository

git clone https://github.com/sumatata/web-api-efcore-crud.git
cd web-api-efcore-crud

### 2. Open the solution in Visual Studio
	•	Open WebRest.sln
	•	Restore NuGet packages and build the solution

### 3. Run the application
	•	Set WebRestAPI as the startup project
	•	Run it locally using IIS Express or Kestrel
	•	The API should launch at https://localhost:<port>/api/<controller>

### Example Endpoints

Replace products with your actual controller name

	•	GET /api/products – Retrieve all items
	•	GET /api/products/{id} – Retrieve an item by ID
	•	POST /api/products – Create a new item
	•	PUT /api/products/{id} – Update an existing item
	•	DELETE /api/products/{id} – Delete an item

Author

Suma Tata
MS in  Data Science, University of Delaware
