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

## Recomendaciones Inmediatas
1. **Implementar una Cola de Mensajes (Outbox Pattern):** En lugar de `Task.Run`, guardar las actualizaciones pendientes en una tabla de la DB y procesarlas con un servicio en segundo plano que garantice reintentos en caso de fallo.
2. **Validación Visual del Límite de Google:** En lugar de descartar la tarifa silenciosamente en el código interno (APIServices), implementar una validación en la interfaz de usuario que advierta al administrador cuando una tarifa excede el límite permitido por Google antes de intentar guardarla.
3. **Mejorar el Logging de Errores:** Asegurar que cualquier fallo en la comunicación con Conflux/Google sea visible de forma prominente en el panel de administración del hotel.
4. **Sincronización de Consistencia:** Crear un proceso nocturno que compare la base de datos local con lo que tiene Google Hotel Center y corrija las discrepancias automáticamente.
