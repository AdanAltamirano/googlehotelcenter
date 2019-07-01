<template>
    <div>
        <b-table v-bind="{ $scopedSlots }" class="my-2" show-empty striped bordered hover responsive
        :fields="columns"
        :items="providerFunction"
        :per-page="itemsPerPage"
        :current-page="currentPage"
        :busy.sync="isBusy"
        :id="tableId">
            <template slot="table-busy">
                <div class="vld-parent" style="height:200px">
                    <loading :active="true"
                        :is-full-page="false"
                        color="#007bff">
                    </loading>
                </div>
            </template>
            <template slot="empty" slot-scope="scope">
                <h4>{{scope.emptyText}}</h4>
            </template>
        </b-table>
        <b-pagination align="right" :total-rows="totalRows" :per-page="itemsPerPage" v-model="currentPage" class="my-0" />
    </div>
</template>

<script>
// Import component
import Loading from 'vue-loading-overlay';

export default {
    name: 'data-table',
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
        filter:{
            type: String,
            required: false
        },
        itemsPerPage:{
            type: Number,
            required: true
        },
        tableId:{
            type: String,
            required: false
        }
    },
    data() {
        return {
            result: [],
            currentPage: 1,
            totalRows: 0,
            isBusy: false,
            emptyText: 'No hay registros que coincidan con su solicitud'
        }
    },
    methods: {
        /**
        * da el formato correcto para NinjAPI al orden de b-table (vue-bootstrap)
        * @param {String} sortBy columna por la que se hace el ordenamiento
        * @param {Boolean} sortDesc direccion de ordenamiento (true para descending, false para ascending)
        * @return {String} orden con formato de NinjAPI
        */
        formatOrder(sortBy, sortDesc) {
            if (!sortBy) return '';
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
            ).then((response) => {
                // establecer el total de elementos
                this.totalRows = Number(response.headers.map['x-total-count'][0]);
                // prooveer el arreglo de elementos
                callback(response.body);
                this.result = response.body;
                // marcar como que ya no está ocupado
                this.hideLoader();
            }).catch(error => {
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
            //this.loader = this.$loading.show({ color: '#007bff', height: 64, width: 64, isFullPage: false, container: this.$refs.loadingContainer, });
        },
        /**
         * ocultar la animacion de loading
         */
        hideLoader() {
            this.isBusy = false;
        }
    },
    watch:
    {
        result: function(val) {
            this.$root.$emit('table-result', val);   
        }
    }

}
</script>
