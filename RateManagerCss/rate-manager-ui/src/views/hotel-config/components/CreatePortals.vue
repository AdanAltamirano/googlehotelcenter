<template>
  <div>
    <b-form @submit="create">
      <b-form-group id="input-group-1" :label="$t('Application Name')" label-for="input-1">
        <b-form-input
          id="input-1"
          type="text"
          v-model="nameApplication"
          required
          :placeholder="$t('Type Application Name')"
          class="col-8"
        ></b-form-input>
      </b-form-group>
      <div v-if="!isApiEndCorporate" class="vld-parent col-8" style="height:200px">
        <loading :active="true" :is-full-page="false" color="#007bff"></loading>
      </div>
      <div v-else>
        <div v-if="corporates.length > 0">
          <b-form-group
            :label="$t('Corporates')"
            style="height:130px;width:330px; overflow-y:scroll;"
          >
            <b-form-checkbox
              v-for="item in corporates"
              :key="item.id"
              :id="'corporate-' + item.id"
              v-model="corporateSelected"
              :name="item.name"
              :value="item.id"
              unchecked-value
              inline
              plain
              stacked
            >{{item.name}}</b-form-checkbox>
          </b-form-group>
        </div>
        <div v-else>
          <h5>{{$t('No Corporates')}}</h5>
        </div>
      </div>
      <b-button
        :disabled="isLoadingPortals"
        squared
        type="submit"
        variant="primary"
      >{{$t('Create Portals')}}</b-button>
    </b-form>
    <div>
      <div v-if="isCreatingPortals" class="vld-parent mt-2" style="height:200px">
        <loading :active="true" :is-full-page="false" color="#007bff" class="col-8"></loading>
      </div>
      <div v-else>
        <div class="mt-2" v-if="isApiCallEndPortals">
          <b-alert
            class="col-8"
            v-if="apiResultPortals === false"
            variant="danger"
            show
            dismissible
            fade
            @dismissed="isApiCallEndPortals=false"
          >{{apiResultBodyPortals}}</b-alert>
          <b-alert
            v-else
            variant="success"
            class="col-8"
            show
            dismissible
            fade
            @dismissed="isApiCallEndPortals=false"
          >{{apiResultBodyPortals}}</b-alert>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import configService from "../../../api/config-service";
import Loading from "vue-loading-overlay";
export default {
  name: "create-portals",
  components: {
    Loading
  },
  created() {
    this.Corporates();
    this.$root.$on("created-corporate", () => {
      this.Corporates();
    });
    this.$root.$on("creating-corporate", () => {
      this.isLoadingPortals = true;
    });
    this.$root.$on("complete-corporate", () => {
      this.isLoadingPortals = false;
    });
  },
  data() {
    return {
      corporateSelected: "",
      isApiEndCorporate: false,
      corporates: [],
      name: this.$appConfig.session.hotelName,
      nameApplication: this.$appConfig.session.hotelName,
      isCreatingPortals: false,
      isApiCallEndPortals: false,
      apiResultBodyPortals: "",
      apiResultPortals: false,
      isLoadingPortals: true
    };
  },
  methods: {
    Corporates() {
      this.isApiEndCorporate = false;
      this.isLoadingPortals = true;
      configService
        .GetCorporates()
        .then(result => {
          this.corporates = result.body;
        })
        .finally(() => {
          this.isApiEndCorporate = true;
          this.emitPortales();
          this.isLoadingPortals = false;
        });
    },
    create(e) {
      e.preventDefault();
      let portals = {
        Name: this.name,
        Description: this.name,
        Application: this.nameApplication,
        CoorporativeId: parseInt(this.corporateSelected),
        AsosciationId: null,
        SegmentId: null,
        Active: true,
        AfiliationId: null,
        CompanyId: null
      };

      this.isCreatingPortals = true;
      this.$root.$emit("creating-portals");
      this.isApiCallEndPortals = false;
      this.isLoadingPortals = true;
      this.createPortals(portals)
        .then(response => {
          this.apiResultPortals = response.ok;
          this.apiResultBodyPortals =
            this.$appConfig.language === "es"
              ? "Se han creado los portales"
              : "The portals has been created";
        })
        .catch(err => {
          this.apiResultPortals = err.ok;
          this.apiResultBodyPortals = err.body.errors[0].details[0].value;
        })
        .finally(() => {
          this.isCreatingPortals = false;
          this.isApiCallEndPortals = true;
          this.isLoadingPortals = false;
          //this.$root.$emit("complete-portals");
          this.emitPortales();
        });
    },
    createPortals(portals) {
      return configService.CreatePortals(portals);
    },
    emitPortales() {
      this.$root.$emit("portales");
    }
  }
};
</script>