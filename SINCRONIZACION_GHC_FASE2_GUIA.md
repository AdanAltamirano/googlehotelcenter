# Guía de Implementación: Sincronización GHC Fase 2 (Outbox & Recovery)

Esta guía detalla los pasos para completar la Fase 2 del sistema de sincronización con Google Hotel Center, basándose en la infraestructura de auditoría y respaldo ya implementada.

## 1. Infraestructura de Persistencia
Se ha creado la tabla `GoogleSyncHistory` y el servicio `GoogleSyncAuditService`.
**Acción necesaria:** Ejecutar el script `GoogleSyncHistory_Schema.sql` en la base de datos de producción para habilitar el almacenamiento de respaldos.

## 2. Implementación del Patrón Outbox con Hangfire
Los controladores ya han sido refactorizados para registrar cada "intención" de sincronización en la tabla `GoogleSyncHistory`.

### ¿Qué es Hangfire y por qué lo usamos?
Hangfire es un **motor de ejecución de tareas de fondo**. En este diseño, existe una distinción clara entre el **Worker** y el **Motor (Hangfire)**:

*   **GoogleSyncWorker (La Lógica):** Es el código que yo implementé. Sabe *cómo* leer la base de datos, *cómo* extraer el XML de respaldo y *cómo* llamar al API de Google. Sin embargo, por sí solo, el Worker no sabe cuándo ejecutarse ni qué hacer si el servidor se apaga a mitad del proceso.
*   **Hangfire (El Motor):** Es el responsable de "despertar" al Worker. Hangfire garantiza que si una tarea queda pendiente, se ejecute pase lo que pase.

### Beneficios Clave vs. Task.Run o un Worker simple:
1.  **Persistencia (Respaldo Real):** A diferencia de `Task.Run`, que vive en la memoria RAM, Hangfire guarda la cola de tareas en SQL Server. Si el servidor se reinicia o el Pool de IIS se recicla, Hangfire retoma las sincronizaciones pendientes exactamente donde se quedaron.
2.  **Dashboard de Monitoreo:** Hangfire incluye una interfaz visual (/hangfire) donde puedes ver en tiempo real qué sincronizaciones están en cola, cuáles fallaron y por qué (con el detalle del error).
3.  **Reintentos Automáticos:** Si el API de Google está caído temporalmente, Hangfire no se rinde. Reintenta la sincronización con un "backoff" (esperando cada vez un poco más) automáticamente.
4.  **Liberación de la UI:** El usuario que edita una tarifa no tiene que esperar los 5-10 segundos que tarda Google en responder. El sistema guarda en la DB (milisegundos) y Hangfire se encarga del resto "detrás de cámaras".

**Configuración Sugerida:** Crear un Job recurrente (ej. cada 2 minutos) que llame a `APIServices.GoogleSyncWorker.ProcessPendingSyncs()`.

## 3. Lógica de Recuperación (Recovery)
El `GoogleSyncWorker` ya cuenta con el método `ReplaySync` que utiliza el `RequestXML` respaldado para reintentar la operación.
**Acción necesaria:** Asegurar que las URLs del API de Conflux en el `Web.config` sean accesibles desde el servidor donde corra el Worker.

## 4. Visualización en la UI (Confirmación)
Se ha agregado la columna "GHC Status" en `RatesPlans.aspx`.
**Siguiente Paso:**
1. Crear un endpoint `api/sync/status/{ratePlanId}` que devuelva el último estado de la tabla `GoogleSyncHistory`.
2. Implementar una función JavaScript `updateGHCStatus()` que consulte este endpoint al cargar la página y actualice las etiquetas en el DataGrid.

## 5. Pasos paso a paso para el Desarrollador
1. **Paso 1:** Verificar que los logs se están guardando en `GoogleSyncHistory` al editar una tarifa.
2. **Paso 2:** Instalar el paquete NuGet `Hangfire.AspNet` en el proyecto principal.
3. **Paso 3:** En `Global.asax`, inicializar el servidor de Hangfire y configurar la tarea recurrente.
4. **Paso 4:** Probar el reintento forzando un error de red y verificando que el Worker cambie el estado de 'Failed' a 'Success' tras el reintento.
5. **Paso 5:** Completar el frontend para mostrar el check verde o la alerta roja en la columna "GHC Status".

---
*Nota: Con esta fase, el sistema pasa de una sincronización "al mejor esfuerzo" a una sincronización "garantizada con respaldo".*

---
### Nota sobre Hangfire
Como se discutió, la implementación de Hangfire es una **mejora opcional y recomendada**, no definitiva. Por el momento, el sistema sigue utilizando hilos asíncronos (), pero con la ventaja añadida de que ahora cada operación queda **respaldada en la tabla **. Esto permite una auditoría manual y reintentos programados en el futuro sin perder datos.

---
### Nota sobre Hangfire
Como se discutió, la implementación de Hangfire es una **mejora opcional y recomendada**, no definitiva. Por el momento, el sistema sigue utilizando hilos asíncronos (Task.Run), pero con la ventaja añadida de que ahora cada operación queda **respaldada en la tabla GoogleSyncHistory**. Esto permite una auditoría manual y reintentos programados en el futuro sin perder datos.
