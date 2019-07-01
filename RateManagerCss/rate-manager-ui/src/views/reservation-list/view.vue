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
                                    <b-form-group class="ml-5 mr-3 mb-3" label="Estado">
                                        <b-form-select v-model="form.status" :options="status"></b-form-select>
                                    </b-form-group>
                                    <b-form-group class="mr-3 mb-3" label="Tipo de habitacion">
                                        <b-form-select></b-form-select>
                                    </b-form-group>
                                </b-form>
                                <b-form inline>
                                    <b-form-group class="mr-3 mb-3" label="Nombre del cliente">
                                        <b-form-input v-model="form.clientName"></b-form-input>
                                    </b-form-group>
                                    <b-form-group class="mr-5 mb-3" label="Origen">
                                        <b-form-select></b-form-select>
                                    </b-form-group>
                                    <b-form-group class="mb-3" label="">
                                        <b-button @click="search" variant="primary">Buscar</b-button>
                                    </b-form-group>
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
            :per-page="0"
            :current-page="currentPage"
            :items="fill">
                <div slot="table-busy" class="text-center text-danger my-2">
                    <b-spinner class="align-middle"></b-spinner>
                    <strong>&nbsp;Cargando...</strong>
                </div>
            </b-table>
            <filter></filter>
            <b-pagination aria-controls="my-table" align="right"
            v-model="currentPage"
            @change="pageChange"
            :total-rows="1000"
            :per-page="perPage"
           ></b-pagination>
        </b-container>
    </div>
</template>

<script>
import Filter from './components/Filter.vue';
import reservationService from '../../api/reservation-service';

export default {
    name: 'app',
    components: {
        Filter,
    },
    beforeMount() {
        this.get();
    },
    data() {
        return {
            items: [],
            filter_url: null,
            isBusy: false,
            currentPage: 1,
            perPage: 8,
            totalRows: null,
            fields: {
                confirmNumber: {
                    label: this.$t('Reservation Number'),
                },
                client: {
                    label: this.$t('Client'),
                    sortable: true,
                },
                reservationDate: {
                    label: this.$t('Date'),
                    formatter: 'dateFormat',
                },
                roomCount: {
                    label: this.$t('Rooms'),
                },
                checkIn: {
                    label: this.$t('Checkin'),
                    formatter: 'dateFormat',
                },
                checkOut: {
                    label: this.$t('Checkout'),
                    formatter: 'dateFormat',
                },
                status: {
                    label: this.$t('Status'),
                    formatter: 'statusFormat',
                },
            },
            filter: null,
            byTypeDates: [
                { text: this.$t('Reservation date'), value: 'ReservationDate' },
                { text: this.$t('Arrival date'), value: 'CheckOut' },
                { text: this.$t('Departure date'), value: 'CheckIn' },
            ],
            status: [
                { text: `-- ${this.$t('All')} --`, value: 0 },
                { text: this.$t('Reserved'), value: 1 },
                { text: this.$t('Cancelled'), value: 3 },
                { text: this.$t('In process'), value: 4 },
            ],
            form: {
                byDateType: 'ReservationDate',
                dateRange: null,
                status: 0,
                clientName: null,
            },
        };
    },
    computed: {
        fill() {
            return this.items;
        },
        rows() {
            return this.totalRows;
        },
    },
    methods: {
        get() {
            this.filter_url === '' ? null : this.filter_url;
            this.isBusy = true;

            reservationService.GetAll(this.filter_url, this.currentPage, this.perPage).then((response) => {
                console.log(response);
                this.totalRows = response.headers.map.x - total - count[0];
                this.items = response.body;
                this.isBusy = false;
            });
        },
        pageChange(page) {
            this.currentPage = page;
            this.get();
        },
        dateFormat(value) {
            return this.$moment(value).format('D MMM YYYY');
        },
        statusFormat(value) {
            return this.status.filter(s => s.value === value)[0].text;
        },
        rowClass(item, type) {
            if (!item) return;
            if (item.status === 3) return 'table-danger';
        },
        search() {
            this.filter_url = '';

            if (this.form.dateRange != null) {
                this.filter_url += `${this.form.byDateType} gt ${this.$moment(this.form.dateRange.start).format('YYYY-MM-DD')
                } and ${this.form.byDateType} lt ${this.$moment(this.form.dateRange.end).format('YYYY-MM-DD')}`;
            }

            if (this.form.status != 0) this.filter_url += `${this.filter_url != '' ? ' and ' : ''}Status eq ${this.form.status}`;

            if (this.form.clientName != null && this.form.clientName != '') this.filter_url += `${this.filter_url != '' ? ' and ' : ''}Client lk ${this.form.clientName}`;

            this.get();
        },
    },
};
</script>
