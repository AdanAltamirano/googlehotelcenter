<template>
    <div id="app">
        <b-container fluid>
            <b-card title="busqueda">
                <b-row>
                    <b-col md="6" class="my-1">
                        <b-input-group>
                            <b-form-input placeholder="No. de confirmacion"></b-form-input>
                            <b-input-group-append>
                                <b-button>Limpiar</b-button>
                            </b-input-group-append>
                        </b-input-group>
                    </b-col>
                    <b-col md="6" class="my-1">
                        <b-button class="float-right" v-b-toggle.filter_content variant="primary">busqueda avanzada</b-button>
                    </b-col>
                </b-row>
                <b-row>
                    <b-col md="12">
                        <b-collapse id="filter_content">
                            <b-card class="mt-3">
                                <b-form inline>
                                    <b-form-group class="mr-3 mb-3" label="Buscar por">
                                        <b-form-select v-model="form.byDateType" :options="byTypeDates"></b-form-select>
                                    </b-form-group>
                                    <b-form-group class="mb-3" label="Rango de fechas">
                                        <v-date-picker v-model="form.dateRange" mode="range"
                                        :popover="{ placement: 'bottom', visibility: 'click' }"
                                        :columns="2"></v-date-picker>
                                    </b-form-group>
                                </b-form>
                                <b-form inline>
                                    <b-button @click="search" variant="primary">Buscar</b-button>
                                </b-form>
                            </b-card>
                        </b-collapse>
                    </b-col>
                </b-row>
            </b-card>
            <b-table class="my-3" striped bordered borderless small 
            :tbody-tr-class="rowClass" 
            :fields="fields" 
            :busy="isBusy" 
            :filter="filter" 
            :per-page="perPage" 
            :current-page="currentPage" 
            :items="listReservation">
                <div slot="table-busy" class="text-center text-danger my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>&nbsp;Cargando...</strong>
                </div>
            </b-table>

            <b-pagination align="right" v-model="currentPage" :total-rows="rows" :per-page="perPage" aria-controls="my-table"></b-pagination>
        </b-container>
    </div>
</template>

<script>
import FilterAdv from './components/Filter.vue';

export default {
    name: 'app',
    components: {
        FilterAdv
    },
    beforeMount() {
        this.$store.commit('GetAllReservationsById');
    },
    mounted() {
        this.isBusy = true;
    },
    data() {
        return {
            isBusy: false,
            currentPage: 1,
            perPage: 5,
            fields: {
                confirmNumber: {
                    label: this.$t('Reservation Number')
                },
                client: {
                    label: this.$t('Client')
                },
                reservationDate: {
                    label: this.$t('Date'),
                    formatter: 'dateFormat'
                },
                roomCount: {
                    label: this.$t('Rooms')
                },
                checkIn: {
                    label: this.$t('Checkin'),
                    formatter: 'dateFormat'
                },
                checkOut: {
                    label: this.$t('Checkout'),
                    formatter: 'dateFormat'
                },
                status: {
                    label: this.$t('Status'),
                    formatter: 'statusFormat'
                }
            },
            filter: null,
            byTypeDates: [
                {text:'Fecha de Reservacion', value: 'ReservationDate'},
                {text:'Fecha llegada', value: 'CheckOut'},
                {text:'Fecha salida', value: 'CheckIn'}
            ],
            form: {
                byDateType: 'ReservationDate',
                dateRange: null,
            }
        }
    },
    computed: {
        listReservation() {
            this.isBusy = false;
            console.log('all-reservation', this.$store.getters.reservations);
            return this.$store.getters.reservations;
        },
        rows() {
            return this.$store.getters.reservations.length;
        },
    },
    methods: {
        dateFormat(value) {
            return this.$moment(value).format('D MMM YYYY');
        },
        statusFormat(value) {
            let status = '';
            switch(value) {
                case 1: 
                    status = this.$t('Reserved');
                    break;
                case 3:
                    status = this.$t('Cancelled');
                    break;
                case 4:
                    status = this.$t('In process');
                    break;
            }
            return status;
        },
        rowClass(item, type) {
            if (!item) return;
            if (item.status === 3) return 'table-danger';
        },
        search()
        {
            let filter = '';
            filter += this.form.byDateType + ' gt ' + this.$moment(this.form.dateRange.start).format('YYYY-MM-DD') +
            ' and ' + this.form.byDateType + ' lt ' + this.$moment(this.form.dateRange.end).format('YYYY-MM-DD');
            this.$store.commit('GetAllReservationsById', filter);
        }
    }
};
</script>
