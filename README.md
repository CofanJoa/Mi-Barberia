# Mi Barbería 💈

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Una aplicación web moderna para la gestión de reservas de una barbería, construida con ASP.NET Core MVC. Incluye un panel público para clientes y un panel de administración completo para gestionar servicios, barberos y citas.

## ✨ Características

- **Panel de Clientes**: Los clientes pueden explorar servicios, ver barberos y agendar sus citas en línea.
- **Panel de Administración**: Gestión completa de servicios, barberos, horarios y visualización de todas las citas agendadas.
- **Sistema de Usuarios**: Roles diferenciados para clientes y administradores.
- **Interfaz Intuitiva**: Diseño responsive y fácil de usar construido con Bootstrap.

## 🛠️ Tecnologías Utilizadas

- **Backend**: ASP.NET Core 6.0 MVC
- **Base de Datos**: SQL Server con Entity Framework Core (Code-First)
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap
- **Autenticación y Autorización**: ASP.NET Core Identity
- **Patrón de Diseño**: MVC (Model-View-Controller)

## 🚀 Cómo Empezar

Sigue estos pasos para configurar y ejecutar el proyecto en tu máquina local.

### Prerrequisitos

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express edition es suficiente)
- [Git](https://git-scm.com/)

### Instalación y Configuración

1.  **Clona el repositorio**
    ```bash
    git clone https://github.com/CofanJoa/Mi-Barberia.git
    cd Mi-Barberia
    ```

2.  **Configura la aplicación**
    - Renombra el archivo `appsettings.Example.json` a `appsettings.json`.
    - Abre `appsettings.json` y modifica la cadena de conexión con los datos de tu servidor SQL Server local:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MiBarberia;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }
    ```

3.  **Crea la base de datos y aplica las migraciones**
    Ejecuta el siguiente comando en la terminal desde la raíz del proyecto. Esto creará la base de datos y todas las tablas necesarias, incluyendo usuarios predeterminados.
    ```bash
    dotnet ef database update
    ```

4.  **Ejecuta la aplicación**
    ```bash
    dotnet run
    ```
    La aplicación se abrirá en tu navegador predeterminado (normalmente en `https://localhost:7000` o `http://localhost:5000`).

## 🔐 Acceso al Sistema

La aplicación incluye usuarios predeterminados para testing. **Es crucial que cambies estas contraseñas después de la primera instalación.**

### Cuenta de Administrador
- **Email:** admin@barberia.com
- **Contraseña:** Admin123!
- **URL de Acceso Directo:** `https://localhost:7000/Identity/Account/Login`

**¿Qué puedes hacer como administrador?**
- Gestionar todos los servicios de barbería (crear, editar, eliminar).
- Gestionar la lista de barberos.
- Ver, editar y cancelar todas las citas agendadas.
- Gestionar horarios de atención.

### Cuenta de Cliente
- **Email:** cliente@test.com
- **Contraseña:** Cliente123!
- **URL de Acceso Directo:** `https://localhost:7000/Identity/Account/Login`

**¿Qué puedes hacer como cliente?**
- Explorar el catálogo de servicios.
- Ver los perfiles de los barberos.
- Agendar, modificar y cancelar tus propias citas.
- Ver tu historial de reservas.

## 📦 Estructura del Proyecto
Mi-Barberia/
├── Controllers/ # Controladores MVC (HomeController, TurnoController, etc.)
├── Models/ # Modelos de datos (Turno, Servicio, Barbero, etc.)
├── Views/ # Vistas Razor
├── Data/ # Contexto de base de datos y migraciones de Entity Framework
├── wwwroot/ # Archivos estáticos (CSS, JS, imágenes)
├── Areas/ # Áreas de administración (si aplica)
└── appsettings.json # Configuración de la aplicación (NO se sube al repo)

## 🗃️ Base de Datos

Este proyecto utiliza **Entity Framework Core Code-First** con migraciones automáticas. El comando `dotnet ef database update` se encargará de:
- Crear la base de datos si no existe
- Aplicar todos los esquemas de tablas
- Insertar datos iniciales esenciales (usuarios por defecto, servicios básicos, etc.)

## 🤝 Cómo Contribuir

Las contribuciones son bienvenidas. Si deseas mejorar este proyecto:

1.  Haz un Fork del proyecto.
2.  Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`).
3.  Realiza tus cambios y haz commit (`git commit -m 'Add some AmazingFeature'`).
4.  Push a la rama (`git push origin feature/AmazingFeature`).
5.  Abre un Pull Request.

## ⚠️ Notas de Seguridad Importantes

- **Cambia las contraseñas por defecto** inmediatamente después de la primera instalación.
- El archivo `appsettings.json` con tus cadenas de conexión reales está en `.gitignore` y no se sube al repositorio.
- Nunca commitees credenciales o información sensible.

## 📧 Contacto

Creado por [CofanJoa](https://github.com/CofanJoa). ¡Siéntete libre de contactarme si tienes preguntas!

---

**¿Problemas?** Asegúrate de que SQL Server esté ejecutándose y de que la cadena de conexión en `appsettings.json` sea correcta para tu entorno.
