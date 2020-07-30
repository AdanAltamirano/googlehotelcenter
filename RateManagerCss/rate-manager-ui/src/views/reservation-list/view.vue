<template>
    <!-- eslint-disable -->
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Reservation List') }}</h2>
            <advanced-filter
            :item-per-page="itemPerPage"
            :items-per-page="itemsPerPage"
            :result="result"
            :corporates="corporates"
            @changeItems="changeItems"
            @search="search">
            </advanced-filter>

            <data-table
            table-id="rsv_table"
            :columns="fields"
            :resource-function="get"
            :filter="filter_url"
            :items-per-page="itemPerPage"
            sort-by="reservationDate"
            :sort-desc="true"
            :small="true">
                <template v-slot:cell(idconfirmNumber)="data">
                    <b-link
                    target="_blank"
                    :href=" $appConfig.basePath + '/rate-manager-ui/dist/reservation-details.aspx?qs=' + data.item.id">
                        {{ data.item.confirmNumber }}
                    </b-link>
                </template>
                <template v-slot:cell(client)="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:300px;">{{ data.value }}</span>
                </template>
                <template v-slot:cell(hotel)="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:150px;">{{ data.value }}</span>
                </template>
                <template v-slot:cell(corporateId)="data">
                    <span>{{ getCorporateName(data.value) }}</span>
                </template>
                <template v-slot:cell(status)="data">
                    <b-badge v-if="data.value == 1" variant="success">{{ $t('Reserved') }}</b-badge>
                    <b-badge v-if="data.value == 3" variant="danger">{{ $t('Cancelled') }}</b-badge>
                    <b-badge v-if="data.value == 4" variant="warning">{{ $t('In process') }}</b-badge>
                </template>
                <template v-slot:cell(portal)="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:120px;">{{ data.value }}</span>
                </template>
            </data-table>
        </b-container>
    </div>
    <!-- eslint-enable -->
</template>

<script>
//import XLSX from "xlsx";
import AdvancedFilter from './components/AdvancedFilter.vue';
import DataTable from '../../components/data-table.vue';
import ReservationService from '../../api/reservation-service';

export default {
    name: "app",
    components: {
        AdvancedFilter,
        DataTable
    },
    mounted() {
        this.getCorporate();
        this.$root.$on('table-result', val => {
            this.result = val;
        });
    },
    created() {
        this.filter_url = this.defaultSearch();
    },
    data() {
        return {
            result: [],
            filter_url: null,
            fields: [
                {
                    key: 'idconfirmNumber',
                    label: '#'
                },
                {
                    key: 'hotel',
                    label: 'Hotel'
                },
                {
                    key: 'client',
                    label: this.$t('Client')
                },
                {
                    key: 'reservationDate',
                    label: this.$t('Date'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true,
                    sortDirection: 'desc'
                },
                {
                    key: 'checkIn',
                    label: this.$t('Checkin'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true
                },
                {
                    key: 'checkOut',
                    label: this.$t('Checkout'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true
                },
                {
                    key: 'portal',
                    label: this.$t('Origin')
                },
                {
                    key: 'corporateId',
                    label: this.$t('Corporate')
                },
                {
                    key: 'total',
                    label: 'Total',
                    sortable: true
                },
                {
                    key: 'status',
                    label: this.$t('Status'),
                    sortable: true
                }
            ],
            itemPerPage: 20,
            itemsPerPage: [20, 50, 100, 200],
            corporates: []
        };
    },
    methods: {
        get(filter, orderBy, pageSize, page) {
            console.log(window.app);
            return ReservationService.GetAll(filter, orderBy, pageSize, page);
        },
        getCorporate() {
            ReservationService.GetCorporate().then(response => {
                this.corporates = response.body;
            });
        },
        search(filter) {
            this.filter_url = filter;
            this.$root.$emit('bv::refresh::table', 'rsv_table');
        },
        getCorporateName(corporateId) {
            const corp = this.corporates.find(x => x.idCorporativo == corporateId);
            return corp !== undefined ? corp.nombreCorp : '';
        },
        changeItems(value) {
            this.itemPerPage = value;
        },
        defaultDates() {
            const start = new Date();
            start.setMonth(start.getMonth() - 1);
            return {
                start: start,
                end: new Date()
            };
        },
        defaultSearch() {
            const x = this.defaultDates();
            const s = `Status eq 1 and ReservationDate gt ${this.$moment(x.start).format("YYYY-MM-DD")} 
            and ReservationDate lt ${this.$moment(x.end).format("YYYY-MM-DD")}`;
            return s;
        }
    }
};
</script>
