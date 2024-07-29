<template>
    <!-- eslint-disable -->
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Reservation List') }}</h2>
            <advanced-filter :item-per-page="itemPerPage" :items-per-page="itemsPerPage" :result="result"
                :corporates="corporates" @changeItems="changeItems" @search="search">
            </advanced-filter>

            <data-table table-id="rsv_table" :columns="fields" :resource-function="get" :filter="filter_url"
                :items-per-page="itemPerPage" sort-by="reservationDate" :sort-desc="true" :small="true">
                <template v-slot:cell(idconfirmNumber)="data">
                    <b-link target="_blank"
                        :href="$appConfig.basePath + '/rate-manager-ui/dist/reservation-details.aspx?qs=' + data.item.id">
                        <span v-tooltip="data.item.confirmNumber" class="d-block text-truncate" style="width:100px;">{{
                            data.item.confirmNumber }}</span>
                    </b-link>
                </template>
                <template v-slot:cell(client)="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:300px;">{{ data.value
                        }}</span>
                </template>
                <template v-slot:cell(corporateId)="data">
                    <span>{{ getCorporateName(data.value) }}</span>
                </template>
                <template v-slot:cell(nigths)="data">
                    <span>{{ getNigthStay(data.item) }}</span>
                </template>
                <template v-slot:cell(rate)="data">
                    <span>{{ parseFloat(getAverageRate(data.item)).toFixed(2) }} {{ getCurrency(data.item.total)
                        }}</span>
                </template>
                <template v-slot:cell(subtotal)="data">
                    <span>{{ parseFloat(getSubtotal(data.item)).toFixed(2) }} {{ getCurrency(data.item.total) }}</span>
                </template>
                <template v-slot:cell(comission)="data">
                    <span>{{ parseFloat(getComission(data.item)).toFixed(2) }} {{ getCurrency(data.item.total) }}</span>
                </template>
                <template v-slot:cell(tax)="data">
                    <span>{{ parseFloat(getTotalTax(data.item)).toFixed(2) }} {{ getCurrency(data.item.total) }}</span>
                </template>
                <template v-slot:cell(status)="data">
                    <b-badge v-if="data.value == 1" variant="success">{{ $t('Reserved') }}</b-badge>
                    <b-badge v-if="data.value == 3" variant="danger">{{ $t('Cancelled') }}</b-badge>
                    <b-badge v-if="data.value == 4" variant="warning">{{ $t('In process') }}</b-badge>
                </template>
                <template v-slot:cell(portal)="data">
                    <span v-tooltip="data.value" class="d-block text-truncate" style="width:120px;">{{ data.value
                        }}</span>
                </template>
            </data-table>
        </b-container>
    </div>
    <!-- eslint-enable -->
</template>

<script>
//import XLSX from "xlsx";
import AdvancedFilter from './components/AdvancedReportFilter.vue';
//import ModifyForm from './components/ModifyComissions.vue';
import DataTable from '../../components/data-table.vue';
import ReservationService from '../../api/reservation-service';

export default {
    name: "app",
    components: {
        AdvancedFilter,
        DataTable
        //ModifyForm
    },
    mounted() {
        this.getCorporate();
        this.getHotelChannels();
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
                // { key: 'hotel', label: 'Hotel' },
                {
                    key: 'client',
                    label: this.$t('Client')
                },
                {
                    key: 'reservationDate',
                    label: this.$t('Res. Date'),
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
                    key: 'guests',
                    label: this.$t('Guests')
                },
                {
                    key: 'nigths',
                    label: this.$t('Nights')
                },
                {
                    key: 'roomCount',
                    label: this.$t('Rooms')
                },
                {
                    key: 'portal',
                    label: this.$t('Channel')
                },
                // { key: 'corporateId', label: this.$t('Corporate') }, { key: 'paymentMethod', label: this.$t('Payment Way') },
                {
                    key: 'rate',
                    label: this.$t('Rate'),
                    sortable: true
                },

                {
                    key: 'subtotal',
                    label: 'Subtotal',
                    sortable: true
                },
                {
                    key: 'tax',
                    label: this.$t('Tax'),
                    sortable: true
                },
                {
                    key: 'comission',
                    label: this.$t('Comission'),
                    sortable: true
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
            corporates: [],
            hotelChannels: []
        };
    },
    methods: {
        get(filter, orderBy, pageSize, page) {
            /*console.log('Api Response:');
            console.log(window.app);*/
            return ReservationService.GetAll(filter, orderBy, pageSize, page);
        },
        getCorporate() {
            ReservationService.GetCorporate().then(response => {
                this.corporates = response.body;
                console.log(this.corporates);
            });
        },
        search(filter) {
            this.filter_url = filter;
            this.$root.$emit('bv::refresh::table', 'rsv_table');
        },
        getHotelChannels() {
            ReservationService.GetHotelChannels(this.$appConfig.session.hotelId).then(response => {
                this.hotelChannels = response.body;
                console.log("Hotel Channels:");
                console.log(this.hotelChannels);
            });
        },
        getNigthStay(reservation) {
            return reservation.nigths * reservation.roomCount;
        },
        getCurrency(total) { // Currency
            return total.replace(/[0-9]/g, '').replace('.', '');
        },
        getReservationTotal(total) { // Total sin currency --> 100.50MXN --> 100.50
            return total.replace(/[^\d.-]/g, '');
        }
        ,
        getTotalTax(reservation) { // Impuestos
            return this.getReservationTotal(reservation.total) - this.getSubtotal(reservation);
        },
        getSubtotal(reservation) { // Total sin impuestos

            if (reservation.tax > 0) {
                return this.getReservationTotal(reservation.total) / (1 + (reservation.tax / 100));
            }
            else {
                return this.getReservationTotal(reservation.total);
            }
        },
        getAverageRate(reservation) { // Tarifa promedio
            //console.log('Data Objetc:');
            //console.log(data);
            return this.getSubtotal(reservation) / this.getNigthStay(reservation);
        },
        getComission(reservation) {

            var result = 0;

            if (this.hotelChannels.length > 0) {
                for (var x = 0; x < this.hotelChannels.length; x++) {
                    if (reservation.portal.toUpperCase().includes("-") && this.hotelChannels[x].nombre === "Portal Internet Power") {
                        result = this.getReservationTotal(reservation.total) * (this.hotelChannels[x].comision / 100)
                        continue;
                    } else {
                        if (reservation.portal.replace(/\s/g, '').toUpperCase().includes(this.hotelChannels[x].nombre.replace(/\s/g, '').toUpperCase())) {
                            result = this.getReservationTotal(reservation.total) * (this.hotelChannels[x].comision / 100)
                            continue;
                        }
                    }
                }
            }

            return result;
        }
        ,
        getCorporateName(corporateId) {

            const corp = this.corporates.find(x => x.idCorporativo == corporateId);

            if (corp !== undefined) {
                if (corp.nombreCorp.includes(":")) {
                    let splitCorpName = corp.nombreCorp.split(":");

                    return splitCorpName[1];
                }

                return corp.nombreCorp
            }

            return '';

            //return corp !== undefined ? corp.nombreCorp : '';
        },
        changeItems(value) {
            this.itemPerPage = value;
        },
        defaultDates() {
            const start = new Date();
            start.setMonth(start.getMonth() - 1);

            let dateEnd = new Date();
            dateEnd.setDate(dateEnd.getDate() + 1);

            return {
                start: start,
                end: dateEnd
            };
        },
        defaultSearch() {
            const x = this.defaultDates();
            const s = `Status eq 1 and ReservationDate ge ${this.$moment(x.start).format("YYYY-MM-DD")} 
            and ReservationDate lt ${this.$moment(x.end).format("YYYY-MM-DD")}`;
            return s;
        }
    }
};
</script>
