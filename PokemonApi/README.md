# 🧩 Pokémon Intermediate API (.NET 8)

A **.NET 8 Web API** that acts as an **intermediate layer** between clients and the public **PokéAPI**, enriched with **Redis caching**, **organization-level API key authentication**, and **AI-generated Pokémon stories** using **OpenAI or OpenRouter**.

This project is designed to demonstrate **clean architecture**, **scalable API design**, and **real-world integration patterns**.

---

## 🚀 Features

### Core Features
- 📄 List Pokémons (max 10 per request)
- 🔍 Search Pokémons by **id or name**
- ⚡ Redis caching for optimized performance
- 🔐 API Key authentication (1 key per organization)
- 📦 Generic API response structure
- 📊 Swagger enabled by default

### Bonus / Advanced
- 🤖 AI-generated Pokémon stories
- 🔁 Multiple AI providers (**OpenAI / OpenRouter**)
- 🏭 Factory + Strategy pattern for AI provider selection
- 🧯 Global exception handling middleware
- 🧾 Structured logging
- 🧠 Configuration-driven behavior

---

### Design Patterns Used
- Repository Pattern
- Factory Pattern (AI provider selection)
- Middleware Pattern
- Options Pattern
- Dependency Injection

## 🛠 Prerequisites

Make sure the following are installed:

### 1️⃣ .NET SDK
- **.NET 8 SDK**
- Download: https://dotnet.microsoft.com/download
- **Docker** (for Redis)
- docker run -d -p 6379:6379 redis

- verify Redis is running:
  ```bash
  docker ps
  redis-cli ping  (Expect Pong)
  ```	


- Set Environment Variables
- setx OPENAI_API_KEY "sk-xxxx"
- setx OPENROUTER_API_KEY "or-xxxx"
- ⚠️ Never store secrets in appsettings.json

setup Redis connection string in appsettings.json:
``` "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
  ```
- Thankyou


## How to run the project
- Clone the repository  https://github.com/parthgodshelwar2024/Pokemon
- dotnet restore
- dotnet build
- dotnet run
- You should see logs similar to: Redis OK
- https://localhost:{port}/swagger
- All endpoints require this header:  
    ```
-       "ApiKeys": {
      "org-pokemon-team": "abc123-secret-key"
      }
-   ```






