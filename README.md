# 🍋 Mango Microservices Project

This is a real-world **e-commerce microservices application** built with the latest technologies in the .NET ecosystem. The `Mango` project demonstrates a modular, scalable architecture using **.NET 8**, **Ocelot API Gateway**, **Entity Framework Core**, **.NET Identity**, **Azure Service Bus**, and **SQL Server** — all following **clean architecture** and **best practices**.

---

## 📚 Project Description

You will learn the foundational elements of microservices by incrementally building an application step by step. This course-level project includes:

- **Modular Microservices Architecture**
- **Authentication & Authorization** with `.NET Identity`
- **Communication** over **Azure Service Bus**
- **API Gateway** with **Ocelot**
- **Layered Architecture** with Clean Code Principles
- **MVC Frontend** to interact with all microservices

---

## 📦 Microservices Included

- ✅ **Product Microservice**
- ✅ **Coupon Microservice**
- ✅ **Shopping Cart Microservice**
- ✅ **Order Microservice**
- ✅ **Email Microservice**
- ✅ **Payment Microservice**
- ✅ **.NET Identity Microservice**
- ✅ **Ocelot API Gateway**
- ✅ **MVC Web Application**

---

## 🛠️ Technologies Used

- **.NET 6 / .NET 8**
- **Entity Framework Core**
- **SQL Server**
- **Ocelot API Gateway**
- **.NET Identity**
- **ASP.NET Core MVC**
- **RESTful APIs**
- **Docker** (optional for deployment)
- **Swagger** (for API testing)

---

## 🚀 Getting Started

### 🔧 Prerequisites

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Microsoft Edge](https://www.microsoft.com/edge)
---

### 🏁 How to Run in Microsoft Edge

1. **Open the solution (`Mango.sln`)** in **Visual Studio**.
2. Right-click the **MVC Web Project** (Frontend) and set it as **Startup Project**.
3. Click the browser dropdown near the green `Run` (▶️) button.
4. Select **Microsoft Edge**.  
   If not available:
   - Click **"Browse With..."**
   - Add Edge path:  
     `C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe`
   - Set it as default.
5. Press `F5` or click the green `Run` button.
6. Application will open in **Microsoft Edge** using `https://localhost:{port}`.

---

### 💻 Running All Microservices

> Run each microservice individually or configure **Docker Compose** for orchestration (optional).

- You can start each microservice via:
  ```bash
  dotnet run --project Mango.Services.[ServiceName]
