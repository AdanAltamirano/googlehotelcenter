# Análisis de Puntos Débiles: Sincronización con Google Hotel Center (GHC)

Este documento detalla las debilidades identificadas en los procesos de creación, edición y eliminación de tarifas, promociones y planes tarifarios, enfocándose en las causas que provocan fallos en las actualizaciones automáticas a Google Hotel Center.

---

## 1. Debilidades Técnicas y de Arquitectura

### 1.1 Uso de Tareas "Fire-and-Forget" (`Task.Run`)
**Ubicación:** `OffersController.vb`, `RatesController.vb`, `ctrRatePlan.ascx.vb`, `RatesPlans.aspx.vb`.
- **Descripción:** Gran parte de las llamadas a `ConfluxService` para sincronizar con Google se realizan dentro de `Task.Run` sin esperar el resultado (`await`) en el hilo principal de la solicitud web.
- **Impacto:**
    - **Fallos Silenciosos:** Si la tarea falla por red, timeout o error en el API de Conflux, el usuario recibe una respuesta de "Éxito" en la UI, pero Google nunca se actualiza.
    - **Pérdida de Contexto:** Al ser hilos separados, errores críticos no se reportan adecuadamente al log principal de la aplicación en el momento del fallo.
    - **Inestabilidad:** Si el Pool de Aplicaciones de IIS se reinicia mientras hay tareas pendientes en `Task.Run`, esas actualizaciones se pierden permanentemente.

### 1.2 Falta de Atomicidad (DB vs. GHC)
**Descripción:** La base de datos local se actualiza primero y, posteriormente, se intenta sincronizar con Google.
- **Impacto:** Si la base de datos se guarda correctamente pero la llamada al API de Google falla (o la tarea asíncrona muere), los datos quedan desincronizados: la web muestra una tarifa pero Google muestra otra antigua. No existe un mecanismo de reintento automático para estas discrepancias.

---

## 2. Debilidades en la Lógica de Negocio y Parsers

### 2.1 Manejo del Límite de Precio de Google (170,000)
**Ubicación:** `APIServices/Conflux/Parser/Parser.cs`, métodos `GeneralAuxCreateRateAmountMessage` y `RateAuxCreateRateAmountMessage`.
- **Descripción:** El sistema implementa una restricción propia de Google Hotel Center que prohíbe el envío de tarifas superiores a **170,000**.
- **Debilidad Identificada:** Aunque el límite es una regla externa obligatoria, el sistema actual **ignora silenciosamente** cualquier tarifa que supere este monto durante la generación del mensaje (parser).
- **Impacto:** Si un hotel carga una tarifa superior a este umbral (especialmente común en monedas con valores nominales altos), el proceso de sincronización simplemente descarta el registro. Al no haber una notificación o error visible para el usuario, el administrador percibe que la sincronización "falló" sin causa aparente, cuando en realidad fue un descarte preventivo no informado.

### 2.2 Sincronización Desacoplada (Precios vs. Disponibilidad)
**Ubicación:** `RatesController.vb`, métodos `ExecuteServices` y `SendRatesIfEnabledAsync`.
- **Descripción:** El envío de precios (`UpdateRate`) y el envío de cierres/restricciones (`SendClosureToService`) son llamadas independientes.
- **Impacto:** Si una llamada tiene éxito y la otra falla, Google podría tener el precio correcto pero la habitación cerrada (o viceversa), causando inconsistencias en la disponibilidad real del hotel en el buscador.

### 2.3 Lógica de Filtrado Interno (Segmentos y Banderas)
**Ubicación:** `APIServices/Conflux/Parser/Parser.cs`.
- **Descripción:** El sistema filtra tarifas basándose en `segmentsNoRates` (configurado en Web.config), `IsMobileRate` e `IsCallCenterOnly`.
- **Impacto:** Si un hotel cambia su estrategia y marca una tarifa como "Call Center Only", esta se elimina de Google, pero si el proceso de borrado falla, la tarifa seguirá apareciendo en Google aunque ya no sea válida para el canal online, provocando quejas de clientes por precios no honrados.

---

## 3. Debilidades en la Gestión de Datos

### 3.1 Complejidad en Traslapes de Fechas
**Ubicación:** `RatesService.cs` (Método `CopyOverlappedRates`).
- **Descripción:** Cuando se inserta una tarifa que solapa con una existente, el sistema local recorta o divide la tarifa antigua.
- **Impacto:** La lógica para traducir estos cambios complejos de la base de datos a mensajes OTA de "Delete" y "Update" para Google es propensa a errores. Un error en el cálculo de los nuevos rangos de fechas puede dejar "huecos" sin tarifa o periodos donde la tarifa vieja persiste en Google.

### 3.2 Manejo de Eliminaciones Lógicas
- **Descripción:** Al eliminar un RatePlan en la UI, se llama a `DeleteAsync`.
- **Impacto:** La obtención de los mensajes de borrado (`GetDeleteMessagesGoogle`) depende de que `spGetCurrentRatesByHotel` devuelva los registros marcados como eliminados. Si hay inconsistencias en las banderas de eliminación en la DB, el proceso de limpieza en Google Hotel Center no se ejecutará para todos los rangos de fechas afectados.

---

## Estrategias de Solución Recomendadas

Para resolver los fallos de sincronización, se proponen tres niveles de intervención, desde correcciones rápidas hasta cambios arquitectónicos de fondo.

### Nivel 1: Correcciones Técnicas Inmediatas (Quick Wins)
1. **Sustitución de `Task.Run` por persistencia:** En lugar de disparar una tarea asíncrona volátil, registrar la intención de actualización en una tabla de "Pendientes de Sincronización" dentro de la misma transacción de la base de datos.
2. **Validación Previa (Pre-flight Checks):** Antes de permitir el guardado en la UI, ejecutar la lógica del parser para detectar errores conocidos (como el límite de 170,000 o falta de campos obligatorios) y mostrar advertencias al usuario.
3. **Logging Orientado a Diagnóstico:** Centralizar los logs de comunicación con Google vinculándolos a un `Correlation ID` que permita rastrear una edición en la UI desde que se guarda en la DB hasta que el API de Google devuelve (o no) una respuesta.

### Nivel 2: Mejoras de Procesos y Visibilidad
4. **Dashboard de Estado de Sincronización:** Crear una pantalla donde el administrador pueda ver en tiempo real cuántas tarifas están "Sincronizadas", "Pendientes" o "Fallidas". Esto elimina la incertidumbre de si el cambio llegó a Google.
5. **Botón de "Sincronización Forzada":** Implementar una función que permita al administrador reenviar todos los precios y cierres de un plan tarifario específico a Google, útil para corregir discrepancias puntuales sin tener que editar los datos.
6. **Mecanismo de Reintento con Backoff:** Si el API de Conflux falla por problemas de red o saturación, el sistema debe reintentar automáticamente en intervalos crecientes (ej. 1 min, 5 min, 15 min) antes de marcarlo como error definitivo.

### Nivel 3: Cambios de Arquitectura (Solución Definitiva)
7. **Patrón Outbox con Worker Dedicado:** Implementar un servicio de Windows o un Job (ej. Hangfire) que procese la tabla de "Pendientes". Este worker debe ser el único responsable de la comunicación externa, garantizando que si el servidor se reinicia, el trabajo se reanude donde quedó.
8. **Sincronización Basada en Estado (Checksums):** Almacenar un "Hash" o firma de la última versión de la tarifa enviada exitosamente a Google. Un proceso de auditoría puede comparar periódicamente el Hash de la DB local contra el enviado a Google para detectar y corregir desincronizaciones silenciosas automáticamente.
9. **Consolidación de Mensajes (Bundling):** Agrupar actualizaciones de precios y disponibilidad en un solo proceso lógico para asegurar que Google reciba la imagen completa de la oferta del hotel, evitando estados inconsistentes donde el precio es nuevo pero el cierre es viejo.
