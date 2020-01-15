<template>
  <!-- eslint-disable -->
  <div id="app">
    <!--Busqueda-->
    <b-container fluid>
      <f2-g-search></f2-g-search>
    </b-container>
  </div>
  <!-- eslint-enable -->
</template>
<script>
import F2GSearch from "./components/F2GSearch.vue";
export default {
  name: "app",
  components: {
    F2GSearch
  }
};
</script>