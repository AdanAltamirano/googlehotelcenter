<template>
<!-- eslint-disable -->
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{$t('Reservation List')}}</h2>
            <advanced-filter
            :item-per-page="itemPerPage"
            :items-per-page="itemsPerPage"
            :result="result"
            @exportToExcel="exportToExcel"
            @changeItems="changeItems"
            @search="search"></advanced-filter>

            <data-table table-id="rsv_table"
            :columns="fields"
            :resource-function="get"
            :filter="filter_url"
            :items-per-page="itemPerPage"
            sort-by="reservationDate"
            :sort-desc="true"
            :small="true">
                <template slot="idconfirmNumber" slot-scope="data">
                    <b-link target="_blank" :href="'/ratemanager/rate-manager-ui/dist/reservation-details.aspx?qs=' + data.item.id">{{data.item.confirmNumber}}</b-link>
                </template>
                <template slot="client" slot-scope="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:300px;">{{data.value}}</span>
                </template>
                <template slot="hotel" slot-scope="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:150px;">{{data.value}}</span>
                </template>
                <template slot="status" slot-scope="data">
                    <b-badge v-if="data.value == 1" variant="success">{{$t('Reserved')}}</b-badge>
                    <b-badge v-if="data.value == 3" variant="danger">{{$t('Cancelled')}}</b-badge>
                    <b-badge v-if="data.value == 4" variant="warning">{{$t('In process')}}</b-badge>
                </template>
                <template slot="portal" slot-scope="data">
                     <span v-tooltip="data.value" class="d-block text-truncate" style="width:120px;">{{data.value}}</span>
                </template>
            </data-table>
        </b-container>
    </div>
<!-- eslint-enable -->
</template>

<script>
import XLSX from 'xlsx';
import AdvancedFilter from './components/AdvancedFilter.vue';
import DataTable from '../../components/data-table.vue';
import ReservationService from '../../api/reservation-service';

export default {
    name: 'app',
    components: {
        AdvancedFilter,
        DataTable,
    },
    mounted() {
        this.$root.$on('table-result', (val) => {
            this.result = val;
            this.excelFormat();
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
                    label: '#',
                },
                {
                    key: 'hotel',
                    label: 'Hotel',
                },
                {
                    key: 'client',
                    label: this.$t('Client'),
                },
                {
                    key: 'reservationDate',
                    label: this.$t('Date'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true,
                    sortDirection: 'desc',
                },
                {
                    key: 'checkIn',
                    label: this.$t('Checkin'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true,
                },
                {
                    key: 'checkOut',
                    label: this.$t('Checkout'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                    sortable: true,
                },
                {
                    key: 'portal',
                    label: this.$t('Origin'),
                },
                {
                    key: 'status',
                    label: this.$t('Status'),
                    sortable: true,
                },
            ],
            itemPerPage: 20,
            itemsPerPage: [20, 50, 100, 200],
        };
    },
    methods: {
        get(filter, orderBy, pageSize, page) {
            return ReservationService.GetAll(filter, orderBy, pageSize, page);
        },
        search(filter) {
            this.filter_url = filter;
            this.$root.$emit('bv::refresh::table', 'rsv_table');
        },
        excelFormat() {
            const bkResult = this.result;
            this.result = [];
            bkResult.forEach((value) => {
                const row = {};
                this.fields.forEach((valueF) => {
                    if (valueF.key in value) {
                        let val = value[valueF.key];

                        if ('formatter' in valueF) {
                            val = this.$moment(val).format('D MMM YYYY');
                            row[valueF.label] = val;
                        }

                        if (valueF.key === 'status') {
                            switch (val) {
                            case 1: val = this.$t('Reserved'); break;
                            case 3: val = this.$t('Cancelled'); break;
                            case 4: val = this.$t('In process'); break;
                            default: val = '';
                            }
                        }
                        row[valueF.label] = val;
                    }
                });
                this.result.push(row);
            });
        },
        exportToExcel() {
            // only array possible
            const data = XLSX.utils.json_to_sheet(this.result);

            // a workbook is the name given to an excel file
            const wb = XLSX.utils.book_new(); // make workbook of excel

            // add worksheet to workbook
            // workbook contains one or more worksheets
            XLSX.utils.book_append_sheet(wb, data, this.$t('Reservation List'));

            // export excel file
            XLSX.writeFile(wb, 'reservaciones.xlsx'); // name of the file
        },
        changeItems(value) {
            this.itemPerPage = value;
        },
        defaultDates() {
            const start = new Date();
            start.setMonth(start.getMonth() - 1);
            return {
                start: start,
                end: new Date(),
            }
        },
        defaultSearch() {
            const x = this.defaultDates();
            const s = `Status eq 1 and ReservationDate gt ${this.$moment(x.start).format('YYYY-MM-DD')} 
            and ReservationDate lt ${this.$moment(x.end).format('YYYY-MM-DD')}`;
            return s;
        },
    },
};
</script>
