# Inkallajta Tour 🌄🏔️

Plataforma web turística completa (Frontend + Backend + Base de Datos) para reserva y gestión de tours en Cusco, Machu Picchu y Perú.

---

## 🚀 Tecnologías

* **Frontend:** Angular 21 (Signals, Standalone Components, SPA), Nginx.
* **Backend:** ASP.NET Core 9 (Web API, Entity Framework Core, JWT, BCrypt).
* **Base de Datos:** MySQL 8.0.
* **Despliegue:** Docker, Docker Compose, Coolify.

---

## 📦 Estructura del Proyecto

```
inkallajtatour/
├── backend/                  # API REST en ASP.NET Core 9
│   ├── Controllers/          # Endpoints (Tours, Bookings, Categories, etc.)
│   ├── Data/                 # Entity Framework Core DbContext y Seeders
│   ├── Models/               # Modelos de datos
│   ├── Dockerfile            # Imagen multi-stage para producción
│   └── Program.cs            # Configuración de servicios, CORS y JWT
├── frontend/                 # Aplicación Angular 21
│   ├── src/                  # Componentes públicos y panel admin
│   ├── public/               # Assets estáticos y env.js (config runtime)
│   ├── nginx.conf            # Servidor Nginx con proxy inverso /api
│   └── Dockerfile            # Imagen multi-stage con Nginx
├── docker-compose.yml        # Orquestación lista para Coolify / Docker
├── COOLIFY.md                # Guía paso a paso para despliegue en Coolify
└── .env.example              # Variables de entorno de ejemplo
```

---

## 💻 Ejecución Local

### 1. Requisitos
* .NET SDK 9.0
* Node.js 20+ y npm
* MySQL Server (en puerto 3306)

### 2. Backend
```bash
cd backend
dotnet run --launch-profile http
# API disponible en http://localhost:5050/api
```

### 3. Frontend
```bash
cd frontend
npm install
npm start
# Aplicación disponible en http://localhost:4200
```

---

## 🚢 Despliegue en Coolify

Para instrucciones detalladas sobre el despliegue con Docker Compose en Coolify, consulta la guía [COOLIFY.md](COOLIFY.md).
