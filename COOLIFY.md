# Guía de Despliegue en Coolify - Inkallajta Tour

Esta guía describe cómo desplegar el proyecto **Inkallajta Tour** en tu servidor Coolify a partir del repositorio de GitHub: `https://github.com/MZubiri/inkallajtatour`.

---

## Arquitectura de Despliegue

El proyecto está dockerizado y listo para funcionar en conjunto mediante **Docker Compose**:
1. **Frontend:** Angular 21 servido por Nginx (puerto `80`), con reverse proxy integrado en `/api/` y enrutamiento SPA.
2. **Backend:** ASP.NET Core 9 (puerto `5050`) conectado a MySQL.
3. **Database:** MySQL 8.0 con volumen persistente (`mysql_data`).

---

## Opción Recomendada: Despliegue con Docker Compose (1-Click)

1. En tu panel de **Coolify**:
   * Entra a tu **Project** y **Environment**.
   * Haz clic en **+ New Resource** -> **Docker Compose**.
2. Selecciona **From Git Repository**:
   * **Repository URL:** `https://github.com/MZubiri/inkallajtatour`
   * **Branch:** `main` (o la rama que uses).
3. Coolify detectará automáticamente el archivo [docker-compose.yml](file:///docker-compose.yml).
4. **Asignación de Dominio (FQDN):**
   * En la configuración del servicio `frontend`, ingresa el dominio público que deseas usar (ej. `https://tours.tudominio.com`).
   * Coolify generará y renovará automáticamente los certificados SSL con Let's Encrypt.
5. **Variables de Entorno (Opcional):**
   Si deseas cambiar las credenciales por defecto, puedes definirlas en la sección *Environment Variables*:
   * `DB_ROOT_PASSWORD`: Contraseña root de MySQL.
   * `DB_NAME`: Nombre de la base de datos (por defecto `inkallajta_tour`).
   * `JWT_KEY`: Llave secreta para firmar tokens JWT (mínimo 32 caracteres).
   * `CORS_ALLOWED_ORIGINS`: Dominios permitidos o `*`.
6. Haz clic en **Deploy**.

---

## Credenciales Iniciales del Panel de Administración

Una vez desplegado:
* **URL:** `https://tu-dominio.com/admin/login`
* **Email:** `admin@inkallajtatour.com`
* **Contraseña:** `Admin123!`

> [!IMPORTANT]
> Recuerda cambiar la contraseña del usuario administrador una vez ingreses al panel de control en producción.
