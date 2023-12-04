<template>
  <!-- eslint-disable -->
  <div>
    <b-table
      v-bind="{ $scopedSlots }"
      :class="`my-2 ${customClass}`"
      show-empty
      striped
      bordered
      hover
      responsive
      :fields="columns"
      :items="providerFunction"
      :per-page="itemsPerPage"
      :current-page="currentPage"
      :busy.sync="isBusy"
      :id="tableId"
      :empty-text="emptyText"
      :small="small"
      :sort-by="sortBy"
      :sort-desc="sortDesc"
      :tbody-tr-class="rowClass">
      <template slot="table-busy">
        <div class="vld-parent" style="height:200px">
          <loading :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
      </template>
      <template slot="empty" slot-scope="scope">
        <h4>{{scope.emptyText}}</h4>
      </template>
    </b-table>
    <div class="d-flex">
      <!-- <span>Mostrando del {{currentPage}} al {{totalPages}} de {{totalRows}} {{tableTypeResults}}</span> -->
      <span>Mostrando del {{start}} al {{end}} de {{totalRows}} {{tableTypeResults}} </span>
      <b-pagination
        align="right"
        style="margin-left:auto !important;"
        :total-rows="totalRows"
        :per-page="itemsPerPage"
        v-model="currentPage"
        class="my-0"
      />
    </div>
  </div>
  <!-- eslint-enable -->
</template>

<script>
// Import component
import Loading from "vue-loading-overlay";

export default {
    name: "data-table",
    components: {
        Loading
    },
    props: {
        columns: {
            type: Array,
            required: true
        },
    resourceFunction: {
      type: Function,
      required: false
    },
    filter: {
      type: String,
      required: false
    },
    itemsPerPage: {
      type: Number,
      required: true
    },
    tableId: {
      type: String,
      required: false
    },
    sortBy: {
      type: String,
      required: false
    },
    sortDesc: {
      type: Boolean,
      required: false
    },
    small: {
      type: Boolean,
      required: false
    },
    customClass:{
      type: String,
      required: false,
      default:''
    },
    rowClass:{
      type:String,
      required: false,
      default:''
    },
    tableTypeResults:{
      type:String,
      required: false,
      default:'reservas'
    },
    newProperties:{
      type:Object,
      required:false,
      default:null
    }
  },
  data() {
    return {
      result: [],
      currentPage: 1,
      totalRows: 0,
      isBusy: false,
      emptyText: this.$t("There are no records that match your request"),
      start:0,
      end:0
    };
  },
  methods: {
    /**
     * da el formato correcto para NinjAPI al orden de b-table (vue-bootstrap)
     * @param {String} sortBy columna por la que se hace el ordenamiento
     * @param {Boolean} sortDesc direccion de ordenamiento (true para descending, false para ascending)
     * @return {String} orden con formato de NinjAPI
     */
    formatOrder(sortBy, sortDesc) {
      if (!sortBy) return "";
      return `${sortBy} ${sortDesc ? "desc" : "asc"}`;
    },
    /**
     * función proveedora de datos para b-table
     */
    providerFunction(ctx, callback) {
      this.showLoader();
      this.resourceFunction(
        this.filter,
        this.formatOrder(ctx.sortBy, ctx.sortDesc),
        ctx.perPage,
        ctx.currentPage
      )
        .then(response => {
          this.$root.$emit("queryString", [
            this.filter,
            this.formatOrder(ctx.sortBy, ctx.sortDesc),
            ctx.perPage,
            ctx.currentPage
          ]);
          console.log(response);
          console.log(response.body);
          // establecer el total de elementos
          this.totalRows = Number(response.headers.map["x-total-count"][0]);
           
          console.log(this.totalRows);
          if(this.totalRows > 0){
            this.start = 1 + (ctx.perPage * ctx.currentPage) - ctx.perPage;
            this.end = ctx.perPage + (ctx.perPage * ctx.currentPage) - ctx.perPage;
            if(ctx.currentPage === Math.ceil(this.totalRows/this.itemsPerPage))
            {
              this.end -= Math.abs((ctx.perPage * ctx.currentPage) - this.totalRows);
            }
          }
          else{
            this.start = 0;
            this.end = 0;
          }
          //agregar nuevas propiedades a la respuesta
          if(this.newProperties != null){
            let keys = Object.keys(this.newProperties);         
            keys.forEach(key =>{
              console.log(key);
              let value = this.newProperties[key.toString()];
              console.log(value);
              response.body.forEach(element => {
                element[key] = value
              });
            });
          }
          // prooveer el arreglo de elementos
          callback(response.body);
          this.result = response.body;
          // marcar como que ya no está ocupado
          this.hideLoader();
        })
        .catch(() => {
          // TODO: MOSTRAR Error
          // agrear un arreglo sin elementos
          callback([]);
          // marcar como que ya no está ocupado
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
    }
  },
  computed: {
    totalPages() {
      return Math.ceil(this.totalRows / this.itemsPerPage);
    }
  },
  watch: {
    result(val) {
      console.log(val);
      this.$root.$emit("table-result", val);
    }
  }
};
</script>
