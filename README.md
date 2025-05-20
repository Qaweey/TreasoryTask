#  Product Integration API

This is a simple RESTful Web API built using **.NET 8.0**, which integrates with the mock API provided at [https://restful-api.dev](https://restful-api.dev/). It extends the functionality of the mock API by introducing structured logging, filtering, paging, model validation, and robust error handling.

---

## 📌 Features

- ✅ Retrieve products with **filter by name** and **pagination**.
- ✅ Add new products with **dynamic data fields**.
- ✅ Delete products by ID.
- ✅ Proper model **validation** and **error handling**.
- ✅ **Structured logging** for request tracing and debugging.

---

## 🚀 Technologies Used

- .NET 8.0 Web API
- C#
- HttpClientFactory
- ILogger for structured logging
- System.Text.Json for JSON (with dynamic `Dictionary<string, object>` support)
- Dependency Injection (DI)

---



