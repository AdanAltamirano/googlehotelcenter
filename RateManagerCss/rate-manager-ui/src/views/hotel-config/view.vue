<template>
  <!-- eslint-disable -->
  <div id="app">
    <b-container fluid>
      <!-- Configuracion del hotel -->
      <hotel-configuration></hotel-configuration>
      <!-- Configuracion de los planes tarifarios -->
      <rates-configuration></rates-configuration>
      <!-- Configuracion de los portales -->
      <!--<portals-configuration></portals-configuration>-->
      <!-- Configuracion Portales -->
    </b-container>
  </div>
  <!-- eslint-enable -->
</template>
<script>
import HotelConfiguration from "./components/HotelConfiguration.vue";
import RatesConfiguration from "./components/RatesConfiguration.vue";
import PortalsConfiguration from "./components/PortalsConfiguration.vue";
export default {
  name: "app",
  components: {
    HotelConfiguration,
    RatesConfiguration,
    PortalsConfiguration
  }
};
</script>
