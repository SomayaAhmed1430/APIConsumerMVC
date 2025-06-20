# ASP.NET Core MVC Consumer - Department API Integration

This project is an **MVC Web Application** that **consumes a Web API** (ProviderAPI) using `HttpClient`. It displays and manages data from the API such as Departments and Employees.

---

## 🎯 Features

- ASP.NET Core MVC project
- Uses `HttpClient` to call API endpoints
- Displays Departments from API
- Supports:
  - Viewing list of departments
  - Viewing department details
  - Adding a new department
  - Updating an existing department
  - Deleting a department
- Strongly-typed models for API data
- Clean UI using Razor Views

---

## 🔗 Consumed API Endpoints (From ProviderAPI)

| Method | Route                            | Description                            |
|--------|----------------------------------|----------------------------------------|
| GET    | `api/Department`                 | Get all departments                    |
| GET    | `api/Department/{id}`            | Get department by ID                   |
| POST   | `api/Department`                 | Add new department                     |
| PUT    | `api/Department/{id}`            | Update department                      |
| DELETE | `api/Department/{id}`            | Delete department                      |

---

## 🔧 Technologies Used

- ASP.NET Core MVC  
- C#  
- `HttpClient`  
- Newtonsoft.Json  
- Visual Studio 2022

---

## 🚀 How to Run

1. Open the solution in Visual Studio  
2. Make sure the **ProviderAPI** is running (check the Swagger link)  
3. Update the **API base URL** in `appsettings.json` or wherever you're using `HttpClient`  
4. Run the MVC project (Ctrl + F5)  
5. Browse the views and perform operations that talk to the API

---

## 📌 Notes

- This project **does not have its own database**, it depends entirely on the **ProviderAPI**
- All data is fetched or sent via `HttpClient`
- Supports JSON serialization/deserialization using Newtonsoft.Json

---

> Developed by Somaya ✨
