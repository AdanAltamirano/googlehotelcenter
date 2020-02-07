<template>
  <div>
    <div v-if="!apiGetCorporates" class="vld-parent" style="height:200px">
      <loading :active="true" :is-full-page="false" color="#007bff"></loading>
    </div>
    <div v-else>
      <h2 style="color:#10467a;">{{$t('Front2Go Rates Configuration')}}</h2>
      <b-row>
        <b-col md="6" class="my-1">
          <b-row>
            <b-col md="7">
              <b-form-group description="Lista de corporativos">
                <b-form-select v-model="selected" :options="corporates">
                  <template slot="first">
                    <option :value="null" disabled>-- {{$t('Select a corporate')}} --</option>
                  </template>
                </b-form-select>
              </b-form-group>
            </b-col>
            <b-col md="5">
              <b-button
                variant="primary"
                @click="SearchF2GConfigurationByCorporate"
              >{{$t('Search')}}</b-button>
            </b-col>
          </b-row>
        </b-col>
        <!-- /.b-col -->
        <b-col md="6" class="my-1">
          <b-row>
            <b-col md="8">
              <b-row>
                <b-col md="10">
                  <b-button
                    :disabled="disable"
                    v-b-toggle.collapse-1
                    class="float-right"
                    variant="primary"
                  >{{$t('F2G New Code')}}</b-button>
                </b-col>
                <b-col md="2">
                  <b-button @click="Info" style="border:none; outline:none; background:none;">
                    <i class="fas fa-question-circle fa-lg" style="color:#10467a !important"></i>
                  </b-button>
                </b-col>
              </b-row>
              <!-- <b-button
                :disabled="disable"
                v-b-toggle.collapse-1
                class="float-right"
                variant="primary"
              >{{$t('F2G New Code')}}</b-button>-->
              <!-- <b-button size="sm" style="border:none; outline:none; background:none;">
                <i class="fas fa-question-circle fa-lg" style="color:#10467a !important"></i>
              </b-button>-->
              <!-- /.ayuda -->
            </b-col>
            <b-col md="4" style="margin-top:-1.9rem;">
              <b-form-group :label="$t('elements per page')" class="float-right">
                <b-form-select class="float-right" v-model.number="perPage" :options="itemsPerPage"></b-form-select>
              </b-form-group>
            </b-col>
          </b-row>
        </b-col>
      </b-row>
      <b-row>
        <b-col md="12">
          <b-collapse v-model="isVisible" id="collapse-1">
            <b-card>
              <b-row class="mt-3">
                <b-col md="4" offset-md="1">
                  <b-form-group :description="$t('F2G Code')">
                    <b-form-input v-on:keyup="onKeyUpF2GCode" id="code" v-model="f2gCode"></b-form-input>
                  </b-form-group>
                </b-col>
                <b-col md="4">
                  <b-form-group :description="$t('Promotion Code')">
                    <b-form-input v-on:keyup="onKeyUpF2GPromo" id="promo" v-model="f2gPromoCode"></b-form-input>
                  </b-form-group>
                </b-col>
                <b-col md="3">
                  <b-button
                    @click="SaveNewF2G"
                    :disabled="f2gCode === ''"
                    variant="primary"
                  >{{$t('Save')}}</b-button>
                </b-col>
              </b-row>
              <b-row>
                <b-col md="12">
                  <b-alert show variant="danger" dismissible>{{alert}}</b-alert>
                </b-col>
              </b-row>
            </b-card>
          </b-collapse>
        </b-col>
      </b-row>
      <f2-g-configuration :perPage="perPage"></f2-g-configuration>
    </div>
  </div>
</template>
<script>
import F2GService from "../../../api/f2g-service";
import Loading from "vue-loading-overlay";
import F2GConfiguration from "./F2GConfiguration.vue";
import _ from "lodash";
export default {
  name: "f2gsearch",
  components: {
    Loading,
    F2GConfiguration
  },
  data() {
    return {
      disable: true,
      apiGetCorporates: false,
      selected: null,
      corporates: [],
      f2gCode: "",
      f2gPromoCode: "",
      itemsPerPage: [10, 30, 50, 100],
      perPage: 10,
      isVisible: false
    };
  },
  beforeCreate() {
    F2GService.GetF2GCorporates()
      .then(result => {
        this.corporates = result.body;
        this.apiGetCorporates = true;
      })
      .catch(err => {
        this.apiGetCorporates = true;
      });
  },
  computed: {
    alert() {
      if (window.app.language === "es") {
        return "El nuevo código solo va estar disponible en la tabla hasta que se guarde un hotel o/u hoteles con el nuevo código";
      } else {
        return "The new code will only be available in the table until a hotel or hotels with the new code are saved";
      }
    }
  },
  methods: {
    SearchF2GConfigurationByCorporate() {
      this.isVisible = false;
      this.disable = true;
      let id = _.find(this.corporates, ["value", this.selected]).id;
      this.$root.$emit("corporateId", id);
    },
    SaveNewF2G() {
      this.f2gPromoCode = this.f2gPromoCode.replace(/Í/g, "I");
      this.f2gPromoCode = this.f2gPromoCode.replace(/Ó/g, "O");
      this.f2gPromoCode = this.f2gPromoCode.replace(/É/g, "E");
      this.f2gPromoCode = this.f2gPromoCode.replace(/Á/g, "A");
      this.f2gPromoCode = this.f2gPromoCode.replace(/Ú/g, "U");
      if (
        this.f2gPromoCode.indexOf("SIN CODIGO DE PROMOCION") != -1 ||
        this.f2gPromoCode.indexOf("NO PROMOTION CODE") != -1
      ) {
        this.$appAlert({
          type: "error",
          title: this.$t("Promotion not valid"),
          confirmButtonText: this.$t("Exit"),
          confirmButtonColor: "#d33"
        });
      } else {
        this.$root.$emit(
          "newF2G",
          this.f2gCode.trim(),
          this.f2gPromoCode.trim()
        );
        this.isVisible = false;
      }
    },
    onKeyUpF2GCode() {
      this.f2gCode = this.f2gCode.toUpperCase();
    },
    onKeyUpF2GPromo() {
      this.f2gPromoCode = this.f2gPromoCode.toUpperCase();
    },
    Info() {
      let message =
        window.app.language === "es"
          ? "Para agregar un nuevo código F2G el corporativo debe de tener hoteles y planes tarifarios"
          : "To add a new F2G code the corporate must have hotels and rate plans";

      this.$appAlert({
        type: "info",
        text: message,
        confirmButtonText: this.$t("Exit"),
        confirmButtonColor: "#d33"
      });
    }
  },
  mounted() {
    this.$root.$on("enableNewF2GButton", () => {
      this.disable = false;
    });

    this.$root.$on("disableNewF2GButton", () => {
      this.disable = true;
    });
  }
};
</script>