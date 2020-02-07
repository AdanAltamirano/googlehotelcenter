<template>
  <div>
    <b-form @submit="create">
      <b-form-group id="input-group-1" :label="$t('Corporate Name')" label-for="input-1">
        <b-form-input
          id="input-1"
          type="text"
          v-model="name"
          required
          :placeholder="$t('Type Corporate Name')"
        ></b-form-input>
      </b-form-group>
      <div style="text-align:end;">
        <b-button
          :disabled="isLoading"
          squared
          type="submit"
          variant="primary"
        >{{$t('Create Corporate')}}</b-button>
      </div>
    </b-form>
    <div>
      <div v-if="isCreatingCoporate" class="vld-parent" style="height:200px">
        <loading :active="true" :is-full-page="false" color="#007bff"></loading>
      </div>
      <div v-else>
        <div class="mt-2" v-if="isApiCallEnd">
          <b-alert
            v-if="apiResult === false"
            variant="danger"
            show
            dismissible
            fade
            @dismissed="isApiCallEnd=false"
          >{{apiResultBody}}</b-alert>
          <b-alert
            v-else
            variant="success"
            show
            dismissible
            fade
            @dismissed="isApiCallEnd=false"
          >{{apiResultBody}}</b-alert>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import configService from "../../../api/config-service";
import Loading from "vue-loading-overlay";
export default {
  name: "create-corporative",
  components: {
    Loading
  },
  data() {
    return {
      name: "",
      isCreatingCoporate: false,
      isApiCallEnd: false,
      apiResult: false,
      apiResultBody: "",
      isLoading: true
    };
  },
  created() {
    this.$root.$on("portales", () => {
      this.isLoading = false;
    });
    this.$root.$on("creating-portals", () => {
      this.isLoading = true;
    });
    // this.$root.$on("complete-portals", () => {
    //   this.isLoading = false;
    // });
  },
  methods: {
    create(e) {
      e.preventDefault();

      let corporate = {
        Id: null,
        Name: this.name,
        CompanyId: null,
        Email: null,
        Billing: null,
        Catalog: null,
        Type: null
      };

      this.isCreatingCoporate = true;
      this.$root.$emit("creating-corporate");
      this.isApiCallEnd = false;
      this.isLoading = true;

      this.createCorporate(corporate)
        .then(response => {
          this.apiResult = response.ok;
          this.apiResultBody =
            this.$appConfig.language === "es"
              ? "Se ha creado el corporativo"
              : "The corporate has been created";
          this.sendMessageCreatePortals();
        })
        .catch(err => {
          this.apiResult = err.ok;
          this.apiResultBody = err.body.errors[0].details[0].value;
          this.$root.$emit("complete-corporate");
        })
        .finally(() => {
          this.isCreatingCoporate = false;
          this.isApiCallEnd = true;
          this.isLoading = false;
        });
    },
    createCorporate(corporate) {
      return configService.CreateCorporate(corporate);
    },
    sendMessageCreatePortals() {
      this.$root.$emit("created-corporate");
    }
  }
};
</script>