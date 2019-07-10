<template>
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
                    <b-link :href="'/RateManager/HotelAdministrator/Pages/ReservationDetails.aspx?qs=' + data.item.id">{{data.item.confirmNumber}}</b-link>
                </template>
                <template slot="hotel" slot-scope="data">
                    <span class="d-block text-truncate" style="width:150px;">{{data.value}}</span>
                </template>
                <template slot="status" slot-scope="data">
                    <b-badge v-if="data.value == 1" variant="success">{{$t('Reserved')}}</b-badge>
                    <b-badge v-if="data.value == 3" variant="danger">{{$t('Cancelled')}}</b-badge>
                    <b-badge v-if="data.value == 4" variant="warning">{{$t('In process')}}</b-badge>
                </template>
                <template slot="portal" slot-scope="data">
                     <span class="d-block text-truncate" style="width:120px;">{{data.value}}</span>
                </template>
            </data-table>
        </b-container>
    </div>
</template>

<script>
import AdvancedFilter from './components/AdvancedFilter.vue';
import DataTable from '../../components/data-table.vue'
import ReservationService from '../../api/reservation-service';
import XLSX from 'xlsx';

export default {
    name: 'app',
    components: {
        AdvancedFilter,
        DataTable
    },
    mounted() {
        this.$root.$on('table-result', (val) => {
            this.result = val;
            this.excelFormat();
        });
    },
    data() {
        return {
            default: {
                once: true,
                orderBy: 'reservationDate desc'
            },
            result: [],
            filter_url: null,
            fields: [
                {
                    key: 'idconfirmNumber',
                    label: '#',//this.$t('Reservation Number')
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
                    formatter: value => {
                        return this.$moment(value).format('D MMM YYYY')
                    },
                    sortable: true,
                    sortDirection: 'desc'
                },
                {
                    key: 'checkIn',
                    label: this.$t('Checkin'),
                    formatter: value => {
                        return this.$moment(value).format('D MMM YYYY')
                    },
                    sortable: true
                },
                {
                    key: 'checkOut',
                    label: this.$t('Checkout'),
                    formatter: value => {
                        return this.$moment(value).format('D MMM YYYY')
                    },
                    sortable: true
                },
                {
                    key: 'portal',
                    label: this.$t('Origin')
                },
                {
                    key: 'status',
                    label: this.$t('Status'),
                    sortable: true
                }
            ],
            itemPerPage: 20,
            itemsPerPage: [20, 50, 100, 200]
        }
    },
    methods: {
        get(filter, orderBy, pageSize, page) {
            if (this.default.once) {
                orderBy = this.default.orderBy;
                this.default.once = false;
            }
            return ReservationService.GetAll(filter, orderBy, pageSize, page);
        },
        search(filter) {
            this.filter_url = filter;
            this.$root.$emit('bv::refresh::table', 'rsv_table');
        },
        excelFormat() {
            let bkResult = this.result;
            this.result = [];
            bkResult.forEach((value, index) => {
                let row = {};
                this.fields.forEach((valueF, indexF) => {
                    if (value.hasOwnProperty(valueF.key)) {
                        let val = value[valueF.key];

                        if (valueF.hasOwnProperty('formatter')) {
                            val = this.$moment(val).format('D MMM YYYY');
                            row[valueF.label] = val;
                        }

                        if (valueF.key === 'status') {
                            switch(val) {
                                case 1: val = this.$t('Reserved'); break;
                                case 3: val = this.$t('Cancelled'); break;
                                case 4: val = this.$t('In process'); break;
                            }
                        }
                        row[valueF.label] = val;
                    }
                });
                this.result.push(row);
            })
        },
        exportToExcel() {
            //only array possible
            var data = XLSX.utils.json_to_sheet(this.result);

            //a workbook is the name given to an excel file
            var wb = XLSX.utils.book_new(); //make workbook of excel

            //add worksheet to workbook
            //workbook contains one or more worksheets
            XLSX.utils.book_append_sheet(wb, data, this.$t('Reservation List'));

            //export excel file
            XLSX.writeFile(wb, 'reservaciones.xlsx'); //name of the file
        },
        changeItems(value) {
            this.itemPerPage = value;
        }
    }
};
</script>
