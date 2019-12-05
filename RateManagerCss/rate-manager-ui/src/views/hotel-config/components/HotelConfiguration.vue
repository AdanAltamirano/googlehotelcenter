<template>
  <div>
    <h2 class="text-primary">{{$t('Configuration Hotel')}}</h2>
    <b-table
      class="my-2"
      show-empty
      striped
      bordered
      hover
      responsive
      :fields="fields"
      :small="true"
      :items="getError"
      :busy.sync="isBusy"
      :tbody-tr-class="rowClass"
      :sort-by="sortBy"
      :sort-desc="sortDesc"
    >
      <template slot="table-busy">
        <div class="vld-parent" style="height:200px">
          <loading :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
      </template>
      <template slot="empty" slot-scope="scope">
        <h4>{{scope.emptyText}}</h4>
      </template>
      <template slot="message" slot-scope="data">
        <div class="d-flex justify-content-between">
          {{data.value}}
          <b-button
            @click="showAlert(data.item.idProperty)"
            style="background:none; color:inherit; border:none; outline:none;"
            size="sm"
          >
            <i class="fas fa-question-circle fa-lg"></i>
          </b-button>
          <!-- <b-button
            style="background:none; color:inherit; border:none; outline:none;"
            size="sm"
            v-b-modal="'modal-' + data.item.idProperty"
          >
            <i class="fas fa-question-circle fa-lg"></i>
          </b-button>
          <b-modal
            :id="'modal-' + data.item.idProperty"
            ref="modal"
            title="Edit subnet"
            @ok="handleOk"
            :hide-footer="hideFooterModal"
          >
            <b-img src="https://picsum.photos/1024/400/?image=41" fluid alt="Responsive image"></b-img>
          </b-modal>-->
        </div>
      </template>
      <template slot="status" slot-scope="data">
        <b-badge v-if="data.value == 200" variant="success">{{$t('Accepted')}}</b-badge>
        <b-badge v-if="data.value == 300" variant="warning">{{$t('Warning')}}</b-badge>
        <b-badge v-if="data.value == 400" variant="danger">{{$t('Error')}}</b-badge>
      </template>
    </b-table>
  </div>
</template>
<script>
import configService from "../../../api/config-service";
import Loading from "vue-loading-overlay";
import cardsImage from "../assets/cards.png";
import depositAndAvailabilityImage from "../assets/depositAndAvailability.png";
import portalImage from "../assets/portal.png";
import numberPropertyImage from "../assets/numberProperty.png";
export default {
  name: "hotel-configuration",
  components: {
    Loading
  },
  props: {
    id: {
      type: String | Number
    }
  },
  data() {
    return {
      propertiesImages: [
        depositAndAvailabilityImage,
        portalImage,
        depositAndAvailabilityImage,
        numberPropertyImage,
        cardsImage
      ],
      hideFooterModal: true,
      sortBy: "status",
      sortDesc: false,
      isBusy: false,
      hotelId: this.$appConfig.session.hotelId,
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
    getError(ctx, callback) {
      this.showLoader();
      configService
        .GetErrors(this.hotelId)
        .then(result => {
          if (ctx.sortDesc)
            result.body.sort((a, b) => {
              return b.status - a.status;
            });
          else
            result.body.sort((a, b) => {
              return a.status - b.status;
            });
          callback(result.body);
          this.hideLoader();
        })
        .catch(err => {
          callback([]);
          this.hideLoader();
        });
    },
    /**
     * Mostrar la animacion de loading
     */
    showLoader() {
      this.isBusy = true;
    },
    /**
     * ocultar la animacion de loading
     */
    hideLoader() {
      this.isBusy = false;
    },
    rowClass(item, type) {
      if (!item) return;
      if (item.status === 200) return ["table-success", "success-text"];
      else if (item.status === 300) return ["table-warning", "warning-text"];
      else return ["table-danger", "error-text"];
    },
    handleOk(bvModalEvt) {
      // Prevent modal from closing
      bvModalEvt.preventDefault();
      // Trigger submit handler
      this.handleSubmit();
    },
    showAlert(id) {
      let hotelAlert = {
        width: 800,
        imageUrl: "rate-manager-ui/dist/" + this.propertiesImages[id - 1],
        imageAlt: this.$t("Configuration"),
        confirmButtonText: "Salir",
        confirmButtonColor: "#d33"
      };

      this.$appAlert(hotelAlert);
    }
  }
};
</script>