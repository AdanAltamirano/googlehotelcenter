<template>
  <div>
    <div
      v-if="apiGetConfiguration != null && !apiGetConfiguration"
      class="vld-parent"
      style="height:200px"
    >
      <loading :active="true" :is-full-page="false" color="#007bff"></loading>
    </div>
    <div v-else>
      <b-container fluid>
        <!-- <h2 class="text-primary">Prueba</h2> -->
        <b-table
          class="my-2"
          show-empty
          striped
          bordered
          hover
          responsive
          :fields="fields"
          :items="configuration"
          :per-page="perPage"
          :current-page="currentPage"
          :empty-text="emptyText"
          :sort-by.sync="sortBy"
        >
          <template slot="empty" slot-scope="scope">
            <h5 class="text-center">{{scope.emptyText}}</h5>
          </template>
          <!--actions-->
          <template slot="actions" slot-scope="row">
            <!--Add Button-->
            <b-button
              size="sm"
              :pressed.sync="row.item.addState"
              :disabled="row.item.deleteState"
              @click="row.toggleDetails"
              class="mr-2"
              style="background-color:#10467a; border:none;"
            >
              <i class="fas fa-plus"></i>
              &nbsp;
              <i class="fas fa-hotel"></i>
            </b-button>&nbsp;
            <!--Delete Button-->
            <b-button
              id="delete"
              size="sm"
              :pressed.sync="row.item.deleteState"
              :disabled="row.item.addState"
              @click="row.toggleDetails"
              class="mr-2"
              style="background-color:#dc3545; border:none;"
            >
              <i class="fas fa-minus"></i>
              &nbsp;
              <i class="fas fa-hotel"></i>
            </b-button>
          </template>
          <!--details-->
          <template slot="row-details" slot-scope="row">
            <b-card style="height:auto;">
              <b-row class="mb-2">
                <b-col md="12" v-if="row.item.addState">
                  <b-row>
                    <b-col md="4">
                      <b-form-group :description="$t('Hotels List')">
                        <multiselect
                          label="name"
                          v-model="row.item.hotel"
                          :options="row.item.hotels"
                          track-by="hotelId"
                          :multiple="true"
                          :selectLabel="$t('Select')"
                          :selectedLabel="''"
                          :deselectLabel="''"
                          :placeholder="$t('Search Hotels')"
                          :max-height="150"
                        ></multiselect>
                        <!-- /. hoteles -->
                      </b-form-group>
                      <b-button
                        @click="Save(row)"
                        class="mr-2"
                        style="background-color:#10467a; border:none;"
                        :disabled="!row.item.hotel.length > 0 
                        || !row.item.ratesList.length > 0 "
                      >{{$t('Save')}}</b-button>
                    </b-col>
                    <b-col md="4">
                      <b-form-group :description="$t('Rate Plans List')">
                        <multiselect
                          label="name"
                          v-model="row.item.ratesList"
                          :options="row.item.rates"
                          track-by="ratePlanId"
                          :multiple="true"
                          :selectLabel="$t('Select')"
                          :selectedLabel="''"
                          :deselectLabel="''"
                          :placeholder="$t('Search Rate Plans')"
                          :max-height="150"
                        ></multiselect>
                        <!-- /. planes tarifarios -->
                      </b-form-group>
                    </b-col>
                    <b-col md="4">
                      <b-button
                        @click="MostrarHoteles(row)"
                        style="background-color:#10467a; border:none;"
                      >{{$t('Mapped Hotels')}}</b-button>
                    </b-col>
                  </b-row>
                </b-col>
                <!-- /. guardar --->
                <b-col md="12" v-else-if="row.item.deleteState">
                  <b-row>
                    <b-col md="4">
                      <b-form-group description="Lista de hoteles">
                        <multiselect
                          label="name"
                          v-model="row.item.hotel"
                          :options="row.item.hotels"
                          track-by="hotelId"
                          :multiple="true"
                          :selectLabel="$t('Select')"
                          :selectedLabel="''"
                          :deselectLabel="''"
                          :placeholder="$t('Search Hotels')"
                          :max-height="150"
                        ></multiselect>
                        <!-- /. hoteles -->
                      </b-form-group>
                      <b-button
                        @click="Delete(row)"
                        class="mr-2"
                        style="background-color:#dc3545; border:none;"
                        :disabled="!row.item.hotel.length > 0 
                        || !row.item.ratesList.length > 0 "
                      >Borrar</b-button>
                    </b-col>
                    <b-col md="4">
                      <b-form-group description="Lista de planes tarifarios">
                        <multiselect
                          label="name"
                          v-model="row.item.ratesList"
                          :options="row.item.rates"
                          track-by="ratePlanId"
                          :multiple="true"
                          :selectLabel="$t('Select')"
                          :selectedLabel="''"
                          :deselectLabel="''"
                          :placeholder="$t('Search Rate Plans')"
                          :max-height="150"
                        ></multiselect>
                        <!-- /. planes tarifarios -->
                      </b-form-group>
                    </b-col>
                    <!-- /.borrar -->
                    <b-col md="4">
                      <b-button
                        @click="MostrarHoteles(row)"
                        style="background-color:#dc3545; border:none;"
                      >{{$t('Mapped Hotels')}}</b-button>
                    </b-col>
                  </b-row>
                </b-col>
              </b-row>
            </b-card>
            <!-- /.card -->
          </template>
        </b-table>
        <b-pagination
          v-model="currentPage"
          :total-rows="rows"
          :per-page="perPage"
          aria-controls="my-table"
          align="right"
        ></b-pagination>
      </b-container>
    </div>
  </div>
</template>
<script>
import Multiselect from "vue-multiselect";
import Vue from "vue";
import HotelsByCode from "./HotelsByCode.vue";
import F2GService from "../../../api/f2g-service";
import Loading from "vue-loading-overlay";
import _ from "lodash";
export default {
  name: "f2g-configuration",
  components: {
    Multiselect,
    Loading
  },
  props: ["perPage"],
  data() {
    return {
      sortBy: "ratePlanIdF2G",
      emptyText: this.$t("No results found"),
      apiGetConfiguration: null,
      configuration: [],
      fields: [
        {
          key: "ratePlanIdF2G",
          label: this.$t("F2G Code"),
          sortable: true
        },
        {
          key: "promoCode",
          label: this.$t("Promotion Code")
        },
        {
          key: "actions",
          label: this.$t("Actions")
        }
      ],
      currentPage: 1,
      fieldsHotelsMapped: [
        { key: "ratePlanIdUv", label: this.$t("Rate Plan") },
        { key: "name", label: this.$t("Hotel Name") }
      ],
      corporateId: null,
      hotels: [],
      rates: []
    };
  },
  mounted() {
    this.$root.$on("corporateId", id => {
      this.apiGetConfiguration = false;
      this.corporateId = id;
      F2GService.GetF2GConfigurationByCorporateId(id)
        .then(result => {
          //this.apiGetConfiguration = true;
          this.configuration = result.body;
        })
        .catch(err => {
          //this.apiGetConfiguration = true;
        });
      F2GService.GetF2GHotelsRates(id)
        .then(result => {
          this.apiGetConfiguration = true;
          this.hotels = result.body.hotels;
          this.rates = result.body.rates;

          if (this.hotels.length > 0 && this.rates.length > 0) {
            this.$root.$emit("enableNewF2GButton");
          } else {
            this.$root.$emit("disableNewF2GButton");
          }
        })
        .catch(err => {
          this.apiGetConfiguration = true;
        });
    });

    this.$root.$on("newF2G", (f2gCode, f2gPromoCode) => {
      console.log(f2gPromoCode);
      let containsF2G = this.ContainsF2G(f2gCode, f2gPromoCode);

      if (containsF2G) {
        this.$appAlert({
          type: "error",
          title: this.$t("The code already exists"),
          confirmButtonText: this.$t("Exit"),
          confirmButtonColor: "#d33"
        });
      } else {
        let promo = !f2gPromoCode
          ? window.app.language === "es"
            ? "Sin código de promoción"
            : "No promotion code"
          : f2gPromoCode;

        let newF2G = {
          ratePlanIdF2G: f2gCode,
          promoCode: promo,
          hasPromo: !f2gPromoCode ? false : true,
          hotelsMapped: [],
          hotels: this.hotels,
          rates: this.rates,
          addState: false,
          deleteState: false,
          hotel: [],
          ratesList: []
        };

        this.configuration.push(newF2G);
      }
    });
  },
  computed: {
    rows() {
      return this.configuration.length;
    }
  },
  methods: {
    MostrarHoteles(row) {
      let component = Vue.extend(HotelsByCode);
      let instance = new component({
        propsData: {
          fields: this.fieldsHotelsMapped,
          rates_hotels_list: row.item.hotelsMapped,
          empty: this.emptyText
        }
      });
      instance.$mount();

      let list = $("<div>").append(instance.$el);

      this.$appAlert({
        title: this.$t("Hotels with Rate Plan"),
        type: "info",
        confirmButtonText: this.$t("Exit"),
        confirmButtonColor: "#d33",
        html: list,
        customClass: {
          content: "rates-hotels-table"
        }
      });
    },
    Save(row) {
      let f2gSaveModel = {
        F2GId: row.item.ratePlanIdF2G,
        Promo: row.item.promoCode,
        Hotels: row.item.hotel,
        Rates: row.item.ratesList,
        Action: 1
      };

      this.apiGetConfiguration = false;

      F2GService.UpdateF2G(f2gSaveModel)
        .then(result => {
          this.$appAlert({
            type: "success",
            title: this.$t("Saved Hotels"),
            confirmButtonText: this.$t("Exit"),
            confirmButtonColor: "#d33"
          });
          F2GService.GetF2GConfigurationByCorporateId(this.corporateId)
            .then(result => {
              this.apiGetConfiguration = true;
              this.configuration = result.body;
            })
            .catch(err => {
              this.apiGetConfiguration = true;
            });
        })
        .catch(err => {
          this.apiGetConfiguration = true;
          this.$appAlert({
            type: "error",
            title: this.$t("Hotels were not saved"),
            confirmButtonText: this.$t("Exit"),
            confirmButtonColor: "#d33"
          });
        });
    },
    Delete(row) {
      let f2gSaveModel = {
        F2GId: row.item.ratePlanIdF2G,
        Promo: row.item.promoCode,
        Hotels: row.item.hotel,
        Rates: row.item.ratesList,
        Action: 2
      };

      this.apiGetConfiguration = false;

      F2GService.UpdateF2G(f2gSaveModel)
        .then(result => {
          this.$appAlert({
            type: "success",
            title: this.$t("Hotels Deleted"),
            confirmButtonText: this.$t("Exit"),
            confirmButtonColor: "#d33"
          });
          F2GService.GetF2GConfigurationByCorporateId(this.corporateId)
            .then(result => {
              this.apiGetConfiguration = true;
              this.configuration = result.body;
            })
            .catch(err => {
              this.apiGetConfiguration = true;
            });
        })
        .catch(err => {
          this.apiGetConfiguration = true;
          this.$appAlert({
            type: "error",
            title: this.$t("Hotels were not deleted"),
            confirmButtonText: this.$t("Exit"),
            confirmButtonColor: "#d33"
          });
        });
    },
    ContainsF2G(f2gCode, f2gPromoCode) {
      let hasPromo = true;

      if (f2gPromoCode) {
        console.log("Promocion: " + f2gPromoCode);
        f2gPromoCode = f2gPromoCode.replace(/Í/g, "I");
        f2gPromoCode = f2gPromoCode.replace(/Ó/g, "O");
        f2gPromoCode = f2gPromoCode.replace(/É/g, "E");
        f2gPromoCode = f2gPromoCode.replace(/Á/g, "A");
        f2gPromoCode = f2gPromoCode.replace(/Ú/g, "U");
      } else {
        hasPromo = false;
      }

      let f2gPromo = !f2gPromoCode
        ? window.app.language === "es"
          ? "Sin código de promoción"
          : "No promotion code"
        : f2gPromoCode;

      let containF2GCode = _.find(this.configuration, {
        ratePlanIdF2G: f2gCode,
        promoCode: f2gPromo,
        hasPromo: hasPromo
      });

      if (!containF2GCode) {
        return false;
      }
      return true;
    }
  }
};
</script>