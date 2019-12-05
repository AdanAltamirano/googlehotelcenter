<template>
  <div>
    <div class="d-flex justify-content-between">
      <h2 class="mt-3 text-primary">{{$t('Configuration Active Rates Plans')}}</h2>
      <b-button @click="alertRates" size="sm" style="border:none; outline:none; background:none;">
        <i class="fas fa-question-circle fa-lg" style="color:#10467a !important"></i>
      </b-button>
      <!-- <b-button v-b-modal.modals-1 size="sm" style="border:none; outline:none; background:none;">
        <i class="fas fa-question-circle fa-lg" style="color:#10467a !important"></i>
      </b-button>
      <b-modal id="modals-1" title="BootstrapVue" :hide-footer="hideFooter">
        <p class="my-4">Hello from modal!</p>
      </b-modal>-->
    </div>
    <div v-if="!isApiEnd" class="vld-parent" style="height:200px">
      <loading :active="true" :is-full-page="false" color="#007bff"></loading>
    </div>
    <div v-else>
      <div v-if="ratesPlans.length > 0" :class="{'rates-class':ratesPlans.length >= 6}">
        <b-row>
          <b-col md="12">
            <div class="accordion my-2" id="accordionRatePlans">
              <div v-for="item in ratesPlans" :key="item.id" class="card">
                <div class="card-header" :id="'heading'+item.id">
                  <h2 class="mb-0">
                    <button
                      class="btn btn-link"
                      type="button"
                      data-toggle="collapse"
                      :data-target="'#target' + item.id"
                      aria-expanded="false"
                      :aria-controls="'target' + item.id"
                    >{{item.id}}</button>
                  </h2>
                </div>
                <!-- /.card-header -->
                <div
                  :id="'target'+ item.id"
                  class="collapse"
                  :aria-labelledby="'heading'+item.id"
                  data-parent="#accordionRatePlans"
                >
                  <div class="card-body">
                    <b-table
                      class="my-2"
                      show-empty
                      striped
                      bordered
                      hover
                      responsive
                      :fields="fields"
                      :small="true"
                      :items="item.listProperties"
                      :tbody-tr-class="rowClass"
                    >
                      <template slot="empty" slot-scope="scope">
                        <h4>{{scope.emptyText}}</h4>
                      </template>
                      <template slot="status" slot-scope="data">
                        <b-badge v-if="data.value == 200" variant="success">{{$t('Accepted')}}</b-badge>
                        <b-badge v-if="data.value == 300" variant="warning">{{$t('Warning')}}</b-badge>
                        <b-badge v-if="data.value == 400" variant="danger">{{$t('Error')}}</b-badge>
                      </template>
                    </b-table>
                  </div>
                  <!-- /.card-body -->
                </div>
              </div>
              <!-- /.card -->
            </div>
          </b-col>
        </b-row>
      </div>
      <div v-else>
        <h5>{{$t('No Rate Plans')}}</h5>
      </div>
    </div>
  </div>
</template>
<script>
import configService from "../../../api/config-service";
import Loading from "vue-loading-overlay";
import image from "../assets/ratesConfiguration.png";

export default {
  name: "rates-configuration",
  components: {
    Loading
  },
  beforeCreate() {
    configService
      .GetRates(this.$appConfig.session.hotelId)
      .then(result => {
        console.log(result.body);
        this.ratesPlans = result.body;
        this.isApiEnd = true;
      })
      .catch(err => {
        this.isApiEnd = true;
      });
  },
  data() {
    return {
      image: image,
      hideFooter: true,
      sortBy: "status",
      sortDesc: false,
      isApiEnd: false,
      ratesPlans: [],
      fields: [
        {
          key: "idProperty",
          label: "#"
        },
        {
          key: "nameProperty",
          label: this.$t("Property")
        },
        {
          key: "message",
          label: this.$t("Message")
        },
        {
          key: "status",
          label: this.$t("Status"),
          sortable: true
        }
      ]
    };
  },
  methods: {
    rowClass(item, type) {
      if (!item) return;
      if (item.status === 200) return ["table-success", "success-text"];
      else if (item.status === 300) return ["table-warning", "warning-text"];
      else return ["table-danger", "error-text"];
    },
    alertRates() {
      let rates = {
        width: 800,
        imageUrl: "rate-manager-ui/dist/" + this.image,
        imageAlt: this.$t("Configuration"),
        confirmButtonText: "Salir",
        confirmButtonColor: "#d33"
      };
      this.$appAlert(rates);
    }
  }
};
</script>
