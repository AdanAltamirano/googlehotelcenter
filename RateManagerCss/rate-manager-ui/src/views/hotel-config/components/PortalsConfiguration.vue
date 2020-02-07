<template>
  <div class="mt-3">
    <h2 class="text-primary">{{$t('Portals Configuration')}}</h2>
    <b-card>
      <b-card-body>
        <b-row>
          <b-col cols="8">
            <h5 class="card-title text-primary">{{$t('Create Portals')}}</h5>
            <create-portals></create-portals>
          </b-col>
          <b-col cols="4">
            <h5 class="text-primary" style="text-align:end;">{{$t('Create Corporate')}}</h5>
            <create-corporative></create-corporative>
          </b-col>
        </b-row>
      </b-card-body>
    </b-card>
  </div>
</template>
<script>
import CreateCorporative from "./CreateCorporative.vue";
import CreatePortals from "./CreatePortals.vue";
export default {
  name: "portals-configuration",
  components: {
    CreateCorporative,
    CreatePortals
  }
};
</script>