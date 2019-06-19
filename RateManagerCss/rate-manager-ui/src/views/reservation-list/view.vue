<template>
    <div id="app">
        <b-container fluid>
            <advanced-filter></advanced-filter>
            <b-table class="my-3" striped borderless small :busy="isBusy" :filter="filter" :per-page="perPage" :current-page="currentPage" :items="listReservation">
                <div slot="table-busy" class="text-center text-danger my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>&nbsp;Cargando...</strong>
                </div>
            </b-table>

            <b-pagination v-model="currentPage" :total-rows="rows" :per-page="perPage" aria-controls="my-table"></b-pagination>
        </b-container>
    </div>
</template>

<script>
import Filter from './components/Filter.vue';

export default {
    name: 'app',
    components: {
        Filter,
    },
    beforeMount() {
        this.$store.commit('GetAllReservations');
    },
    mounted() {
        this.isBusy = true;
    },
    data() {
        return {
            isBusy: false,
            currentPage: 1,
            perPage: 5,
            fields: [
                {
                    key: 'client',
                    sortable: true,
                },
            ],
            filter: null,
        }
    },
    computed: {
        listReservation() {
            this.isBusy = false;
            return this.$store.getters.reservations;
        },
        rows() {
            return this.$store.getters.reservations.length;
        },
    },
};
</script>
