# Tickets Service

**Tickets Service** es un microservicio diseñado para gestionar los boletos de los pasajeros de manera segura y eficiente, permitiendo crear, consultar, actualizar y eliminar tickets, manteniendo la integridad de los datos y facilitando la gestión del transporte.

---

## Funcionalidades

### Crear ticket

Permite registrar un nuevo boleto para un pasajero, asegurando que cada registro sea único y consistente. Se valida que no exista otro ticket duplicado para el mismo pasajero en la misma fecha(dia).

**Información registrada:**

- ID del ticket (asignacion automatica)
- ID del pasajero (referencia al Usuario)
- Fecha y hora de emisión del ticket
- Tipo de ticket (ida o vuelta)
- Estado del ticket (activo, usado, caducado)
- Monto pagado (mayor a 0.1)

### Visualizar tickets

Permite consultar todos los boletos registrados.

**Datos mostrados:**

- ID del ticket
- ID del pasajero (referencia al Usuario)
- Fecha y hora de emisión
- Tipo de ticket (ida o vuelta)
- Estado del ticket (activo, usado, caducado)
- Monto pagado

### Visualizar ticket por ID

Permite obtener información de un ticket mediante su identificador.

- Muestra los mismos datos que Visualizar tickets, **exceptuando el estado del ticket**.

### Editar ticket

Permite actualizar de manera segura los datos de un boleto, verificando la integridad, validez y consistencia de los registros.

**Campos editables:**

- Estado del ticket (activo, usado, caducado)
- Tipo de ticket (ida o vuelta)
- Fecha y hora de emisión
- Monto pagado

Casos a considerar:

- No se permite reactivar un ticket usado.
- No se permite cambiar el estado de un ticket caducado.
- Se valida que no exista otro ticket duplicado para el mismo pasajero.

### Eliminar ticket

Desactiva temporalmente un boleto sin eliminar su registro físicamente mediante **SOFT DELETE**.

- Marca el ticket como inactivo.
- Solo puede ser realizado por administradores.
- Preserva la trazabilidad del sistema.

---

## Arquitectura y patrón de diseño

- Microservicio implementado en **ASP.NET Core**.
- Uso de **Docker** para contenerización y despliegue.
- Patrón de diseño basado en **repositorios** y servicios para separar lógica de negocio y acceso a datos.
- Integración con base de datos MongoDB en la nube mediante un driver directo en el código.
- Documentación de la API mediante **Swagger**.

---

## Ejecución local

1. Clonar el repositorio:

```bash
git clone <URL_DEL_REPOSITORIO>
cd <CARPETA_DEL_PROYECTO>
```

2. Construir la imagen de Docker:

```bash
docker build -t c4rloss44440/tickets-api-deploy .
```

3. Levantar el contenedor:

```bash
docker compose up
```

4. Acceder a la documentación y probar la API:
   [http://localhost:3000/swagger/index.html](http://localhost:3000/swagger/index.html)

## Ejecución en la nube

La API está desplegada y disponible en:
[https://tickets-api-deploy.onrender.com/swagger/index.html](https://tickets-api-deploy.onrender.com/swagger/index.html)
